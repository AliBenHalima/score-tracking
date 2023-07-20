using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScoreTracking.App.Database;
using ScoreTracking.App.DTOs.Requests;
using ScoreTracking.App.DTOs.Responses;
using ScoreTracking.App.Helpers;
using ScoreTracking.App.Interfaces.Services;
using ScoreTracking.App.Models;
using ScoreTracking.App.Services;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace ScoreTracking.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            IEnumerable<User> users = await this._userService.GetUsers();
            return Ok(new SuccessResponse<IEnumerable<User>>("Users Fetched", users));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetUser([FromRoute] int id)
        {
            User user = await this._userService.GetUser(id);
            return Ok(new SuccessResponse<User>("User Fetched", user));
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserRequest createUserRequest)
        {
           var user = await this._userService.CreateUser(createUserRequest);
            return Ok(new SuccessResponse<User>("User Created", user));
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateUser([FromRoute] int id, UpdateUserRequest updateUserRequest)
        {
            var user = await this._userService.UpdateUser(id, updateUserRequest);
            return Ok(new SuccessResponse<User>("User Updated", user));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id) 
        {
            await this._userService.DeleteUser(id);
            return Ok(new SuccessResponse<NullabilityInfo>("User Deleted", null));
        }
    }
}
