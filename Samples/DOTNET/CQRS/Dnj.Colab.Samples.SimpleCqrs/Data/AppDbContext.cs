using Dnj.Colab.Samples.SimpleCqrs.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dnj.Colab.Samples.SimpleCqrs.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
    public DbSet<TodoItemEntity> TodoItems { get; set; }
}
