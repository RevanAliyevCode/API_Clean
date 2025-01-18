using System;
using Business.Wrappers;
using MediatR;

namespace Business.Features.Product.Command.DeleteProduct;

public class DeleteProductCommandRequest : IRequest<Response>
{
    public int Id { get; set; }
}
