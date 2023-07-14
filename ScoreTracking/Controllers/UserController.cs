using Common.WebApi.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScoreTracking.Models;
using ScoreTracking.Requests;
using ScoreTracking.Services;

namespace ScoreTracking.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public DatabaseContext dbContext { get; }
        private readonly UserService _userService;

        public UserController(DatabaseContext dbContext, UserService userService) {

            this.dbContext = dbContext;
            this._userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            List<User> users = await this._userService.GetUsers();
            return Ok(users);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetUser([FromRoute] int id)
        {
            var user = await this._userService.GetUser(id);
            return Ok(user);
        }

        [HttpPost]
        public IActionResult CreateUser(CreateUserRequest createUserRequest)
        {
            var user = this._userService.CreateUser(createUserRequest);
            return Ok();
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateUser([FromRoute] int id, UpdateUserRequest updateUserRequest)
        {
            var user = await this._userService.UpdateUser(id, updateUserRequest);
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await this._userService.DeleteUser(id);
            return StatusCode(StatusCodes.Status200OK, "User Deleted");
        }
    }
}
