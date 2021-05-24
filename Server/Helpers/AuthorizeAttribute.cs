using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Server.Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AdminOnlyAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.Identity != null)
            {
                var tip = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "tip");
                if (tip is {Value: "admin"}) return;
            }
            // context.Result = new UnauthorizedResult();
            context.Result = new ContentResult() { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}