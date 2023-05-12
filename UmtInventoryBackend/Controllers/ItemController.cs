using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UmtInventoryBackend.Data;
using UmtInventoryBackend.Entities;
using UmtInventoryBackend.Enums;
using UmtInventoryBackend.Models;

namespace UmtInventoryBackend.Controllers;

[Route("api/[controller]")]
[ApiController]
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
    [Route("GetPaginatedItems")]
    public ActionResult<PaginatedItems<Item>> GetPaginatedItems(int page = 1, int pageSize = 10, UserType filterUserType = UserType.IT)
    {
        var query = _dbContext.Items.AsQueryable();

        if (filterUserType != UserType.IT)
        {
            query = query.Where(i => i.Type == filterUserType);
        }

        var items = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        var totalItems = query.Count();

        var paginatedItems = new PaginatedItems<Item>
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
    public async Task<ActionResult<IEnumerable<User>>> GetItemById(int id)
    {
        var itemExist = _dbContext.Items.Where(x => x.Id == id);

        if (itemExist == null) return NotFound("Item not found!");
        return Ok(itemExist);
    }
    
    [HttpPost]
    [Route("AddEditItem")]
    public async Task<ActionResult<Item>> AddEditItem(ItemDto item)
    {
        var WorkspaceExist = _dbContext.Workspaces.Where(x => x.Id == item.WorkspaceId).FirstOrDefault();
        if (item.Id == 0)
        {
            Item newItem = new Item();
            newItem.Name = item.Name;
            newItem.Condition = item.Condition;
            newItem.Description = item.Description;
            newItem.Quantity = item.Quantity;
            newItem.Price = item.Price;
            newItem.WorkspaceId = item.WorkspaceId;
            newItem.Workspace = WorkspaceExist;
            
            _dbContext.Items.Add(newItem);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetItems), new { id = item.Id }, item);
        }

        var existingItem = _dbContext.Items.FirstOrDefault(x => x.Id == item.Id);
        if (existingItem != null)
        {
            existingItem.Name = item.Name;
            existingItem.Description = item.Description;
            existingItem.Price = item.Price;
            existingItem.Quantity = item.Quantity;
            existingItem.Condition = item.Condition;

            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetItems), new { id = item.Id }, existingItem);
        }

        return NotFound();
    }

    [HttpDelete]
    [Route("DeleteItem/{id}")]
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
    public IActionResult ExportItems()
    {
        var items = _dbContext.Items.ToList();
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