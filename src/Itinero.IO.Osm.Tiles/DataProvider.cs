using System.Collections.Generic;
using Itinero.Data.Graphs;
using Itinero.Data.Providers;
using Itinero.IO.Osm.Tiles.Parsers;

namespace Itinero.IO.Osm.Tiles
{
    /// <summary>
    /// Represents a data provider.
    /// </summary>
    public class DataProvider : ILiveDataProvider
    {
        private readonly RouterDb _routerDb;
        private readonly GlobalIdMap _idMap;
        private readonly string _baseUrl;
        private readonly HashSet<uint> _loadedTiles;
        private readonly int _zoom;

        /// <summary>
        /// Creates a new data provider.
        /// </summary>
        /// <param name="routerDb">The routerdb to load data in.</param>
        /// <param name="baseUrl">The base url to load tiles from.</param>
        /// <param name="globalIdMap">The global id map, if any.</param>
        /// <param name="zoom">The zoom level.</param>
        public DataProvider(RouterDb routerDb, string baseUrl = TileParser.BaseUrl,
            GlobalIdMap globalIdMap = null, int zoom = 14)
        {
            _routerDb = routerDb;
            _idMap = globalIdMap;
            if (_idMap == null) _idMap = new GlobalIdMap();
            _baseUrl = baseUrl;
            _zoom = 14;
            
            _loadedTiles = new HashSet<uint>();
        }

        /// <inheritdoc/>
        public bool TouchVertex(VertexId vertexId)
        {
            if (_loadedTiles.Contains(vertexId.TileId))
            { // tile was already loaded.
                return false;
            }

            _loadedTiles.Add(vertexId.TileId);
            return _routerDb.AddOsmTile(_idMap, Tile.FromLocalId(vertexId.TileId, _zoom), _baseUrl);
        }

        /// <inheritdoc/>
        public bool TouchBox((double minLon, double minLat, double maxLon, double maxLat) box)
        {
            // build the tile range.
            var tileRange = new TileRange(box, _zoom);
            
            // get all the tiles and build the router db.
            var updated = false;
            foreach (var tile in tileRange)
            {
                if (_loadedTiles.Contains(tile.LocalId)) continue;
                
                if (_routerDb.AddOsmTile(_idMap, tile, _baseUrl))
                {
                    updated = true;
                }

                _loadedTiles.Add(tile.LocalId);
            }

            return updated;
        }
    }
}