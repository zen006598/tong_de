using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tongDe.Data.Repository;
using tongDe.Models;
using tongDe.Models.ViewModels;

namespace tongDe.Controllers;
[Authorize, Route("[controller]")]
public class ItemCategoryController : ApplicationController
{
    private readonly IMapper _mapper;
    private readonly IItemCategoryRepository _itemCategory;
    private readonly IShopRepository _shop;

    public ItemCategoryController(
        ILogger<ItemCategoryController> logger,
        IMapper mapper,
        IItemCategoryRepository itemCategoryRepository,
        IShopRepository shopRepository) : base(logger)
    {
        _mapper = mapper;
        _itemCategory = itemCategoryRepository;
        _shop = shopRepository;
    }

    [HttpGet("Create")]
    public IActionResult Create(int shopId)
    {
        var newItemCategory = new ItemCategoryCreateVM { ShopId = shopId };
        return View(newItemCategory);
    }

    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ItemCategoryCreateVM itemCategoryCreateVM)
    {
        if (!ModelState.IsValid) return View(itemCategoryCreateVM);
        int shopId = itemCategoryCreateVM.ShopId;
        var shop = await _shop.GetAsync(s => s.Id == shopId);

        if (shop is null)
        {
            TempData["ErrorMessage"] = "The shop is not found!";
            _logger.LogError($"Shop with ID {shopId} not found!", shopId);
            return View(itemCategoryCreateVM);
        }

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

    [HttpGet("Edit")]
    public async Task<IActionResult> Edit(int id)
    {
        var itemCategory = await _itemCategory.GetAsync(ic => ic.Id == id);
        if (itemCategory is null) return NotFound();
        var itemCategoryToUpdate = _mapper.Map<ItemCategoryEditVM>(itemCategory);
        return View(itemCategoryToUpdate);
    }

    [HttpPost("Edit"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ItemCategoryEditVM itemCategoryEditVM)
    {
        if (!ModelState.IsValid) return View(itemCategoryEditVM);
        int id = itemCategoryEditVM.Id;
        var ItemCategoryToUpdate = await _itemCategory.GetAsync(ic => ic.Id == id);

        if (ItemCategoryToUpdate is null)
        {
            TempData["ErrorMessage"] = "The item category is not found!";
            _logger.LogError($"item category with ID {id} not found!", id);
            return View(itemCategoryEditVM);
        }

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
    [HttpPost("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        //get all item under this category
        ItemCategory itemCategory = await _itemCategory.GetItemCategoryWithItemsAsync(id);
        if (itemCategory is null) return NotFound();
        //remove the fk from item
        try
        {
            if (itemCategory.Items is not null)
            {
                foreach (var item in itemCategory.Items)
                {
                    item.ItemCategoryId = null;
                }
                _itemCategory.Update(itemCategory);
                await _itemCategory.SaveAsync();
            }
            //remove this category
            _itemCategory.Remove(itemCategory);
            await _itemCategory.SaveAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, $"Error occurred while Remove a Item Category");
            return NotFound();
        }

        return RedirectToAction("itemCategories", "Shop", new { id = itemCategory.ShopId });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}
