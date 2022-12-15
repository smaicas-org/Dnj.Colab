using Dnj.Colab.Samples.CustomersRestFul.Data.Interfaces;
using Dnj.Colab.Samples.CustomersRestFul.Models;
using Microsoft.EntityFrameworkCore;

namespace Dnj.Colab.Samples.CustomersRestFul.Data;

public class CustomerRepository : ICustomerRepository
{
    private readonly DataContext _context;
    public CustomerRepository(DataContext context) => _context = context;
    public async Task CreateAsync<T>(T entity) where T : class
    {
        _context.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync<T>(T entity) where T : class
    {
        _context.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<Customer> GetCustomerByAddressAsync(string address)
    {
        Customer? customer = await _context.Customers.FirstOrDefaultAsync(u => u.Address == address);
        return customer;
    }

    public async Task<IEnumerable<Customer>> GetCustomersAsync()
    {
        List<Customer> customers = await _context.Customers.ToListAsync();
        return customers;
    }

    public async Task<Customer> GetCustomerByCellphoneAsync(string cellphone)
    {
        Customer? customer = await _context.Customers.FirstOrDefaultAsync(u => u.Cellphone == cellphone);
        return customer;
    }

    /// <exception cref="ArgumentNullException"><paramref name="id"/> is <see langword="null"/></exception>
    public async Task<Customer> GetCustomerByIdAsync(int? id)
    {
        if (id == null) throw new ArgumentNullException(nameof(id));
        Customer? customer = await _context.Customers.FirstOrDefaultAsync(u => u.Id == id);
        return customer;
    }

    public async Task<Customer> GetCustomerByNameAsync(string name)
    {
        Customer? customer = await _context.Customers.FirstOrDefaultAsync(u => u.Name == name);
        return customer;
    }

    public async Task<bool> SaveAll() => await _context.SaveChangesAsync() > 0;

    public async Task UpdateAsync(Customer customer)
    {
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync();
    }
}
