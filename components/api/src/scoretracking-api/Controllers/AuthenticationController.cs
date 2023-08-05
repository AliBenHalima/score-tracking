using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using ScoreTracking.App.DTOs.Requests.Authentication;
using ScoreTracking.App.DTOs.Responses;
using ScoreTracking.App.DTOs.Users;
using ScoreTracking.App.Interfaces.Services;
using ScoreTracking.App.Models;
using System;
using System.Threading.Tasks;

namespace ScoreTracking.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IMapper _mapper;

        public AuthenticationController(IUserService userService, IAuthenticationService authenticationService, IMapper mapper)
        {
            _userService = userService;
            _authenticationService = authenticationService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<GenericSuccessResponse<User>>> Register(RegisterUserRequest registerUserRequest)
        {
            User user = await _authenticationService.Register(registerUserRequest);
            RegistredUserDTO userDTO = _mapper.Map<RegistredUserDTO>(user);
            return Ok(new GenericSuccessResponse<RegistredUserDTO>("Success", userDTO));
        }

        [EnableRateLimiting("sliding-signin")]
        [HttpPost]
        [Route("signin")]
        public async Task<ActionResult<GenericSuccessResponse<string>>> Signin(SigninUserRequest signinUserRequest)
        {
            string token = await _authenticationService.Signin(signinUserRequest);
            
            return Ok(new GenericSuccessResponse<string>("Success", token));
        }
    }
}
