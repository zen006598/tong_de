using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tongDe.Data;
using tongDe.Models;
using tongDe.Models.ViewModels;

namespace tongDe.Controllers;
public class ItemController : Controller
{
    private readonly ILogger<ItemController> _logger;
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public ItemController(
        ILogger<ItemController> logger,
        ApplicationDbContext dbContext,
        IMapper mapper)
    {
        _logger = logger;
        _dbContext = dbContext;
        _mapper = mapper;
    }

    [HttpGet("Shop/{ShopId}/Item/Create")]
    public IActionResult Create(int shopId)
    {
        var newItem = new ItemCreateVM { ShopId = shopId };
        return View(newItem);
    }

    [HttpPost("Shop/{ShopId}/Item/Create")]
    public async Task<IActionResult> Create(int shopId, ItemCreateVM itemCreateVM)
    {
        var shop = await _dbContext.Shops.FindAsync(shopId);

        if (shop is null)
        {
            _logger.LogError($"Shop with ID {shopId} not found!", shopId);
            return View(itemCreateVM);
        }

        if (!ModelState.IsValid)
        {
            return View(itemCreateVM);
        }

        try
        {
            var item = _mapper.Map<Item>(itemCreateVM);

            _dbContext.Add(item);
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, $"Error occurred while creating a Item for ShopId {shopId}", shopId);
            return View(itemCreateVM);
        }

        return RedirectToAction("Details", "Shop", new { id = shopId });
    }

    [HttpGet("Item/Edit/{id}")]
    public IActionResult Edit(int id)
    {
        var item = _dbContext.Items.FirstOrDefault(item => item.Id == id);
        if (item is null) return NotFound();
        var itemEditVM = _mapper.Map<ItemEditVM>(item);
        return View(itemEditVM);
    }

    [HttpPost("Item/Edit/{id}")]
    public async Task<IActionResult> Edit(int id, ItemEditVM itemEditVM)
    {
        if (!ModelState.IsValid) return View(itemEditVM);

        var itemToUpdate = await _dbContext.Items.FirstOrDefaultAsync(item => item.Id == id);

        if (itemToUpdate is null) return NotFound();

        if (itemEditVM.ShopId != itemToUpdate.ShopId) return NotFound();

        _mapper.Map(itemEditVM, itemToUpdate);

        try
        {
            _dbContext.Update(itemToUpdate);
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, $"Error occurred while update a client");
            return View(itemEditVM);
        }

        return RedirectToAction("Details", "Shop", new { id = itemToUpdate.ShopId });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}
