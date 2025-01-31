using CustomAppRendering.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomAppRendering
{
    [AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthenticateAPIKeyAttribute : Attribute, IAsyncActionFilter
    {
        private const string apiKeyHeaderName = "Authorization";
        public static string key = string.Empty;
        /// <summary>
        /// OnActionExecutionAsync method used to get the Apikey value from the header and validate it
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            if (context.HttpContext.Request.Headers.TryGetValue(apiKeyHeaderName, out var apiKey))
            {
                var authenticateHeaderValue = apiKey;
                key = authenticateHeaderValue;
                var Decryptedvalue = Encoding.UTF8.GetString(Convert.FromBase64String(authenticateHeaderValue)).Split(':');
                var value = Decryptedvalue.LastOrDefault();
                if (!value.Equals(AppSettings.ApiKey))
                {
                    context.Result = new ContentResult()
                    {
                        StatusCode = 401,
                        Content ="\"statusCode\":\"401\"\n\"message\":\"Invalid APIKey\""
                    };
                    return;
                }
            }
            else {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content ="\"statusCode\":\"401\"\n\"message\":\"Enter the Apikey\""
                };
                return;
            }
            await next();
        }
    }
}
