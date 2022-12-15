using Dnj.Colab.Samples.CustomersRestFul.Data.Interfaces;
using Dnj.Colab.Samples.CustomersRestFul.Models;
using Microsoft.EntityFrameworkCore;

namespace Dnj.Colab.Samples.CustomersRestFul.Data;

public class EmployeeRepository : IEmployeeRepository
{

    private readonly DataContext _context;
    public EmployeeRepository(DataContext context) => _context = context;
    public void Add<T>(T entity) where T : class => _context.Add(entity);

    public void Delete<T>(T entity) where T : class => _context.Remove(entity);

    public async Task<IEnumerable<Employee>> GetEmployeesAsync()
    {
        List<Employee> employees = await _context.Employees.ToListAsync();
        return employees;
    }
    public async Task<Employee> GetEmployeesIdAsync(int id)
    {
        Employee? employee = await _context.Employees.FirstOrDefaultAsync(u => u.Id == id);
        return employee;
    }
    public async Task<Employee> GetEmployeesNameAsync(string name)
    {
        Employee? employee = await _context.Employees.FirstOrDefaultAsync(u => u.Name == name);
        return employee;
    }
    public async Task<Employee> GetEmployeesAddressAsync(string address)
    {
        Employee? employee = await _context.Employees.FirstOrDefaultAsync(u => u.Address == address);
        return employee;
    }
    public async Task<Employee> GetEmployeesCellphoneAsync(string cellphone)
    {
        Employee? employee = await _context.Employees.FirstOrDefaultAsync(u => u.Cellphone == cellphone);
        return employee;
    }
    public async Task<Employee> GetEmployeesDniAsync(string dni)
    {
        Employee? employee = await _context.Employees.FirstOrDefaultAsync(u => u.Dni == dni);
        return employee;
    }
    public async Task<bool> SaveAll() => await _context.SaveChangesAsync() > 0;
}