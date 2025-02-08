using System;
using API.Application.Abstraction.UnitOfWork;
using API.Application.Features.Product.Command.DeleteProduct;
using API.Application.Wrappers;
using API.Domain.Exceptions;
using API.Domain.Repositories.Product;
using Business.Features.Product.Command.DeleteProduct;
using Moq;

namespace UnitTests.Handlers.Product.Command;

public class DeleteProductHandlerTest
{
    readonly Mock<IProductReadRepository> _productReadRepositoryMock;
    readonly Mock<IProductWriteRepository> _productWriteRepositoryMock;
    readonly Mock<IUnitOfWork> _unitOfWorkMock;
    readonly DeleteProductCommandHandler _handler;

    public DeleteProductHandlerTest()
    {
        _productReadRepositoryMock = new Mock<IProductReadRepository>();
        _productWriteRepositoryMock = new Mock<IProductWriteRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _handler = new DeleteProductCommandHandler(_unitOfWorkMock.Object, _productWriteRepositoryMock.Object, _productReadRepositoryMock.Object);
    }

    [Fact]
    public async Task Handler_WhenProductNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var request = new DeleteProductCommandRequest()
        {
            Id = default
        };
        // Act
        _productReadRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((API.Domain.Entities.Product)null);

        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        var ex = await Assert.ThrowsAsync<NotFoundException>(act);
        Assert.Contains("Product not found", ex.Errors);
    }

    [Fact]
    public async Task Handler_WhenProductFound_ShouldDeleteProduct()
    {
        // Arrange
        var request = new DeleteProductCommandRequest()
        {
            Id = 1
        };
    
        _productReadRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Domain.Entities.Product());

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);


        // Assert
        Assert.IsType<Response>(result);
        Assert.True(result.Succeeded);
        _productWriteRepositoryMock.Verify(x => x.Delete(It.IsAny<API.Domain.Entities.Product>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.CommitChangesAsync(), Times.Once);
    }
}
