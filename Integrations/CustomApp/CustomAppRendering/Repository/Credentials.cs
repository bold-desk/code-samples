using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomAppRendering.Repository
{
   /// <summary>
   /// default properties for returning response
   /// </summary>
    public class Response
    {
        public int statusCode { get; set; }
        public string message { get; set; }
    }
}
