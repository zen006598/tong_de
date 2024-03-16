using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tongDe.Data;
using tongDe.Data.Repository;
using tongDe.Models;
using tongDe.Models.ViewModels;

namespace tongDe.Controllers;
[Authorize, Route("[controller]")]
public class ItemController : ApplicationController
{
    private readonly IMapper _mapper;
    private readonly IShopRepository _shop;
    private readonly IItemRepository _item;
    private readonly IItemCategoryRepository _itemCategory;

    public ItemController(
        ILogger<ItemController> logger,
        IMapper mapper,
        IShopRepository shopRepository,
        IItemRepository itemRepository,
        IItemCategoryRepository itemCategoryRepository) : base(logger)
    {
        _mapper = mapper;
        _shop = shopRepository;
        _item = itemRepository;
        _itemCategory = itemCategoryRepository;
    }

    [HttpGet("Create")]
    public IActionResult Create(int shopId)
    {
        var newItem = new ItemCreateVM { ShopId = shopId };
        return View(newItem);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create(ItemCreateVM itemToCreate)
    {
        if (!ModelState.IsValid) return View(itemToCreate);

        int shopId = itemToCreate.ShopId;
        var shop = await _shop.GetAsync(s => s.Id == shopId);

        if (shop is null)
        {
            _logger.LogError($"Shop with ID {shopId} not found!", shopId);
            TempData["ErrorMessage"] = "The Shop is not found!";
            return View(itemToCreate);
        }

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

    [HttpGet("Edit")]
    public async Task<IActionResult> Edit(int id)
    {
        var item = await _item.GetItemWithAliasesAsync(id);
        if (item is null) return NotFound();

        var itemToUpdate = _mapper.Map<ItemEditVM>(item);
        var itemCategories = await _itemCategory.GetItemCategoriesAsync(item.ShopId);
        itemToUpdate.ItemCategories = itemCategories.ToList();

        return View(itemToUpdate);
    }

    [HttpPost("Edit")]
    public async Task<IActionResult> Edit(ItemEditVM itemEditVM)
    {
        if (!ModelState.IsValid) return View(itemEditVM);
        int id = itemEditVM.Id;
        var itemToUpdate = await _item.GetAsync(i => i.Id == id);

        if (itemToUpdate is null)
        {
            _logger.LogError("Item no found");
            TempData["ErrorMessage"] = "The Item is not found!";
            return View(itemEditVM);
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

    [HttpGet("Details")]
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
    [HttpPost("Delete")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _item.GetItemWithAliasesAsync(id);

        if (item is null) return NotFound(new { message = $"Item with ID {id} not found." });

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
