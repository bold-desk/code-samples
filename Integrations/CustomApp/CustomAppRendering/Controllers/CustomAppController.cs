using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CustomAppRendering.Repository;
using CustomAppRendering.Data;

namespace CustomAppRendering.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomAppController : ControllerBase
    {
        public object JsonConvert { get; private set; }
        /// <summary>
        /// Creating Sample without Authentication
        /// </summary>
        /// <param name="payload"></param>
        /// <returns>Response</returns>
        [Route("/NoAuthentication")]
        [HttpPost]
        public IActionResult RenderCustomApp([FromBody] object payload)
        {
                Response Response = new Response
                {
                    statusCode =200,
                    message = "<p>Hello world</p>",  
                };
                return Ok(Response);      
        }
        /// <summary>
        /// Creating Sample with Login (Username and password) Authentication
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [AuthenticatePassword]
        [Route("/AuthenticatePassword")]
        [HttpPost]
        public IActionResult RenderCustomAppByPassword([FromBody] object payload)
        {
            Response Response = new Response
            {
                statusCode = 200,
                message = "<p>Hello world</p>",   
            };
            return Ok(Response);
        }
        /// <summary>
        /// Creating Sample with Apikey Authentication
        /// </summary>
        /// <param name="payload"></param>
        /// <returns>Response</returns>
        [AuthenticateAPIKey]
        [Route("/AuthenticateAPIKey")]
        [HttpPost]
        public IActionResult RenderCustomAppByApiKey([FromBody] object payload)
        {
            Response Response = new Response
            {
                statusCode = 200,
                message = "<p>Hello world</p>",   
            };
            return Ok(Response);
        }
        /// <summary>
        /// Creating Sample with Signing key Authenticatio
        /// </summary>
        /// <param name="payload"></param>
        /// <returns>Response</returns>
        [Route("/AuthenticateSigningKey")]
        [HttpPost]
        public IActionResult RenderCustomAppBySigning([FromBody] object payload)
        {
            string Keys = string.Empty;
            string value = string.Empty;
            if (Request.Headers.TryGetValue("X-Signature", out var ExtractedHeaderKeyValues))
            {
                Keys = ExtractedHeaderKeyValues.First();
            }
            else
            {
                var Content = "Enter the signing key";
                return Ok(Content);
            }
            string Payload = payload.ToString();
            string signingKey = AppSettings.SigningKey;
            using (var hmacsha256 = new HMACSHA256(Encoding.UTF8.GetBytes(signingKey)))
            {
                var hash = hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(Payload));
                value = Convert.ToBase64String(hash);
            }
            if (!value.Equals(Keys))
            {
                var Content = "Invalid Signing key";
                return Ok(Content);
            }
            Response Response = new Response
            {
                statusCode = 200,
                message = "<p>Hello world</p>",  
            };
            return Ok(Response);
        }
    }
}
