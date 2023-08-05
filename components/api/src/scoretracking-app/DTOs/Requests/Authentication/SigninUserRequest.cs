using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreTracking.App.DTOs.Requests.Authentication
{
    public record SigninUserRequest
    {
        public string Email   { get; init; }
        public string Password { get; init; }
    }
}
