using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tongDe.Data;
using tongDe.Data.Repository;
using tongDe.Models;
using tongDe.Models.ViewModels;

namespace tongDe.Controllers;
[Authorize]
public class ItemController : Controller
{
    private readonly ILogger<ItemController> _logger;
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IShopRepository _shop;
    private readonly IItemRepository _item;

    public ItemController(
        ILogger<ItemController> logger,
        ApplicationDbContext dbContext,
        IMapper mapper,
        IShopRepository shopRepository,
        IItemRepository itemRepository)
    {
        _logger = logger;
        _dbContext = dbContext;
        _mapper = mapper;
        _shop = shopRepository;
        _item = itemRepository;
    }

    [HttpGet("Shop/{ShopId}/Item/Create")]
    public IActionResult Create(int shopId)
    {
        var newItem = new ItemCreateVM { ShopId = shopId };
        return View(newItem);
    }

    [HttpPost("Shop/{ShopId}/Item/Create")]
    public async Task<IActionResult> Create(int shopId, ItemCreateVM itemToCreate)
    {
        var shop = await _shop.GetAsync(s => s.Id == shopId);

        if (shop is null)
        {
            _logger.LogError($"Shop with ID {shopId} not found!", shopId);
            return View(itemToCreate);
        }

        if (!ModelState.IsValid) return View(itemToCreate);

        var item = _mapper.Map<Item>(itemToCreate);
        try
        {
            await _item.AddAsync(item);
            await _item.SaveAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, $"Error occurred while creating a Item for ShopId {shopId}", shopId);
            return View(itemToCreate);
        }

        return RedirectToAction("Items", "Shop", new { id = shopId });
    }

    [HttpGet("Item/Edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        var item = await _item.GetItemWithAliasesAsync(id);

        if (item is null)
        {
            _logger.LogError("Item no found");
            return NotFound();
        }

        var itemEditVM = _mapper.Map<ItemEditVM>(item);
        //TODO:ItemCategories repository
        itemEditVM.ItemCategories = await _dbContext.ItemCategories
            .Where(ic => ic.ShopId == item.ShopId)
            .ToListAsync();

        return View(itemEditVM);
    }
    [HttpPost("Item/Edit/{id}")]
    public async Task<IActionResult> Edit(int id, ItemEditVM itemEditVM)
    {
        if (!ModelState.IsValid) return View(itemEditVM);

        var itemToUpdate = await _item.GetAsync(i => i.Id == id);

        if (itemToUpdate is null)
        {
            _logger.LogError("Item no found");
            return NotFound();
        }

        if (itemEditVM.ShopId != itemToUpdate.ShopId) return NotFound();

        _mapper.Map(itemEditVM, itemToUpdate);

        try
        {
            _item.Update(itemToUpdate);
            await _item.SaveAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, $"Error occurred while update a item");
            return View(itemEditVM);
        }

        return RedirectToAction("Items", "Shop", new { id = itemToUpdate.ShopId });
    }

    [HttpGet("Item/Details/{id}")]
    public async Task<IActionResult> Details(int id)
    {
        var item = await _item.GetItemWithAliasesAsync(id);
        if (item is null)
        {
            _logger.LogError($"Item Id:({id}) is not found");
            return NotFound();
        }

        return View(item);
    }
    [HttpPost("Item/Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _item.GetItemWithAliasesAsync(id);

        if (item is null) return NotFound(new { message = $"ItemAlias with ID {id} not found." });

        try
        {
            _item.Remove(item);
            await _item.SaveAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, $"Error occurred while deleting ItemAlias with ID {id}.");
            return NotFound(new { message = $"Error occurred while deleting ItemAlias with ID {id}." });
        }

        return RedirectToAction("items", "Shop", new { id = item.ShopId });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}
