using Microsoft.EntityFrameworkCore;
using WebApplication4.Models;

namespace WebApplication4.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly DbContextOptions<DemoContext> _dbContextOptions;

        public CustomerService(DbContextOptions<DemoContext> dbContextOptions)
        {
            _dbContextOptions = dbContextOptions;
        }

        public async Task<List<Customer>> GetAllCustomers()
        {
            using (var context = new DemoContext(_dbContextOptions))
            {
                return await context.Customer.ToListAsync();
            }
        }

        public async Task<Customer> GetCustomerById(int id)
        {
            using (var context = new DemoContext(_dbContextOptions))
            {
                return await context.Customer.FirstOrDefaultAsync(c => c.Id == id);
            }
        }

        public async Task CreateCustomer(Customer customer)
        {
            using (var context = new DemoContext(_dbContextOptions))
            {
                context.Add(customer);
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateCustomer(Customer customer)
        {
            using (var context = new DemoContext(_dbContextOptions))
            {
                context.Update(customer);
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteCustomer(int id)
        {
            using (var context = new DemoContext(_dbContextOptions))
            {
                var customer = await context.Customer.FindAsync(id);
                if (customer != null)
                {
                    context.Customer.Remove(customer);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task UploadFile(IFormFile file)
        {
            // Kontrolli faili kohta
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }
            // Veel mingeid kontrolle

            using (var fileStream = new FileStream("c:\\temp\\file.txt", FileMode.OpenOrCreate))
            using (var uploadedStream = file.OpenReadStream())
            {
                uploadedStream.Seek(0, SeekOrigin.Begin);
                uploadedStream.CopyTo(fileStream);
            }
        }
    }
}
