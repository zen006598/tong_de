using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tongDe.Data.Repository;
using tongDe.Models;
using tongDe.Models.ViewModels;

namespace tongDe.Controllers;
[Authorize]
public class ItemCategoryController : Controller
{
    private readonly ILogger<ItemCategoryController> _logger;
    private readonly IMapper _mapper;
    private readonly IItemCategoryRepository _itemCategory;
    private readonly IShopRepository _shop;

    public ItemCategoryController(
        ILogger<ItemCategoryController> logger,
        IMapper mapper,
        IItemCategoryRepository itemCategoryRepository,
        IShopRepository shopRepository)
    {
        _logger = logger;
        _mapper = mapper;
        _itemCategory = itemCategoryRepository;
        _shop = shopRepository;
    }

    [HttpGet("Shop/{ShopId}/ItemCategory/Create")]
    public IActionResult Create(int shopId)
    {
        var newItemCategory = new ItemCategoryCreateVM { ShopId = shopId };
        return View(newItemCategory);
    }

    [HttpPost("Shop/{ShopId}/itemCategory/Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(int shopId, ItemCategoryCreateVM itemCategoryCreateVM)
    {
        var shop = await _shop.GetAsync(s => s.Id == shopId);

        if (shop is null)
        {
            _logger.LogError($"Shop with ID {shopId} not found!", shopId);
            return View(itemCategoryCreateVM);
        }

        if (!ModelState.IsValid) return View(itemCategoryCreateVM);

        var itemCategory = _mapper.Map<ItemCategory>(itemCategoryCreateVM);

        try
        {
            await _itemCategory.AddAsync(itemCategory);
            await _itemCategory.SaveAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, $"Error occurred while creating a Item for ShopId {shopId}", shopId);
            return View(itemCategoryCreateVM);
        }

        return RedirectToAction("itemCategories", "Shop", new { id = shopId });
    }

    [HttpGet("ItemCategory/Edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        var itemCategory = await _itemCategory.GetAsync(ic => ic.Id == id);
        if (itemCategory is null) return NotFound();
        var itemCategoryToUpdate = _mapper.Map<ItemCategoryEditVM>(itemCategory);
        return View(itemCategoryToUpdate);
    }

    [HttpPost("ItemCategory/Edit/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ItemCategoryEditVM itemCategoryEditVM)
    {
        if (!ModelState.IsValid) return View(itemCategoryEditVM);

        var ItemCategoryToUpdate = await _itemCategory.GetAsync(ic => ic.Id == id);

        if (ItemCategoryToUpdate is null) return NotFound();

        _mapper.Map(itemCategoryEditVM, ItemCategoryToUpdate);

        try
        {
            _itemCategory.Update(ItemCategoryToUpdate);
            await _itemCategory.SaveAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, $"Error occurred while update a Item Category");
            return View(itemCategoryEditVM);
        }
        return RedirectToAction("ItemCategories", "Shop", new { id = ItemCategoryToUpdate.ShopId });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}
