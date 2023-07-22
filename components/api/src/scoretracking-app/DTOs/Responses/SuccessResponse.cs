using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ScoreTracking.App.DTOs.Responses
{
    public class SuccessResponse<T>
    {
        public string Message { get; set; }
        public T? Data { get; set; } = default ;

        public SuccessResponse(string message, T? data) {
            Message = message;
            Data = data;
        }
        public SuccessResponse(string message)
        {
            Message = message;
        }
    }
}
