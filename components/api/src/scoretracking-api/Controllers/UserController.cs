using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScoreTracking.App.Database;
using ScoreTracking.App.DTOs.Requests;
using ScoreTracking.App.DTOs.Requests.Users;
using ScoreTracking.App.DTOs.Responses;
using ScoreTracking.App.Helpers;
using ScoreTracking.App.Interfaces.Services;
using ScoreTracking.App.Models;
using ScoreTracking.App.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ScoreTracking.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public readonly IUriService UriService;
        public UserController(IUserService userService, IUriService uriService)
        {
            _userService = userService;
            UriService = uriService;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<ActionResult<GenericSuccessResponse<PagedList<User>>>> GetUsers([FromQuery] FilterDTO filters, CancellationToken cancellationToken)
        {
            string route = Request.Path.Value;
            var usersQuery = this._userService.GetUsers(filters, cancellationToken);
            var users =  await PagedList<User>.CreateAsync(usersQuery, UriService, filters.Page, filters.PageSize, route);

            return Ok(new GenericSuccessResponse<PagedList<User>>("Users Fetched", users));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<GenericSuccessResponse<User>>> GetUser([FromRoute] int id)
        {
           
            User user = await this._userService.GetUser(id);
            return Ok(new GenericSuccessResponse<User>("User Fetched", user));
        }

        [HttpPost]
        public async Task<ActionResult<GenericSuccessResponse<User>>> CreateUser(CreateUserRequest createUserRequest)
        {
           var user = await this._userService.CreateUser(createUserRequest);
            return Ok(new GenericSuccessResponse<User>("User Created", user));
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<GenericSuccessResponse<User>>> UpdateUser([FromRoute] int id, UpdateUserRequest updateUserRequest)
        {
            var user = await this._userService.UpdateUser(id, updateUserRequest);
            return Ok(new GenericSuccessResponse<User>("User Updated", user));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<SuccessResponse>> DeleteUser(int id) 
        {
            await this._userService.DeleteUser(id);
            // can't remove generic type in case of absence of data. (to review)
            // Probably creating a new class to handle this use case (without generic type)
            return Ok(new SuccessResponse("User Deleted")); 
        }

    }
}
