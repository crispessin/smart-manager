using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartManager.Data;
using SmartManager.Models;
using X.PagedList.Extensions;

namespace SmartManager.Controllers
{
    public class ClientController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? page, string search)
        {
            int pageSize = 20;
            int pageNumber = (page ?? 1);

            var clientsQuery = _context.Clients.AsNoTracking();

            if (!string.IsNullOrEmpty(search))
            {
                clientsQuery = clientsQuery.Where(c => c.Name.ToUpper().Contains(search.ToUpper()));
            }

            ViewData["Search"] = search;

            return View(clientsQuery.OrderBy(c => c.Id).ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public IActionResult ToggleBlock(int id, bool isBlocked)
        {
            var client = _context.Clients.Find(id);
            if (client == null)
            {
                return NotFound();
            }

            client.IsBlocked = isBlocked;
            _context.SaveChanges();

            return Ok();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Phone,PersonType,DocumentNumber,InscricaoEstadual,InscricaoEstadualPF,InscricaoEstadualIsento,IsBlocked,Gender,BirthDate,Password,ConfirmPassword,IsBlocked")] Client client)
        {
            if (ModelState.IsValid)
            {
                if (_context.Clients.Any(c => c.Email == client.Email))
                {
                    ModelState.AddModelError("Email", "O e-mail já está vinculado a outro Comprador.");
                }
                if (_context.Clients.Any(c => c.DocumentNumber == client.DocumentNumber))
                {
                    ModelState.AddModelError("DocumentNumber", "O CPF/CNPJ já está vinculado a outro Comprador.");
                }
                if (!string.IsNullOrEmpty(client.InscricaoEstadual) && !client.InscricaoEstadualIsento && _context.Clients.Any(c => c.InscricaoEstadual == client.InscricaoEstadual))
                {
                    ModelState.AddModelError("InscricaoEstadual", "A Inscrição Estadual já está vinculada a outro Comprador.");
                }
                
                if (string.IsNullOrWhiteSpace(client.Password) || client.Password != client.ConfirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "As senhas devem ser preenchidas e coincidir.");
                }

                if (ModelState.IsValid)
                {
                    client.Password = BCrypt.Net.BCrypt.HashPassword(client.Password);

                    _context.Add(client);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(client);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Phone,PersonType,DocumentNumber,InscricaoEstadual,InscricaoEstadualPF,InscricaoEstadualIsento,IsBlocked,Gender,BirthDate,Password,ConfirmPassword,IsBlocked")] Client client, bool PasswordChanged)
        {
            if (id != client.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Verificação de e-mail, CPF/CNPJ e Inscrição Estadual duplicados
                if (_context.Clients.Any(c => c.Email == client.Email && c.Id != client.Id))
                {
                    ModelState.AddModelError("Email", "O e-mail já está vinculado a outro Comprador.");
                }
                if (_context.Clients.Any(c => c.DocumentNumber == client.DocumentNumber && c.Id != client.Id))
                {
                    ModelState.AddModelError("DocumentNumber", "O CPF/CNPJ já está vinculado a outro Comprador.");
                }
                if (!string.IsNullOrEmpty(client.InscricaoEstadual) && !client.InscricaoEstadualIsento && _context.Clients.Any(c => c.InscricaoEstadual == client.InscricaoEstadual && c.Id != client.Id))
                {
                    ModelState.AddModelError("InscricaoEstadual", "A Inscrição Estadual já está vinculada a outro Comprador.");
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        var existingClient = await _context.Clients.FindAsync(id);
                        if (existingClient == null)
                        {
                            return NotFound();
                        }

                        existingClient.Name = client.Name;
                        existingClient.Email = client.Email;
                        existingClient.Phone = client.Phone;
                        existingClient.PersonType = client.PersonType;
                        existingClient.DocumentNumber = client.DocumentNumber;
                        existingClient.InscricaoEstadual = client.InscricaoEstadual;
                        existingClient.IsBlocked = client.IsBlocked;
                        existingClient.Gender = client.Gender;
                        existingClient.BirthDate = client.BirthDate;
                        existingClient.InscricaoEstadualPF = client.InscricaoEstadualPF;
                        existingClient.IsBlocked = client.IsBlocked;
                        existingClient.InscricaoEstadualIsento = client.InscricaoEstadualIsento;

                        if (PasswordChanged && !string.IsNullOrWhiteSpace(client.Password) && client.Password == client.ConfirmPassword)
                        {                            
                            existingClient.Password = BCrypt.Net.BCrypt.HashPassword(client.Password);
                        }                        

                        _context.Clients.Update(existingClient);

                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ClientExists(client.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(client);
        }

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }

    }
}
