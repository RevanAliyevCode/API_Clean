using System;
using API.Application.Abstraction.UnitOfWork;
using API.Application.Features.Product.Command.UpdateProduct;
using API.Application.Features.Product.Dtos;
using API.Application.Wrappers;
using API.Domain.Exceptions;
using API.Domain.Repositories.Product;
using AutoMapper;
using Moq;

namespace UnitTests.Handlers.Product.Command;

public class UpdateProductHandlerTest
{
    readonly Mock<IProductReadRepository> _productRepositoryMock;
    readonly Mock<IProductWriteRepository> _productWriteRepositoryMock;
    readonly Mock<IUnitOfWork> _unitOfWorkMock;
    readonly Mock<IMapper> _mapperMock;

    readonly UpdateProductCommandHandler _handler;

    public UpdateProductHandlerTest()
    {
        _productRepositoryMock = new Mock<IProductReadRepository>();
        _productWriteRepositoryMock = new Mock<IProductWriteRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();

        _handler = new UpdateProductCommandHandler(_unitOfWorkMock.Object, _productWriteRepositoryMock.Object, _productRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_WhenProductNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var request = new UpdateProductCommandRequest()
        {
            Id = default,
            Name = "Product 1",
            Price = 100,
            Stock = 10,
            Description = "Description kcckeded edjnekdned endkende denkdnednjed kendke",
        };

        _productRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((API.Domain.Entities.Product)null);

    
        // Act
        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);


        // Assert
        var ex = await Assert.ThrowsAsync<NotFoundException>(act);
        Assert.Contains("Product not found", ex.Errors);
    }

    [Fact]
    public async Task Handle_WhenProductFound_ShouldUpdateProduct()
    {
        // Arrange
        var request = new UpdateProductCommandRequest()
        {
            Id = 1,
            Name = "Product 1",
            Price = 100,
            Stock = 10,
            Description = "Description kcckeded edjnekdned endkende denkdnednjed kendke",
            Picture = "ivbor"
        };

        _productRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Domain.Entities.Product());

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.IsType<ResponseSuccess<ProductDTO>>(result);
        Assert.True(result.Succeeded);
        _productWriteRepositoryMock.Verify(x => x.Update(It.IsAny<API.Domain.Entities.Product>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.CommitChangesAsync(), Times.Once);
    }
}
