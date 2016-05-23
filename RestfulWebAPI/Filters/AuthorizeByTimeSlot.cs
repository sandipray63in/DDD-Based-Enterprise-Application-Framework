using System;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace RestfulWebAPI.Filters
{
    public class AuthorizeByTimeSlot : AuthorizeAttribute
    {
        public int SlotStartHour { get; set; }
        public int SlotEndHour { get; set; }

        protected override bool IsAuthorized(HttpActionContext context)
        {
            return DateTime.Now.Hour >= this.SlotStartHour &&
                        DateTime.Now.Hour <= this.SlotEndHour &&
                            base.IsAuthorized(context);

        }
    }
}
