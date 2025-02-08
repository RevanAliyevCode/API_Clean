using API.Application.Features.Product.Command.AddProduct;
using API.Application.Features.Product.Command.UpdateProduct;
using API.Application.Features.Product.Dtos;
using API.Application.Features.Product.Query.GetAllProducts;
using API.Application.Wrappers;
using Business.Features.Product.Command.DeleteProduct;
using Business.Features.Product.Query.GetProductById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin, Seller")]
    public class ProductController : ControllerBase
    {
        readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [ProducesResponseType(typeof(ResponseSuccess<List<ProductDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status500InternalServerError)]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get(GetAllProductsQueryRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }


        [ProducesResponseType(typeof(ResponseSuccess<ProductDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status500InternalServerError)]
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(int id)
        {
            var response = await _mediator.Send(new GetProductByIdQueryRequest { Id = id });
            return Ok(response);
        }

        [ProducesResponseType(typeof(ResponseSuccess<ProductDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddProductCommandRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [ProducesResponseType(typeof(ResponseSuccess<ProductDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status500InternalServerError)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateProductCommandRequest request)
        {
            request.Id = id;
            var response = await _mediator.Send(request);
            return Ok(response);
        }


        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status500InternalServerError)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _mediator.Send(new DeleteProductCommandRequest { Id = id });
            return Ok(response);
        }
    }
}
