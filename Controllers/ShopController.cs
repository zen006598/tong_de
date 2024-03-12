using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using tongDe.Data;
using tongDe.Models;
using tongDe.Models.ViewModels;

namespace tongDe.Controllers;
[Authorize]
[Route("[controller]"), Authorize]
public class ShopController : Controller
{
    private readonly ILogger<ShopController> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public ShopController(
        ILogger<ShopController> logger,
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext dbContext,
        IMapper mapper)
    {
        _logger = logger;
        _userManager = userManager;
        _dbContext = dbContext;
        _mapper = mapper;
    }
    [HttpGet("Index")]
    public async Task<IActionResult> Index()
    {

        var user = await _userManager.GetUserAsync(User);
        if (user is null) return NotFound();
        var Shops = await _dbContext.Shops.Where(s => s.UserId == user.Id).ToListAsync();
        return View(Shops);
    }

    [HttpGet("Create")]
    public IActionResult Create()
    {
        var userId = _userManager.GetUserId(User);

        if (string.IsNullOrEmpty(userId))
        {
            _logger.LogError($"Can not found use by id (ID: {userId}.)");
            throw new Exception($"Can not found use by id (ID: {userId}.)");
        }

        Shop newShop = new Shop
        {
            UserId = userId,
        };

        return View(newShop);
    }

    [HttpPost("Create"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Shop shop)
    {
        var user = await _userManager.GetUserAsync(User);

        if (user is null) return RedirectToAction("Login", "Account");
        shop.Token = Guid.NewGuid();
        try
        {
            user.Shops.Add(shop);
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "An error occurred while updating the database.");
            return View("Home/Shared/Error", new ErrorViewModel { ErrorMessage = "Unable to save changes. Try again, and if the problem persists, see your system administrator." });
        }

        return RedirectToAction("Index", "Shop");
    }
    [HttpGet("Edit/{Id}")]
    public async Task<IActionResult> Edit(int id)
    {

        var shopToUpdate = await _dbContext.Shops.FirstOrDefaultAsync(s => s.Id == id);
        if (shopToUpdate is null) return NotFound();
        var shopEditVM = _mapper.Map<ShopEditVM>(shopToUpdate);
        return View(shopEditVM);
    }

    [HttpPost("Edit/{Id}"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ShopEditVM shopEditVM)
    {
        var shopToUpdate = await _dbContext.Shops.FirstOrDefaultAsync(s => s.Id == id);

        if (shopToUpdate is null) return NotFound();

        _mapper.Map(shopEditVM, shopToUpdate);

        try
        {
            _dbContext.Update(shopToUpdate);
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, $"Error occurred while update a shop");
            return View(shopEditVM);
        }

        return RedirectToAction("Details", "Shop", new { id = shopToUpdate.Id });
    }

    [HttpGet("Details/{id}")]
    public async Task<IActionResult> Details(int id)
    {
        var shop = await _dbContext.Shops.FirstOrDefaultAsync(s => s.Id == id);

        if (shop is null)
        {
            _logger.LogError($"Can not found Shop by id (ID: {id}.)");
            throw new Exception($"Can not found Shop by id (ID: {id}.)");
        }

        var clients = await _dbContext.Clients
              .Where(c => c.ShopId == id)
              .ToListAsync();

        if (clients is null)
        {
            _logger.LogError($"Can not found clients by shopId (ID: {id}.)");
            throw new Exception($"Can not found clients by shopId (ID: {id}.)");
        }

        var shopDetailsVM = new ShopDetailsVM
        {
            Shop = shop,
            Clients = clients
        };

        return View(shopDetailsVM);
    }

    [HttpGet("{Id}/Items")]
    public async Task<IActionResult> Items(int? id, int? itemCategoryId)
    {
        if (!id.HasValue || id == 0) return NotFound();

        ViewData["itemCategoryFilter"] = itemCategoryId ?? 0;

        IQueryable<Item> query = _dbContext.Items.Where(i => i.ShopId == id);

        if (itemCategoryId.HasValue && itemCategoryId != 0)
        {
            query = query.Where(i => i.ItemCategoryId == itemCategoryId.Value);
        }

        var itemViewModels = await query
              .Select(i => new ItemWithItemCategoryVM
              {
                  Id = i.Id,
                  Name = i.Name,
                  ItemCategoryName = i.ItemCategory.Name
              }).ToListAsync();

        var itemCategories = await _dbContext.ItemCategories
            .Where(ic => ic.ShopId == id)
            .ToListAsync();

        var itemViewModel = new ItemsVM
        {
            ShopId = (int)id,
            Items = itemViewModels,
            ItemCategories = itemCategories
        };

        return View(itemViewModel);
    }
    [HttpGet("{Id}/Clients")]
    public async Task<IActionResult> Clients(int? id)
    {
        var shop = await _dbContext.Shops.Where(s => s.Id == id)
                                         .Select(s => new ClientVM
                                         {
                                             ShopId = s.Id,
                                             Clients = s.Clients.Where(c => !c.Cancel).ToList()
                                         })
                                         .FirstOrDefaultAsync();
        if (shop is null)
        {
            _logger.LogError($"Shop Id:({id}) is not found");
            return NotFound();
        }

        var clientViewModel = _mapper.Map<ClientVM>(shop);

        return View(clientViewModel);
    }

    [HttpGet("{Id}/ItemCategories")]
    public async Task<IActionResult> ItemCategories(int? id)
    {
        var shop = await _dbContext.Shops
                      .Include(s => s.ItemCategories)
                      .FirstOrDefaultAsync(s => s.Id == id);
        if (shop is null)
        {
            _logger.LogError($"Shop Id:({id}) is not found");
            return NotFound();
        }

        var itemCategoryViewModel = _mapper.Map<ItemCategoryVM>(shop);

        return View(itemCategoryViewModel);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error");
    }
}
