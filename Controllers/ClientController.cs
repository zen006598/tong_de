using Microsoft.AspNetCore.Mvc;
using tongDe.Data;
using AutoMapper;
using tongDe.Models.ViewModels;
using tongDe.Models;
using Microsoft.EntityFrameworkCore;
namespace tongDe.Controllers;

public class ClientController : Controller
{
    private readonly ILogger<ClientController> _logger;
    private readonly ApplicationDbContext _dbContext;

    private readonly IMapper _mapper;

    public ClientController(
        ILogger<ClientController> logger,
        ApplicationDbContext dbContext,
        IMapper mapper)
    {
        _logger = logger;
        _dbContext = dbContext;
        _mapper = mapper;
    }

    [HttpGet("Shop/{ShopId}/Client/Create")]
    public IActionResult Create(int shopId)
    {
        var newClient = new ClientCreateVM { ShopId = shopId };
        return View(newClient);
    }

    [HttpPost("Shop/{ShopId}/Client/Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(int shopId, ClientCreateVM client)
    {
        var shop = await _dbContext.Shops.FindAsync(shopId);
        if (shop is null)
        {
            _logger.LogError($"Shop with ID {shopId} not found!", shopId);
            return View(client);
        }

        try
        {
            var clients = _mapper.Map<Client>(client);

            _dbContext.Add(clients);
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, $"Error occurred while creating a client for ShopId {shopId}", shopId);
            return View(client);
        }

        return RedirectToAction("Details", "Shop", new { id = shopId });
    }

    [HttpGet("Client/Edit/{id}")]
    public IActionResult Edit(int id)
    {
        var client = _dbContext.Clients.FirstOrDefault(c => c.Id == id);
        if (client is null) return NotFound();
        var clientViewModel = _mapper.Map<ClientEditVM>(client);
        return View(clientViewModel);
    }
    [HttpPost("Client/Edit/{id}")]
    public async Task<IActionResult> Edit(int id, ClientEditVM client)
    {
        if (!ModelState.IsValid) return View(client);
        var clientToUpdate = await _dbContext.Clients.FirstOrDefaultAsync(c => c.Id == id);

        if (clientToUpdate is null) return NotFound();

        _mapper.Map(client, clientToUpdate);

        try
        {
            _dbContext.Update(clientToUpdate);
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, $"Error occurred while update a client");
            return View(client);
        }

        return RedirectToAction("Details", "Shop", new { id = clientToUpdate.ShopId });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}
