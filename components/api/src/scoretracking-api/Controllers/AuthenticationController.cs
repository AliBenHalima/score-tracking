﻿using AutoMapper;
using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using ScoreTracking.App.DTOs.Emails;
using ScoreTracking.App.DTOs.Requests.Authentication;
using ScoreTracking.App.DTOs.Responses;
using ScoreTracking.App.DTOs.Users;
using ScoreTracking.App.Helpers;
using ScoreTracking.App.Interfaces.Queues;
using ScoreTracking.App.Interfaces.Services;
using ScoreTracking.App.Models;
using ScoreTracking.Extensions.Email.Contratcs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ScoreTracking.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IEmailService _emailService;

        private readonly IMapper _mapper;

        public AuthenticationController(IUserService userService, IAuthenticationService authenticationService, IMapper mapper, IEmailService emailService)
        {
            _userService = userService;
            _authenticationService = authenticationService;
            _mapper = mapper;
            _emailService = emailService;
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

        [HttpPost]
        [Route("send-email")]
        public async Task<ActionResult> SendEmail(EmailDataDTO email)
        {
            string filePath = Directory.GetCurrentDirectory() + "\\Templates\\EmailTemplate.html";
            string emailTemplateText = System.IO.File.ReadAllText(filePath);
            emailTemplateText = emailTemplateText.Replace("{{UserName}}", email.ReceiverName);

            var emailMessage = new EmailMessage
            {
                Id = GlobalHelper.generateRandom(),
                ReceiverName = email.ReceiverName,
                ReceiverAddress = email.ReceiverAddress,
                Content = emailTemplateText,
                Subject = email.EmailSubject
            };
            await _emailService.SendAsync(emailMessage);
            return Ok();
        }
    
    }
}
