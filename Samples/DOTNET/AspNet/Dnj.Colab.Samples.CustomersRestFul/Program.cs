using Dnj.Colab.Samples.CustomersRestFul.Data;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(x =>
{
    x.UseLazyLoadingProxies();
    x.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")
        , options => options.MigrationsAssembly(typeof(Program).Assembly.FullName));
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    using IServiceScope serviceScope = app.Services.CreateScope();
    DataContext dataContext = serviceScope.ServiceProvider.GetRequiredService<DataContext>();
    dataContext.Database.EnsureCreated();
    Console.WriteLine($"Created Db Context {nameof(DataContext)}");
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
