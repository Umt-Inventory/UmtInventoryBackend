using Microsoft.AspNetCore.Mvc;
using UmtInventoryBackend.Data;
using UmtInventoryBackend.Entities;
using UmtInventoryBackend.Enums;
using UmtInventoryBackend.Models.WorkspaceDto;

namespace UmtInventoryBackend.Controllers;
[Route("api/[controller]")]
[ApiController]

public class WorkspaceController : Controller
{
    private readonly ApplicationDbContext _dbContext;

    public WorkspaceController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    [Route("GetPaginatedWorkspaces")]
    public ActionResult<PaginatedWorkspace<Workspace>> GetPaginatedWorkspaces(int page = 1, int pageSize = 10, Buildings? building = null)
    {
        var query = _dbContext.Workspaces.AsQueryable();

        if (building.HasValue)
        {
            Buildings buildingValue = building.Value;
            query = query.Where(w => w.Building == buildingValue);
        }

        var workspaces = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        var totalWorkspaces = query.Count();

        var paginatedWorkspaces = new PaginatedWorkspace<Workspace>
        {
            Workspace = workspaces,
            TotalWorkspaces = totalWorkspaces,
            Page = page,
            PageSize = pageSize,
            FilterBuilding = building
        };

        return Ok(paginatedWorkspaces);
    }
    [HttpPost("AddEditWorkspace")]
    public async Task<ActionResult<WorkspaceDto>> PostWorkspace(WorkspaceDto workspaceDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Workspace workspace;

        if (workspaceDto.Id == 0)
        {
            // Creating a new workspace
            workspace = new Workspace
            {
                Name = workspaceDto.Name,
                Type = workspaceDto.Type,
                Building = workspaceDto.Building
            };

            _dbContext.Workspaces.Add(workspace);
            await _dbContext.SaveChangesAsync();
        }
        else
        {
            // Updating an existing workspace
            workspace = await _dbContext.Workspaces.FindAsync(workspaceDto.Id);

            if (workspace == null)
            {
                return NotFound("Workspace not Found!"); // If the workspace with the specified Id does not exist
            }

            workspace.Name = workspaceDto.Name;
            workspace.Type = workspaceDto.Type;
            workspace.Building = workspaceDto.Building;

            await _dbContext.SaveChangesAsync();
        }

        var returnWorkspaceDto = new WorkspaceDto
        {
            Id = workspace.Id,
            Name = workspace.Name,
            Type = workspace.Type,
            Building = workspace.Building
        };

        return CreatedAtAction(nameof(GetPaginatedWorkspaces), new { id = workspace.Id }, returnWorkspaceDto);
    }

    [HttpDelete("DeleteWorkspace/{id}")]
    public async Task<ActionResult> DeleteWorkspace(int id)
    {
        var workspace = await _dbContext.Workspaces.FindAsync(id);
        if (workspace == null) return NotFound(); // Workspace with the specified id not found

        _dbContext.Workspaces.Remove(workspace);
        await _dbContext.SaveChangesAsync();

        return NoContent(); // Workspace successfully deleted
    }

}