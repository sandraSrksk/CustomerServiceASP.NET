using Microsoft.AspNetCore.Mvc;
using WebApplication4.Models;
using WebApplication4.Services;

public class CustomersServiceController : Controller
{

    private readonly ICustomerService _customerService;

    public CustomersServiceController(ICustomerService customerService)
    {

        _customerService = customerService;
    }

    // GET: Customers
    public async Task<IActionResult> Index()
    {
        var customers = await _customerService.GetAllCustomers();
        return View(customers);
    }

    // GET: Customers/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var customer = await _customerService.GetCustomerById(id.Value);
        if (customer == null)
        {
            return NotFound();
        }

        return View(customer);
    }

    // GET: Customers/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Customers/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Email,Address,Phone")] Customer customer)
    {
        if (ModelState.IsValid)
        {
            await _customerService.CreateCustomer(customer);
            return RedirectToAction(nameof(Index));
        }
        return View(customer);
    }

    // GET: Customers/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var customer = await _customerService.GetCustomerById(id.Value);
        if (customer == null)
        {
            return NotFound();
        }
        return View(customer);
    }

    // POST: Customers/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Address,Phone")] Customer customer)
    {
        if (id != customer.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            await _customerService.UpdateCustomer(customer);
            return RedirectToAction(nameof(Index));
        }
        return View(customer);
    }

    // GET: Customers/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var customer = await _customerService.GetCustomerById(id.Value);
        if (customer == null)
        {
            return NotFound();
        }

        return View(customer);
    }

    // POST: Customers/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _customerService.DeleteCustomer(id);
        return RedirectToAction(nameof(Index));
    }

    // GET: Customers/UploadFile
    [HttpGet]
    public IActionResult UploadFile()
    {
        return View();
    }

    // POST: Customers/UploadFile
    [HttpPost]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        // Kontrolli faili kohta
        if (file == null)
        {
            throw new ArgumentNullException(nameof(file));
        }
        // Veel mingeid kontrolle

        await _customerService.UploadFile(file);

        return RedirectToAction(nameof(Index));
    }
}


