using BookManagementSystem.Controllers;
using BookManagementSystem.Models;
using BookManagementSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace BookManagementSystem.Tests
{
    public class BooksControllerTests
    {
        private readonly Mock<IBookService> _mockBookService;
        private readonly BooksController _controller;

        public BooksControllerTests()
        {
            _mockBookService = new Mock<IBookService>();
            _controller = new BooksController(_mockBookService.Object);
        }

        [Fact]
        public async Task Index_ReturnsViewWithBooks()
        {
            // Arrange
            var books = new List<Book> { new Book { Id = 1, Title = "Test Book" } };
            _mockBookService.Setup(service => service.GetAllBooksAsync()).ReturnsAsync(books);

            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Book>>(viewResult.Model);
            Assert.Single(model);
        }

        [Fact]
        public async Task Details_ReturnsNotFound_WhenBookDoesNotExist()
        {
            // Arrange
            _mockBookService.Setup(service => service.GetBookByIdAsync(99)).ReturnsAsync((Book)null);

            // Act
            var result = await _controller.Details(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ValidModel_AddsBookAndRedirectsToIndex()
        {
            // Arrange
            var book = new Book { Title = "New Book" };
            var imageFile = new Mock<IFormFile>();
            _mockBookService.Setup(service => service.AddBookAsync(It.IsAny<Book>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(book, imageFile.Object);

            // Assert
            _mockBookService.Verify(service => service.AddBookAsync(It.IsAny<Book>()), Times.Once);
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(BooksController.Index), redirectResult.ActionName);
        }

        [Fact]
        public async Task Edit_ValidModel_UpdatesBookAndRedirectsToIndex()
        {
            // Arrange
            var book = new Book { Id = 1, Title = "Updated Book" };
            var imageFile = new Mock<IFormFile>(); // Mock the image file
            _mockBookService.Setup(service => service.UpdateBookAsync(It.IsAny<Book>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Edit(book.Id, book, imageFile.Object);

            // Assert
            _mockBookService.Verify(service => service.UpdateBookAsync(It.IsAny<Book>()), Times.Once);
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(BooksController.Index), redirectResult.ActionName);
        }

        [Fact]
        public async Task DeleteConfirmed_RemovesBookAndRedirectsToIndex()
        {
            // Arrange
            var book = new Book { Id = 1, Title = "Book to delete", ImageUrl = "images/sample.jpg" };
            _mockBookService.Setup(service => service.GetBookByIdAsync(book.Id)).ReturnsAsync(book);
            _mockBookService.Setup(service => service.DeleteBookAsync(book.Id)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteConfirmed(book.Id);

            // Assert
            _mockBookService.Verify(service => service.DeleteBookAsync(book.Id), Times.Once);

            // Ensure the image file was deleted
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", book.ImageUrl.TrimStart('/'));
            Assert.False(System.IO.File.Exists(imagePath), "Image file should be deleted.");

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(BooksController.Index), redirectResult.ActionName);
        }
    }
}
