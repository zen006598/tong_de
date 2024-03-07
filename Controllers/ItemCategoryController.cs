using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tongDe.Data;
using tongDe.Models;
using tongDe.Models.ViewModels;

namespace tongDe.Controllers;
[Authorize]
public class ItemCategoryController : Controller
{
    private readonly ILogger<ItemCategoryController> _logger;
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public ItemCategoryController(
        ILogger<ItemCategoryController> logger,
        ApplicationDbContext dbContext,
        IMapper mapper)
    {
        _logger = logger;
        _dbContext = dbContext;
        _mapper = mapper;
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
        var shop = await _dbContext.Shops.FindAsync(shopId);

        if (shop is null)
        {
            _logger.LogError($"Shop with ID {shopId} not found!", shopId);
            return View(itemCategoryCreateVM);
        }

        if (!ModelState.IsValid) return View(itemCategoryCreateVM);

        try
        {
            var itemCategory = _mapper.Map<ItemCategory>(itemCategoryCreateVM);

            _dbContext.Add(itemCategory);
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, $"Error occurred while creating a Item for ShopId {shopId}", shopId);
            return View(itemCategoryCreateVM);
        }

        return RedirectToAction("itemCategories", "Shop", new { id = shopId });
    }

    [HttpGet("ItemCategory/Edit/{id}")]
    public IActionResult Edit(int id)
    {
        var itemCategory = _dbContext.ItemCategories.FirstOrDefault(ItemCategory => ItemCategory.Id == id);
        if (itemCategory is null) return NotFound();
        var itemCategoryEditVM = _mapper.Map<ItemCategoryEditVM>(itemCategory);
        return View(itemCategoryEditVM);
    }

    [HttpPost("ItemCategory/Edit/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ItemCategoryEditVM itemCategoryEditVM)
    {
        if (!ModelState.IsValid) return View(itemCategoryEditVM);

        var ItemCategoryToUpdate = await _dbContext.ItemCategories.FirstOrDefaultAsync(ItemCategory => ItemCategory.Id == id);

        if (ItemCategoryToUpdate is null) return NotFound();

        _mapper.Map(itemCategoryEditVM, ItemCategoryToUpdate);

        try
        {
            _dbContext.Update(ItemCategoryToUpdate);
            await _dbContext.SaveChangesAsync();
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
