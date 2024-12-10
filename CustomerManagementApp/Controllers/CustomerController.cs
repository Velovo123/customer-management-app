using CustomerManagementApp.Models;
using CustomerManagementApp.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace CustomerManagementApp.Controllers
{
    [Route("customer")]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // GET: customer
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var customers = await _customerService.GetAllAsync();
            if (customers == null || !customers.Any()) return View("NoCustomersFound");

            return View(customers);
        }

        // GET: customer/create
        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: customer/create
        [HttpPost("create")]
        public async Task<IActionResult> Create(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return View(customer);
            }

            await _customerService.CreateAsync(customer);
            return RedirectToAction("Index");
        }

        // GET: customer/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var customer = await _customerService.GetByIdAsync(id);
            if (customer == null) return NotFound();

            return View(customer);
        }

        // GET: customer/edit/{id}
        [HttpGet("edit/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var customer = await _customerService.GetByIdAsync(id);
            if (customer == null) return NotFound();

            return View(customer);
        }

        // POST: customer/edit/{id}
        [HttpPost("edit/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id, Customer customer)
        {
            if (id != customer.Id) return BadRequest("Mismatched customer ID.");
            if (!ModelState.IsValid)
            {
                return View(customer);
            }

            await _customerService.UpdateAsync(customer);
            return RedirectToAction("Index");
        }

        // GET: customer/delete/{id}
        [HttpGet("delete/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var customer = await _customerService.GetByIdAsync(id);
            if (customer == null) return NotFound();

            return View(customer);
        }

        // POST: customer/delete/{id}
        [HttpPost("delete/{id:guid}")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _customerService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
