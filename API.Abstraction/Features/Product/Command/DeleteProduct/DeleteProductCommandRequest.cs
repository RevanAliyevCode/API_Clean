using System;
using API.Application.Wrappers;
using MediatR;

namespace API.Application.Features.Product.Command.DeleteProduct;

public class DeleteProductCommandRequest : IRequest<Response>
{
    public int Id { get; set; }
}
