using System;
using System.Collections.Concurrent;
using System.Configuration;

namespace Infrastructure
{
    public abstract class DisposableClass : IDisposable
    {
        private static int MAX_FINALIZATION_ATTEMPTS = ConfigurationManager.AppSettings["MAX_FINALIZATION_ATTEMPTS"].IsNotNull() ? Convert.ToInt32(ConfigurationManager.AppSettings["MAX_FINALIZATION_ATTEMPTS"]) : 3;
        private static ConcurrentQueue<IDisposable> _failedFinalizations = new ConcurrentQueue<IDisposable>();
        private int _finalizationAttempts;
        private bool _disposed = false;

        /// <summary>
        /// Ideally, there should not be any need to call the destructor/finalize for child classes after inheriting from 
        /// this class.
        /// </summary>
        ~DisposableClass()
        {
            try
            {
                Dispose(false);
            }
            catch
            {
                if (_finalizationAttempts++ <= MAX_FINALIZATION_ATTEMPTS)
                {
                    GC.ReRegisterForFinalize(this);
                }
                else
                {
                    _failedFinalizations.Enqueue(this); // Resurrection
                }
            }
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
            if (_disposed)
                return;

            if (disposing)
            {
                // Free managed object(s) here.
                FreeManagedResources();
            }

            // Free unmanaged object(s) here.
            FreeUnmanagedResources();
            _disposed = true;
        }

        protected void CheckForObjectAlreadyDisposedOrNot(string className)
        {
            if(_disposed)
            {
                throw new ObjectDisposedException(className);
            }
        }

        protected virtual void FreeManagedResources() { }

        protected virtual void FreeUnmanagedResources() { }

    }
}
