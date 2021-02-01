using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetcoreProject2.Utils
{
    public class Response
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public Object Data { get; set; }


       

        public Response(object data, int status=200, string message="Successfully")
        {
            Status = status;
            Message = message;
            Data = data;
        }
    }
}
