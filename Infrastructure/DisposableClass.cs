using System;

namespace Infrastructure
{
    public abstract class DisposableClass : IDisposable
    {
        // Flag: Has Dispose already been called?
        bool disposed = false;

        /// <summary>
        /// Ideally, there should not be any need to call the destructor/finalize for child classes after inheriting from 
        /// this class.
        /// </summary>
        ~DisposableClass()
        {
            Dispose(false);
        }

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Private implementation of Dispose pattern.
        private void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                // Free managed object(s) here.
                FreeManagedResources();
            }

            // Free unmanaged object(s) here.
            FreeUnmanagedResources();
            disposed = true;
        }

        protected virtual void FreeManagedResources() { }

        protected virtual void FreeUnmanagedResources() { }

    }
}
