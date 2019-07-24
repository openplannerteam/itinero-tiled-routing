using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Itinero.Logging;

namespace Itinero.IO.Osm.Tiles.Download
{
    internal class Downloader : IDownloader
    {
        /// <inheritdoc/>
        public async Task<Stream> Download(string url)
        {
            try
            {
                var client = new HttpClient();
                var response = await client.GetAsync(url);
                return await response.Content.ReadAsStreamAsync();
            }
            catch (Exception)
            {
                Itinero.Logging.Logger.Log(nameof(Downloader), TraceEventType.Warning, 
                    $"Failed to download from {url}.");
                return null;
            }
        }

        private static readonly Lazy<Downloader> DefaultLazy = new Lazy<Downloader>(() => new Downloader());
        
        /// <summary>
        /// Gets the default downloader.
        /// </summary>
        public static Downloader Default => DefaultLazy.Value;
    }
}