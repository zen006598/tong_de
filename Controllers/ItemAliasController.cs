using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tongDe.Data;
using tongDe.Models;
using tongDe.Models.ViewModels;

namespace tongDe.Controllers;

public class ItemAliasController : Controller
{
    private readonly ILogger<ItemAliasController> _logger;
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public ItemAliasController(
        ILogger<ItemAliasController> logger,
        ApplicationDbContext dbContext,
        IMapper mapper)
    {
        _logger = logger;
        _dbContext = dbContext;
        _mapper = mapper;
    }
    [HttpGet("Item/{ItemId}/ItemAlias/Create")]
    public IActionResult Create(int itemId)
    {
        var newItemAlias = new ItemAliasCreateVM { ItemId = itemId };
        return View();
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}
