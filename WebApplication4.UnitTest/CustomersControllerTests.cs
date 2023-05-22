using System.Security.Cryptography.Xml;
using Microsoft.AspNetCore.Http;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using WebApplication4.Controllers;
using WebApplication4.Models;
using WebApplication4.Services;
using System.IO;

namespace WebApplication4.UnitTest
{
	public class CustomersControllerTests
	{
		private readonly DemoContext _context;
		private readonly CustomersController _controller;
        private readonly Mock<ICustomerService> _customerServiceMock;

        public CustomersControllerTests() 
		{ 
			var options = new DbContextOptionsBuilder<DemoContext>()
								.UseInMemoryDatabase(Guid.NewGuid().ToString())
								.Options;
			_context = new DemoContext(options);

            _customerServiceMock= new Mock<ICustomerService>();

			_controller = new CustomersController(_context);
		}

		[Fact]
		public async Task Index_should_return_list_of_customers()
		{
			// Arrange
			_context.Customer.Add(new Customer { Id = 1, Name = "Maali Maasikas" });
			_context.SaveChanges();

			// Act
			var result = await _controller.Index() as ViewResult;

			// Assert
			Assert.NotNull(result);
			var isCorrectView = result.ViewName == null ||
								result.ViewName == "Index";
			Assert.True(isCorrectView);
			var model = result.Model as IEnumerable<Customer>;
			Assert.NotNull(model);
			Assert.Single(model);
		}

        [Fact]
        public async Task Details_should_return_notfound_when_id_is_null()
        {
            //Arrange

            //Act
            var result = await _controller.Details(null) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }


        // Test for the Details action to return the correct customer when a valid ID is provided
        [Fact]
        public async Task Details_should_return_customer_when_id_exists()
        {
            // Arrange
            var customerId = 1;
            _context.Customer.Add(new Customer { Id = customerId, Name = "Maali Maasikas" });
            _context.SaveChanges();

            // Act
            var result = await _controller.Details(customerId) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = result.Model as Customer;
            Assert.NotNull(model);
            Assert.Equal(customerId, model.Id);
        }

        // test that the Details action returns "Not Found" result when the customer ID does not exist
        [Fact]
        public async Task Details_should_return_notfound_when_customer_not_found()
        {
            // Arrange
            var nonExistingId = 999;

            // Act
            var result = await _controller.Details(nonExistingId) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        //  test if Create action successfully saves a new customer to the database.
        [Fact]
        public async Task Create_should_save_new_customer()
        {
            // Arrange
            var customer = new Customer { Name = "John Doe", Email = "john@example.com", Address = "123 Main St", Phone = "555-1234" };

            // Act
            var result = await _controller.Create(customer) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);

            var savedCustomer = _context.Customer.FirstOrDefault(c => c.Name == "John Doe");
            Assert.NotNull(savedCustomer);
            // Additional assertions for other properties
            Assert.Equal(customer.Email, savedCustomer.Email);
            Assert.Equal(customer.Address, savedCustomer.Address);
            Assert.Equal(customer.Phone, savedCustomer.Phone);
        }

        // Test if the UploadFile action throws an ArgumentNullException when the file parameter is null.
        [Fact]
        public async Task UploadFile_should_throw_exception_when_file_is_null()
        {
            // Arrange

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _controller.UploadFile(null));
        }


        // Test if the DeleteConfirmed action correctly deletes existing customer
        [Fact]
        public async Task DeleteConfirmed_should_delete_existing_customer()
        {
            // Arrange
            var customerId = 1;
            var existingCustomer = new Customer { Id = customerId, Name = "Maali Maasikas" };
            _context.Customer.Add(existingCustomer);
            _context.SaveChanges();

            // Act
            var result = await _controller.DeleteConfirmed(customerId) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);

            var deletedCustomer = _context.Customer.FirstOrDefault(c => c.Id == customerId);
            Assert.Null(deletedCustomer);
        }


        // Test that Create action returns the same view when the model state is invalid
        [Fact]
        public async Task Create_should_return_view_when_model_state_invalid()
        {
            // Arrange
            var invalidCustomer = new Customer();
            _controller.ModelState.AddModelError("Name", "Name is required");

            // Act
            var result = await _controller.Create(invalidCustomer) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(invalidCustomer, result.Model);
            Assert.True(result.ViewName == null || result.ViewName == "Create");
        }

        // Test that the Delete action returns a "Not Found" result when the ID parameter is null.
        [Fact]
        public async Task Delete_should_return_notfound_when_id_is_null()
        {
            // Arrange

            // Act
            var result = await _controller.Delete(null) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        // Test if the DeleteConfirmed action returns a redirect to the index action after deleting a customer
        [Fact]
        public async Task DeleteConfirmed_should_return_redirect_to_index()
        {
            // Arrange
            var existingCustomer = new Customer { Id = 1, Name = "John Doe" };
            _context.Customer.Add(existingCustomer);
            _context.SaveChanges();

            // Act
            var result = await _controller.DeleteConfirmed(existingCustomer.Id) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }


    }
}