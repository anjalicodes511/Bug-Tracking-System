using Bug_Tracking_System.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bug_Tracking_System.Filters
{
    public class GlobalExceptionFilter: FilterAttribute,IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled)
                return;

            if (filterContext.Exception is BusinessException bex)
            {
                filterContext.Result = new JsonResult
                {
                    Data = new { success = false, message = bex.Message },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
                filterContext.HttpContext.Response.StatusCode = 400; // Bad Request
                filterContext.ExceptionHandled = true;
            }
            else
            {
                // Generic error handling
                filterContext.Result = new JsonResult
                {
                    Data = new { success = false, message = "An unexpected error occurred." },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
                filterContext.HttpContext.Response.StatusCode = 500; // Internal Server Error
                filterContext.ExceptionHandled = true;
            }
        }
    }
}