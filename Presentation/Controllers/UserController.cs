using API.Application.Features.User.Dtos;
using API.Application.Features.User.Query.GetUsers;
using API.Application.Wrappers;
using Business.Features.User.Command.DeleteUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UserController : ControllerBase
    {
        readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [ProducesResponseType(typeof(ResponseSuccess<ResponseSuccess<List<UserDTO>>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<IActionResult> Get(GetUsersQueryRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status500InternalServerError)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var response = await _mediator.Send(new DeleteUserCommandRequest { Id = id });
            return Ok(response);
        }
    }
}
