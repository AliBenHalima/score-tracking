using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreTracking.App.DTOs.Responses
{
    public class GenericSuccessResponse<T>
    {
        public string Message { get; set; }
        public T Data { get; set; } = default(T);
        public GenericSuccessResponse(string message, T data) {
            Message = message;
            Data = data;
        }
    }
}
