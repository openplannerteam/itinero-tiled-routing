using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Itinero.IO.Osm.Tiles.Download;
using Itinero.Logging;

namespace Itinero.Tests.Functional.Download
{
    internal class CachedDownloader : IDownloader
    {
        /// <summary>
        /// Gets a stream for the content at the given url.
        /// </summary>
        /// <param name="url">The url.</param>
        /// <returns>An open stream for the content at the given url.</returns>
        public async Task<Stream> Download(string url)
        {
            var fileName = HttpUtility.UrlEncode(url) + ".tile.zip";
            fileName = Path.Combine(".", "cache", fileName);

            var file = new FileInfo(fileName);
            if (!file.Exists)
            {
                try
                {
                    var client = new HttpClient();
                    client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                    var response = await client.GetAsync(url);
                    using (var stream = await response.Content.ReadAsStreamAsync()) 
                    using (var fileStream = File.Open(fileName, FileMode.Create))
                    {
                        stream.CopyTo(fileStream);    
                    }
                
                    Itinero.Logging.Logger.Log(nameof(CachedDownloader), TraceEventType.Verbose, 
                        $"Downloaded from {url}.");
                }
                catch (Exception ex)
                {
                    Itinero.Logging.Logger.Log(nameof(CachedDownloader), TraceEventType.Warning, 
                        $"Failed to download from {url}: {ex}.");
                    return null;
                }
            }  
            
            var diskStream = file.OpenRead();
            if (diskStream.Length == 0)
            {
                return null;
            }
            return new GZipStream(diskStream, CompressionMode.Decompress);
        }
    }
}