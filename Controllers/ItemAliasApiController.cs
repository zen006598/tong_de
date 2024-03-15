using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tongDe.Data.Repository;
using tongDe.Models;
using tongDe.Models.ViewModels;

namespace tongDe.Controllers;

[ApiController]
[Authorize]
[Route("api")]
public class ItemAliasApiController : ControllerBase
{
    private readonly ILogger<ItemController> _logger;
    private readonly IMapper _mapper;
    private readonly IItemAliasRepository _itemAlias;

    public ItemAliasApiController(
        ILogger<ItemController> logger,
        IMapper mapper,
        IItemAliasRepository itemAliasRepository)
    {
        _logger = logger;
        _mapper = mapper;
        _itemAlias = itemAliasRepository;
    }

    [HttpPost("Item/{ItemId}/ItemAlias/Create")]
    public async Task<ActionResult<ItemAlias>> Create(ItemAliasCreateVM itemAliasCreateVM)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var itemAlias = _mapper.Map<ItemAlias>(itemAliasCreateVM);

        try
        {
            await _itemAlias.AddAsync(itemAlias);
            await _itemAlias.SaveAsync();

            var result = new { id = itemAlias.Id, name = itemAliasCreateVM.Name };
            return CreatedAtAction("GetItemAlias", new { id = itemAlias.Id }, result);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, $"Error occurred while creating a ItemAlias for ItemId {itemAliasCreateVM.ItemId}");
        }
        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while creating the item alias." });
    }

    [HttpPost("ItemAlias/Delete/{Id}")]
    public async Task<ActionResult<ItemAlias>> Delete(int id)
    {
        var itemAlias = await _itemAlias.GetAsync(ia => ia.Id == id);

        if (itemAlias is null) return NotFound(new { message = $"ItemAlias with ID {id} not found." });
        try
        {
            _itemAlias.Remove(itemAlias);
            await _itemAlias.SaveAsync();

            var deletedItemAlias = new { id = itemAlias.Id, name = itemAlias.Name };
            return CreatedAtAction("GetDeletedItemAlias", new { id = itemAlias.Id }, deletedItemAlias);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, $"Error occurred while deleting ItemAlias with ID {id}.");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while deleting the item alias." });
        }
    }

}
