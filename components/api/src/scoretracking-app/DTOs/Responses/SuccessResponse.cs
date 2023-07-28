using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ScoreTracking.App.DTOs.Responses
{
    public class SuccessResponse
    {
        public string message { get; set; }
        public object? data { get; set; }

        public SuccessResponse(string message, object? data) { 
            this.message = message;
            this.data = data;
        }
    }
}
