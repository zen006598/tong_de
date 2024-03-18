using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tongDe.Data;
using tongDe.Data.Repository.Interfaces;
using tongDe.Models;
using tongDe.Models.ViewModels;

namespace tongDe.Controllers;
[Authorize]
[Route("[controller]"), Authorize]
public class ShopController : Controller
{
    private readonly ILogger<ShopController> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;
    private readonly IShopRepository _shop;
    private readonly IClientRepository _client;
    private readonly ApplicationDbContext _dbContext;

    public ShopController(
        ILogger<ShopController> logger,
        UserManager<ApplicationUser> userManager,
        IMapper mapper,
        IShopRepository shopRepository,
        IClientRepository clientRepository,
        ApplicationDbContext dbContext)
    {
        _logger = logger;
        _userManager = userManager;
        _mapper = mapper;
        _shop = shopRepository;
        _client = clientRepository;
        _dbContext = dbContext;
    }
    [HttpGet("Index")]
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user is null) return NotFound();

        var Shops = await _shop.GetShops(user.Id);
        return View(Shops);
    }

    [HttpGet("Create")]
    public IActionResult Create()
    {
        var userId = _userManager.GetUserId(User);
        //TODO: make this method in base controller
        if (string.IsNullOrEmpty(userId))
        {
            _logger.LogError($"Can not found use by id (ID: {userId}.)");
            throw new Exception($"Can not found use by id (ID: {userId}.)");
        }

        Shop newShop = new Shop { UserId = userId };
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
            await _shop.SaveAsync();
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

        var shop = await _shop.GetAsync(s => s.Id == id);
        if (shop is null) return NotFound();

        var shopToUpdate = _mapper.Map<ShopEditVM>(shop);
        return View(shopToUpdate);
    }

    [HttpPost("Edit/{Id}"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ShopEditVM shopEditVM)
    {
        var shopToUpdate = await _shop.GetAsync(s => s.Id == id);

        if (shopToUpdate is null) return NotFound();

        _mapper.Map(shopEditVM, shopToUpdate);

        try
        {
            _shop.Update(shopToUpdate);
            await _shop.SaveAsync();
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
        var shop = await _shop.GetAsync(s => s.Id == id);

        if (shop is null)
        {
            _logger.LogError($"Can not found Shop by id (ID: {id}.)");
            throw new Exception($"Can not found Shop by id (ID: {id}.)");
        }

        var clients = await _client.GetClients(id);

        if (clients is null)
        {
            _logger.LogError($"Can not found clients by shopId (ID: {id}.)");
            throw new Exception($"Can not found clients by shopId (ID: {id}.)");
        }
        var shopToDisplay = _mapper.Map<ShopDetailsVM>(shop);

        return View(shopToDisplay);
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
    public async Task<IActionResult> ItemCategories(int id)
    {
        var shopWithItemCategories = await _shop.GetShopWithItemCategoriesAsync(id);
        if (shopWithItemCategories is null)
        {
            _logger.LogError($"Shop Id:({id}) is not found");
            return NotFound();
        }

        var itemCategoriesToDisplay = _mapper.Map<ItemCategoryVM>(shopWithItemCategories);

        return View(itemCategoriesToDisplay);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error");
    }
}
