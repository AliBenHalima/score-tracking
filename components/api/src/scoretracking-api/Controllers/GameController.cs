using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ScoreTracking.API.Controllers
{
    [Route("api/games")]
    [ApiController]
    public class GameController : ControllerBase
    {
          public GameController() { }

        public Task<IActionResult> GetUserGames(int id)
        {
            throw new NotImplementedException("The method is not yet implemented.");
        }

    }
}
