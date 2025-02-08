using System;
using API.Application.Abstraction.UnitOfWork;
using API.Application.Features.Product.Command.AddProduct;
using API.Application.Features.Product.Dtos;
using API.Application.Wrappers;
using API.Domain.Exceptions;
using API.Domain.Repositories.Product;
using AutoMapper;
using Business.Features.Product.Command.AddProduct;
using Moq;

namespace UnitTests.Handlers.Product.Command;

public class AddProductHandlerTest
{
    readonly Mock<IProductReadRepository> _productRepositoryMock;
    readonly Mock<IProductWriteRepository> _productWriteRepositoryMock;
    readonly Mock<IUnitOfWork> _unitOfWorkMock;
    readonly Mock<IMapper> _mapperMock;
    readonly AddProductCommandHandler _handler;

    public AddProductHandlerTest()
    {
        _productRepositoryMock = new Mock<IProductReadRepository>();
        _productWriteRepositoryMock = new Mock<IProductWriteRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();

        _handler = new AddProductCommandHandler(_unitOfWorkMock.Object, _productRepositoryMock.Object, _productWriteRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_WhenValidatorIsFailed_ShouldThrowValidationException()
    {
        // Arrange
        var request = new AddProductCommandRequest()
        {
            Name = "Product 1",
            Price = 100,
            Stock = 10,
            Description = "Description kcckeded edjnekdned endkende denkdnednjed kendke",
        };

        // Act
        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);


        // Assert
        var ex = await Assert.ThrowsAsync<ValidationException>(act);
        Assert.Contains("Picture is required", ex.Errors);
    }

    [Fact]
    public async Task Handle_WhenProductExists_ShouldThrowValidationException()
    {
        // Arrange
        var request = new AddProductCommandRequest()
        {
            Name = "Product 1",
            Price = 100,
            Stock = 10,
            Description = "Description kcckeded edjnekdned endkende denkdnednjed kendke",
            Picture = "ivbor"
        };

        _productRepositoryMock.Setup(x => x.GetByNamesAsync(request.Name)).ReturnsAsync(new Domain.Entities.Product());

        // Act
        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        var ex = await Assert.ThrowsAsync<ValidationException>(act);
        Assert.Contains("Product with the same name already exists", ex.Errors);
    }

    [Fact]
    public async Task Handle_WhenProductIsAdded_ShouldReturnSuccessResponse()
    {
        // Arrange
        var request = new AddProductCommandRequest()
        {
            Name = "Product 1",
            Price = 100,
            Stock = 10,
            Description = "Description kcckeded edjnekdned endkende denkdnednjed kendke",
            Picture = "ivbor"
        };

        _productRepositoryMock.Setup(x => x.GetByNamesAsync(request.Name)).ReturnsAsync((API.Domain.Entities.Product?)null);

        _mapperMock.Setup(x => x.Map<API.Domain.Entities.Product>(request)).Returns(new Domain.Entities.Product());

        var data = _mapperMock.Setup(x => x.Map<ProductDTO>(It.IsAny<API.Domain.Entities.Product>())).Returns(new ProductDTO());

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ResponseSuccess<ProductDTO>>(result);
        Assert.NotNull(result.Data);
        Assert.IsType<ProductDTO>(result.Data);
        Assert.Equal("Product added successfully", result.Message);
    }
}
