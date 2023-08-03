using ScoreTracking.App.DTOs.Requests.Authentication;
using ScoreTracking.App.Models;
using System.Threading.Tasks;

namespace ScoreTracking.App.Interfaces.Services
{
    public interface IAuthenticationService
    {
        Task<User> Register(RegisterUserRequest registerUserRequest);
        Task<string> Signin(SigninUserRequest signinUserRequest);
    }
}