using Dnj.Colab.Samples.CustomersRestFul.Data;
using Dnj.Colab.Samples.CustomersRestFul.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dnj.Colab.Samples.CustomersRestFul.Controllers;

public class CustomersController : Controller
{
    private readonly DataContext _context;
    private readonly CustomerRepository _customerRepository;

    public CustomersController(DataContext context) => _context = context;

    // GET: Customers
    public async Task<IActionResult> Index()
    {
        IEnumerable<Customer> customers = await _customerRepository.GetCustomersAsync();

        return View(customers.ToList());

    }

    // GET: Customers/Details/5
    /// <exception cref="ArgumentNullException">id is <see langword="null"/></exception>
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Customer customer = await _customerRepository.GetCustomerByIdAsync(id!);

        // TODO: Pasar DTO en lugar de Entidad

        return View(customer);
    }

    // GET: Customers/Create
    public IActionResult Create() => View();

    // POST: Customers/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Address,Cellphone,Dateofcreation")] Customer customer)
    {
        if (!ModelState.IsValid) return View(customer);

        await _customerRepository.CreateAsync(customer);
        return RedirectToAction(nameof(Index));
    }

    // GET: Customers/Edit/5
    /// <exception cref="ArgumentNullException">id is <see langword="null"/></exception>
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Customer customer = await _customerRepository.GetCustomerByIdAsync(id);
        return View(customer);
    }

    // POST: Customers/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Address,Cellphone,Dateofcreation")] Customer customer)
    {
        if (id != customer.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid) return View(customer);
        await _customerRepository.UpdateAsync(customer);
        return RedirectToAction(nameof(Index));
    }

    // GET: Customers/Delete/5
    /// <exception cref="ArgumentNullException">id is <see langword="null"/></exception>
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Customer? customer = await _customerRepository.GetCustomerByIdAsync(id);
        return View(customer);
    }

    // POST: Customers/Delete/5
    /// <exception cref="ArgumentNullException">id is <see langword="null"/></exception>
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        Customer? customer = await _customerRepository.GetCustomerByIdAsync(id);
        await _customerRepository.DeleteAsync(customer);
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> CustomerExists(int id)
    {
        Customer customer = await _customerRepository.GetCustomerByIdAsync(id);
        return customer.Id != 0;
    }
}
