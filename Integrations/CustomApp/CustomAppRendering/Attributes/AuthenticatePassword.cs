
using CustomAppRendering.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomAppRendering
{
    [AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthenticatePasswordAttribute : Attribute, IAsyncActionFilter
    {
        private const string loginHeaderName = "Authorization";
        public static string key = string.Empty;
        /// <summary>
        /// OnActionExecutionAsync method used to get the username and password from the header and validate it.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.HttpContext.Request.Headers.TryGetValue(loginHeaderName, out var data))
            {
                var authenticateHeaderValue = data;
                key = authenticateHeaderValue;
                var Decrypt = Encoding.UTF8.GetString(Convert.FromBase64String(authenticateHeaderValue)).Split(':');
                var username = Decrypt.FirstOrDefault();
                var password = Decrypt.LastOrDefault();
                if (!username.Equals(AppSettings.UserName) || !password.Equals(AppSettings.Password))
                {
                    context.Result = new ContentResult()
                    {
                        StatusCode = 401,
                        Content = "\"statusCode\":\"401\"\n\"message\":\"Invalid username and password\""
                    };
                    return;
                }
            }
            else
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "\"statusCode\":\"401\"\n\"message\":\"Enter the UserName and Password\""
                };
                return;
            }
            await next();
        }
    }
}
