using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApplication1.Filters
{
    public class AdminAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session["UserID"] == null)
            {
                return false;
            }

            string role = Convert.ToString(httpContext.Session["Role"]);
            return role == "Admin";
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Session["UserID"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new
                    {
                        controller = "Account",
                        action = "Login"
                    }));
                return;
            }

            filterContext.Result = new HttpStatusCodeResult(
                403,
                "Bạn không có quyền truy cập trang quản trị.");
        }
    }
}
