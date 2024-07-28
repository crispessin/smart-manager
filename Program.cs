using Bogus;
using Microsoft.EntityFrameworkCore;
using SmartManager.Data;
using SmartManager.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure the SQLite database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Client/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Client}/{action=Index}/{id?}");

// SeedDatabase(app);

app.Run();

void SeedDatabase(IHost app)
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<ApplicationDbContext>();

        context.Database.Migrate();

        // Run the database seeder
        if (!context.Clients.Any())
        {
            var faker = new Faker<Client>("pt_BR")
                .RuleFor(c => c.Name, f => f.Company.CompanyName())
                .RuleFor(c => c.Email, f => f.Internet.Email())
                .RuleFor(c => c.Phone, f => f.Phone.PhoneNumber())
                .RuleFor(c => c.PersonType, f => f.PickRandom(new[] { "Jurídica", "Física" }))
                .RuleFor(c => c.DocumentNumber, (f, c) => c.PersonType == "Física" ? f.Random.Number(11).ToString() : f.Random.Number(14).ToString())
                .RuleFor(c => c.InscricaoEstadual, f => f.Random.AlphaNumeric(12))
                .RuleFor(c => c.Gender, (f, c) => c.PersonType == "Física" ? f.Person.Gender.ToString() : null)
                .RuleFor(c => c.BirthDate, (f, c) => c.PersonType == "Física" ? f.Date.Past(50, DateTime.Today.AddYears(-18)) : null)
                .RuleFor(c => c.Password, f => f.Internet.Password())
                .RuleFor(c => c.ConfirmPassword, (f, c) => c.Password)
                .RuleFor(c => c.InscricaoEstadualPF, f => f.Random.Bool());

            var clients = faker.Generate(30);

            context.Clients.AddRange(clients);
            context.SaveChanges();
        }
    }
}