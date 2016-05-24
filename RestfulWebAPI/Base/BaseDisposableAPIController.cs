using System.Web.Http;

namespace RestfulWebAPI.Base
{
    public abstract class BaseDisposableAPIController : ApiController
    {
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                // Free managed object(s) here.
                FreeManagedResources();
            }

            // Free unmanaged object(s) here.
            FreeUnmanagedResources();
        }

        protected virtual void FreeManagedResources() { }

        protected virtual void FreeUnmanagedResources() { }
    }
}