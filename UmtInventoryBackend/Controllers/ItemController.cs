using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UmtInventoryBackend.Data;
using UmtInventoryBackend.Entities;
using UmtInventoryBackend.Enums;
using UmtInventoryBackend.Models;

namespace UmtInventoryBackend.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class ItemController : Controller
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ItemExcelService _excelService;

    public ItemController(ApplicationDbContext dbContext,ItemExcelService excelService)
    {
        _dbContext = dbContext;
        _excelService = excelService;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Item>>> GetItems()
    {
        if (_dbContext.Items == null) return NotFound();
        return await _dbContext.Items.ToListAsync();
    }

    [HttpGet]
    [Route("GetPaginatedItems/{workspaceId}")]
    [AllowAnonymous]
    public async Task<ActionResult<PaginatedItems<ItemDto>>> GetPaginatedItems(int workspaceId, int page = 1, int pageSize = 10, UserType? filterUserType = null)
    {
        var workspace = await _dbContext.Workspaces.Include(w => w.Items)
            .SingleOrDefaultAsync(w => w.Id == workspaceId);
        if (workspace == null)
        {
            return NotFound($"Workspace with ID {workspaceId} not found.");
        }
    
        var query = workspace.Items.AsQueryable();
    
        if (filterUserType.HasValue)
        {
            query = query.Where(i => i.Type == filterUserType.Value);
        }
    
        var items = query.Skip((page - 1) * pageSize).Take(pageSize)
            .Select(i => new ItemDto
            {
                Id = i.Id,
                Price = i.Price,
                Quantity = i.Quantity,
                Condition = i.Condition,
                Description = i.Description,
                Name = i.Name,
                Type = i.Type
            })
            .ToList();
    
        var totalItems = query.Count();
    
        var paginatedItems = new PaginatedItems<ItemDto>
        {
            Items = items,
            TotalItems = totalItems,
            Page = page,
            PageSize = pageSize,
            FilterUserType = filterUserType
        };
    
        return Ok(paginatedItems);
    }


    [HttpGet]
    [Route("GetItemById")]
    [AllowAnonymous]
    public async Task<ActionResult<ItemDto>> GetItemById(int id)
    {
        var itemExist = await _dbContext.Items.FirstOrDefaultAsync(x => x.Id == id);

        if (itemExist == null)
        {
            return NotFound("Item not found!");
        }

        var itemDto = new ItemDto
        {
            Id = itemExist.Id,
            Price = itemExist.Price,
            Quantity = itemExist.Quantity,
            Condition = itemExist.Condition,
            Description = itemExist.Description,
            Name = itemExist.Name,
            Type = itemExist.Type
        };

        return Ok(itemDto);
    }


    
    [HttpPost]
    [Route("AddEditItem")]
    [AllowAnonymous]
    public async Task<ActionResult<Item>> AddEditItem(AddEditItemDto addEditItem)
    {
        var WorkspaceExist = _dbContext.Workspaces.Where(x => x.Id == addEditItem.WorkspaceId).FirstOrDefault();
        if (addEditItem.Id == 0)
        {
            Item newItem = new Item();
            newItem.Name = addEditItem.Name;
            newItem.Condition = addEditItem.Condition;
            newItem.Description = addEditItem.Description;
            newItem.Quantity = addEditItem.Quantity;
            newItem.Price = addEditItem.Price;
            newItem.WorkspaceId = addEditItem.WorkspaceId;
            newItem.Workspace = WorkspaceExist;
            newItem.Type = addEditItem.Type;
            
            _dbContext.Items.Add(newItem);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetItems), new { id = addEditItem.Id }, addEditItem);
        }

        var existingItem = _dbContext.Items.FirstOrDefault(x => x.Id == addEditItem.Id);
        if (existingItem != null)
        {
            existingItem.Name = addEditItem.Name;
            existingItem.Description = addEditItem.Description;    
            existingItem.Price = addEditItem.Price;
            existingItem.Quantity = addEditItem.Quantity;
            existingItem.Condition = addEditItem.Condition;

            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetItems), new { id = addEditItem.Id }, existingItem);
        }

        return NotFound();
    }

    [HttpDelete]
    [Route("DeleteItem/{id}")]
    [AllowAnonymous]
    public async Task<ActionResult> DeleteItem(int id)
    {
        var item = await _dbContext.Items.FindAsync(id);
        if (item == null)
        {
            return NotFound(); // Item with the specified id not found
        }

        _dbContext.Items.Remove(item);
        await _dbContext.SaveChangesAsync();

        return NoContent(); // Item successfully deleted
    }

    [HttpGet]
    [Route("ExportItems")]
    public IActionResult ExportItems(UserType? userType = null)
    {
        IQueryable<Item> query = _dbContext.Items;

        if (userType != null)
        {
            query = query.Where(item => item.Type == userType);
        }

        var items = query.ToList();
        var excelBytes = _excelService.ExportItemsToExcel(items);

        var fileContent = new ByteArrayContent(excelBytes);
        fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
        {
            FileName = "Items.xlsx"
        };
        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

        var response = new HttpResponseMessage(HttpStatusCode.OK);
        response.Content = fileContent;

        return File(excelBytes, fileContent.Headers.ContentType.ToString(), "Items.xlsx");
    }


    
}