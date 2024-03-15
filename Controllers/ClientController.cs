using Microsoft.AspNetCore.Mvc;
using tongDe.Data;
using AutoMapper;
using tongDe.Models.ViewModels;
using tongDe.Models;
using Microsoft.EntityFrameworkCore;
using tongDe.Data.Repository;
namespace tongDe.Controllers;

public class ClientController : Controller
{
    private readonly ILogger<ClientController> _logger;
    private readonly IShopRepository _shop;
    private readonly IClientRepository _client;
    private readonly IMapper _mapper;

    public ClientController(
        ILogger<ClientController> logger,
        IMapper mapper,
        IClientRepository clientRepository,
        IShopRepository shopRepository)
    {
        _logger = logger;
        _mapper = mapper;
        _client = clientRepository;
        _shop = shopRepository;
    }

    [HttpGet("Shop/{ShopId}/Client/Create")]
    public IActionResult Create(int shopId)
    {
        var newClient = new ClientCreateVM { ShopId = shopId };
        return View(newClient);
    }

    [HttpPost("Shop/{ShopId}/Client/Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(int shopId, ClientCreateVM clientToCreate)
    {
        var shop = await _shop.GetAsync(s => s.Id == shopId);
        if (shop is null)
        {
            _logger.LogError($"Shop with ID {shopId} not found!", shopId);
            return View(clientToCreate);
        }

        try
        {
            var client = _mapper.Map<Client>(clientToCreate);
            await _client.AddAsync(client);
            await _client.SaveAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, $"Error occurred while creating a client for ShopId {shopId}", shopId);
            return View(clientToCreate);
        }

        return RedirectToAction("Details", "Shop", new { id = shopId });
    }

    [HttpGet("Client/Edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        var client = await _client.GetAsync(c => c.Id == id);

        if (client is null) return NotFound();

        var clientViewModel = _mapper.Map<ClientEditVM>(client);

        return View(clientViewModel);
    }
    [HttpPost("Client/Edit/{id}")]
    public async Task<IActionResult> Edit(int id, ClientEditVM client)
    {
        if (!ModelState.IsValid) return View(client);
        var clientToUpdate = await _client.GetAsync(c => c.Id == id);

        if (clientToUpdate is null) return NotFound();

        _mapper.Map(client, clientToUpdate);

        try
        {
            _client.Update(clientToUpdate);
            await _client.SaveAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, $"Error occurred while update a client");
            return View(client);
        }

        return RedirectToAction("Details", "Shop", new { id = clientToUpdate.ShopId });
    }
    [HttpPost("Client/Cancel")]
    public async Task<IActionResult> Cancel(int id)
    {
        var clientToCancel = await _client.GetAsync(c => c.Id == id);
        if (clientToCancel is null) return NotFound();
        clientToCancel.Cancel = true;

        try
        {
            await _client.SaveAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, $"Error occurred while cancel a client");
            return View(clientToCancel);
        }

        return RedirectToAction("Details", "Shop", new { id = clientToCancel.ShopId });
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}
