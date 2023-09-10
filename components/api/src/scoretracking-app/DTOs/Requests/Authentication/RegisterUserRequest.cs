using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreTracking.App.DTOs.Requests.Authentication
{
    public record RegisterUserRequest
    {
        public string FirstName { get; init; }
        public string LastName{ get; init; }
        public string Email   { get; init; }
        public string Phone   { get; init; }
        public string Password { get; init; }
        public string ConfirmPassword { get; init; }
        public IFormFile? Image { get; set; }
        public string? ImagePath { get; set; }

    }
}
