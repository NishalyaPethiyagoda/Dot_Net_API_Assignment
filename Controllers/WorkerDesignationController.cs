using backendAPI.Data;
using backendAPI.Models;
using backendAPI.Response.WorkerDesignations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backendAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WorkerDesignationController : Controller
{
    private readonly AppDbContext _dbContext;

    public WorkerDesignationController(AppDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetWorkerDesignations()
    {
        List<WorkerDesignations> designations = await _dbContext.WorkerDesignations.ToListAsync();
        List<WorkerDesignationResponse> designationResponseList = new();

        if(designations != null)
        {
            foreach(WorkerDesignations designation in designations)
            {
                WorkerDesignationResponse designationResponse = new()
                {
                    Id = designation.Id,
                    Name = designation.name,
                };
                designationResponseList.Add(designationResponse);
            }
            return Ok(designationResponseList);
        }
        else
        {
            return Ok("worker designations not recieved to API");
        }
    }
}
