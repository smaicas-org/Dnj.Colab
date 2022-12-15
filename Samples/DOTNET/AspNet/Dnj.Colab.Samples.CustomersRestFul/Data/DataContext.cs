using Dnj.Colab.Samples.CustomersRestFul.Models;
using Microsoft.EntityFrameworkCore;

namespace Dnj.Colab.Samples.CustomersRestFul.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
}