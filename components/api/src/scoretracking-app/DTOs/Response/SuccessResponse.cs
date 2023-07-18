using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreTracking.App.DTOs.Response
{
    public class SuccessResponse
    {
        public string message { get; set; }
        public Object? data { get; set; }

        public SuccessResponse(string message, Object? data) { 
            this.message = message;
            this.data = data;
        }
    }
}
