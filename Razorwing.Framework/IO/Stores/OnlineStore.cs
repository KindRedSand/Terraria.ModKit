using System;
using System.IO;
using System.Threading.Tasks;
using WebRequest = Razorwing.Framework.IO.Network.WebRequest;

namespace Razorwing.Framework.IO.Stores
{
    public class OnlineStore : IResourceStore<byte[]>
    {
        public async Task<byte[]> GetAsync(string url)
        {
            //if (!url.StartsWith(@"https://", StringComparison.Ordinal))
            //    return null;

            try
            {
                WebRequest req = new WebRequest($@"{url}");
                await req.PerformAsync();
                return req.ResponseData;
            }
            catch
            {
                return null;
            }
        }

        public byte[] Get(string url)
        {
            //if (!url.StartsWith(@"https://", StringComparison.Ordinal))
            //    return null;

            try
            {
                WebRequest req = new WebRequest($@"{url}");
                req.Perform();
                return req.ResponseData;
            }
            catch
            {
                return null;
            }
        }

        public Stream GetStream(string url)
        {
            var ret = Get(url);

            if (ret == null) return null;

            return new MemoryStream(ret);
        }

        #region IDisposable Support

        private bool isDisposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                isDisposed = true;
            }
        }

        ~OnlineStore()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
