using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace HRMS_Web.Extensions
{
    // SECURITY (#114): MVC page requests carry no JWT — the token only rides on AJAX calls —
    // so pages must be guarded by the login session that Login.LoginToPortal creates.
    // An unauthenticated visitor is redirected to the login screen, never given a 401.
    // Actions marked [AllowAnonymous] (e.g. the error page) stay public.
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class SessionAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.ActionDescriptor.EndpointMetadata.OfType<IAllowAnonymous>().Any())
                return;

            if (string.IsNullOrEmpty(context.HttpContext.Session.GetString("ID")))
                context.Result = new RedirectToActionResult("Index", "Login", null);
        }
    }
}
