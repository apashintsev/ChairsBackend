using ChairsBackend.Application;
using ChairsBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChairsBackend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GameController  : CommonController
    {
        public record UserDto(string Name);

        /// <summary>
        /// Получить текущую активную игру
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<Game>), 200)]
        public async Task<IActionResult> GetActiveGame()
        {
            var response = new BaseResponse<Game>(await Mediator.Send(new GetActiveGameQuery()));
            return Ok(response);
        }

        /// <summary>
        /// Вступить в игру
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<bool>), 200)]
        public async Task<IActionResult> JoinGame(UserDto dto)
        {
            var cmd = new JoinGameCommand(dto.Name);
            var response = new BaseResponse<object>(await Mediator.Send(cmd));
            return Ok(response);
        }
        /// <summary>
        /// Вступить в игру
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<bool>), 200)]
        public async Task<IActionResult> TakeChair(UserDto dto)
        {
            var cmd = new TakeChairCommand(dto.Name);
            var response = new BaseResponse<object>(await Mediator.Send(cmd));
            return Ok(response);
        }
    }
}