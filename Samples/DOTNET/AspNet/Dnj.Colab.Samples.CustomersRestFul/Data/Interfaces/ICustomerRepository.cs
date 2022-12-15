using Dnj.Colab.Samples.CustomersRestFul.Models;

namespace Dnj.Colab.Samples.CustomersRestFul.Data.Interfaces;

public interface ICustomerRepository
{
    Task CreateAsync<T>(T entity) where T : class;
    Task UpdateAsync(Customer customer);
    Task DeleteAsync<T>(T entity) where T : class;
    Task<bool> SaveAll();
    Task<IEnumerable<Customer>> GetCustomersAsync();
    Task<Customer> GetCustomerByIdAsync(int? id);
    Task<Customer> GetCustomerByNameAsync(string name);
    Task<Customer> GetCustomerByAddressAsync(string address);
    Task<Customer> GetCustomerByCellphoneAsync(string cellphone);
}