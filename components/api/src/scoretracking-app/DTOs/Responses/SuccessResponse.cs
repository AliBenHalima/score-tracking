using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreTracking.App.DTOs.Responses
{
    public class SuccessResponse<T>
    {
        public string Message { get; set; }
        public SuccessResponse(string message)
        {
            Message = message;
        }
    }
}
