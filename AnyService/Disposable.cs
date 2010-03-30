using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnyService
{
    public class Disposable : IDisposable
    {
        ~Disposable()
        {
            this.Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.Disposed)
            {
                if (disposing) this.Release();
                this.Disposed = true;
            }
        }

        protected virtual void Release() { }

        protected bool Disposed
        {
            get;
            private set;
        }

        #region IDisposable Members

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
