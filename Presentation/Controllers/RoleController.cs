using API.Application.Features.Role.Dtos;
using API.Application.Features.Role.Query.GetRolesQuery;
using API.Application.Wrappers;
using Business.Features.Role.Command.AddRoleToUser;
using Business.Features.Role.Command.DeleteRoleFromUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class RoleController : ControllerBase
    {
        readonly IMediator _mediator;

        public RoleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [ProducesResponseType(typeof(ResponseSuccess<List<RoleDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<IActionResult> Get(GetRolesQueryRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }


        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status500InternalServerError)]
        [HttpPost("addtoUser")]
        public async Task<IActionResult> Add(string userId, string roleId)
        {
            var response = await _mediator.Send(new AddRoleToUserCommandRequest { UserId = userId, RoleId = roleId });
            return Ok(response);
        }

        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status500InternalServerError)]
        [HttpDelete("deleteFromUser")]
        public async Task<IActionResult> From(string userId, string roleId)
        {
            var response = await _mediator.Send(new DeleteRoleFromUserCommandRequest { UserId = userId, RoleId = roleId });
            return Ok(response);
        }
    }
}
