using Bogus;
using Microsoft.EntityFrameworkCore;
using SmartManager.Data;
using SmartManager.Models;
using Bogus.Extensions.Brazil;
using System.Text.RegularExpressions;

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

SeedDatabase(app);

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
                .RuleFor(c => c.PersonType, f => f.PickRandom(new[] { "Jurídica", "Física" }))
                .RuleFor(c => c.DocumentNumber, (f, c) => Regex.Replace(c.PersonType == "Física" ? f.Person.Cpf() : f.Company.Cnpj(), @"[^\d]", ""))
                .RuleFor(c => c.Gender, (f, c) => c.PersonType == "Física" ? (f.Person.Gender.Equals("Male") ? "Masculino" : "Feminino") : "")
                .RuleFor(c => c.BirthDate, (f, c) => c.PersonType == "Física" ? f.Date.Past(50, DateTime.Today.AddYears(-18)) : null)
                .RuleFor(c => c.Name, (f, c) => c.PersonType == "Física" ? f.Name.FullName(f.Person.Gender) : f.Company.CompanyName())
                .RuleFor(c => c.Email, f => f.Internet.Email())
                .RuleFor(c => c.Phone, f => Regex.Replace(f.Phone.PhoneNumberFormat(), @"[^\d]", ""))
                .RuleFor(c => c.Password, f => BCrypt.Net.BCrypt.HashPassword(f.Internet.Password()))                
                .RuleFor(c => c.InscricaoEstadualPF, (f, c) => c.PersonType == "Física" ? f.Random.Bool() : false)
                .RuleFor(c => c.InscricaoEstadual, (f, c) => (c.PersonType == "Jurídica" || c.InscricaoEstadualPF ? string.Join("", f.Random.Digits(12)) : ""));

            var clients = faker.Generate(30);

            context.Clients.AddRange(clients);
            context.SaveChanges();
        }
    }
}