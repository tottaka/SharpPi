using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.IO;

namespace SharpPi.Net
{
    /// <summary>
    /// HTTP helper class
    /// </summary>
    public static class HTTP
    {
        /// <summary>
        /// Sends an HTTP GET request.
        /// </summary>
        public static string Get(string url)
        {
            using (WebClient client = new WebClient { Proxy = null })
                return client.DownloadString(url);
        }

        /// <summary>
        /// Sends an asynchronous HTTP GET request.
        /// </summary>
        public static async Task<byte[]> GetAsync(string url)
        {
            using (WebClient client = new WebClient { Proxy = null })
                return await client.DownloadDataTaskAsync(url);
        }

        /// <summary>
        /// Sends an asynchronous HTTP GET request.
        /// </summary>
        public static void GetAsync(string url, Action<byte[]> callback)
        {
            WebClient client = new WebClient { Proxy = null };
            client.DownloadDataCompleted += GetAsync_DownloadCompleted;
            client.DownloadDataAsync(new Uri(url), callback);
        }

        private static void GetAsync_DownloadCompleted(object sender, DownloadDataCompletedEventArgs eventArgs)
        {
            ((Action<byte[]>)eventArgs.UserState)(eventArgs.Result);
            ((WebClient)sender).Dispose();
        }

        // this should be replaced by a managed class that combines openread and openwrite
        public static void OpenRead(string address, Action<Stream> context)
        {
            using (WebClient client = new WebClient { Proxy = null })
                context(client.OpenRead(address));
        }
    }
}
