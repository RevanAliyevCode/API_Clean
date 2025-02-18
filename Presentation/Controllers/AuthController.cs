using API.Application.Features.User.Command.LoginUser;
using API.Application.Features.User.Command.RegisterUser;
using API.Application.Features.User.Dtos;
using API.Application.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status500InternalServerError)]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommandRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }


        [ProducesResponseType(typeof(ResponseSuccess<TokenDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status500InternalServerError)]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommandRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

    }
}
