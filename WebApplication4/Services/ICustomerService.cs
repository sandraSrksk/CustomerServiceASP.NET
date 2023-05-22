using WebApplication4.Models;

namespace WebApplication4.Services
{
	public interface ICustomerService
	{
        Task<List<Customer>> GetAllCustomers();
        Task<Customer> GetCustomerById(int id);
        Task CreateCustomer(Customer customer);
        Task UpdateCustomer(Customer customer);
        Task DeleteCustomer(int id);
        Task UploadFile(IFormFile file);
    }
}
