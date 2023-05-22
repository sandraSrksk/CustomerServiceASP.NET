using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using WebApplication4.Controllers;
using WebApplication4.Models;
using WebApplication4.Services;

namespace WebApplication4.UnitTest
{
	public class CustomersServiceControllerTests
	{
		private readonly CustomersServiceController _controller;
		private readonly Mock<ICustomerService> _customerServiceMock;

		public CustomersServiceControllerTests()
		{
            var options = new DbContextOptionsBuilder<DemoContext>()
                            .UseInMemoryDatabase(Guid.NewGuid().ToString())
                            .Options;
            var context = new DemoContext(options);

            _customerServiceMock = new Mock<ICustomerService>();

            _controller = new CustomersServiceController(_customerServiceMock.Object);
        }

        [Fact]
		public async Task Details_should_return_notfound_when_customer_is_not_found()
		{
			// Arrange
			var customer = (Customer)null;
			_customerServiceMock.Setup(x => x.GetCustomerById(1))
								.ReturnsAsync(customer);

			// Act
			var result = await _controller.Details(1) as NotFoundResult;

			// Arrange
			Assert.NotNull(result);
		}
	}
}
