using System.IO;
using System.Threading.Tasks;

namespace Itinero.IO.Osm.Tiles.Download
{
    /// <summary>
    /// Abstract definition of a tile downloader.
    /// </summary>
    public interface IDownloader
    {
        /// <summary>
        /// Gets a stream for the content at the given url.
        /// </summary>
        /// <param name="url">The url.</param>
        /// <returns>An open stream for the content at the given url.</returns>
        Task<Stream> Download(string url);
    }
}