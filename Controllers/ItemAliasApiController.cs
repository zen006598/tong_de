using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tongDe.Data;
using tongDe.Models;
using tongDe.Models.ViewModels;

namespace tongDe.Controllers;

[ApiController]
[Route("api")]
public class ItemAliasApiController : ControllerBase
{
    private readonly ILogger<ItemController> _logger;
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public ItemAliasApiController(
        ILogger<ItemController> logger,
        ApplicationDbContext dbContext,
        IMapper mapper)
    {
        _logger = logger;
        _dbContext = dbContext;
        _mapper = mapper;
    }

    [HttpGet("GetData")]
    public IActionResult GetData()
    {
        var data = new { Name = "Example", Value = "123" };
        return Ok(data);
    }

    [HttpPost("Item/{ItemId}/ItemAlias/Create")]
    public async Task<ActionResult<ItemAlias>> PostItemAlias(ItemAliasCreateVM itemAliasCreateVM)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var itemAlias = _mapper.Map<ItemAlias>(itemAliasCreateVM);

            _dbContext.Add(itemAlias);
            await _dbContext.SaveChangesAsync();

            var result = new { id = itemAlias.Id, name = itemAliasCreateVM.Name };

            return CreatedAtAction("GetItemAlias", new { id = itemAlias.Id }, result);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, $"Error occurred while creating a ItemAlias for ItemId {itemAliasCreateVM.ItemId}");
        }
        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while creating the item alias." });
    }

}