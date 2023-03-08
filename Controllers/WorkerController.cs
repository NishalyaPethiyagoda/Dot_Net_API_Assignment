using backendAPI.Data;
using backendAPI.Models;
using backendAPI.Request.Worker;
using backendAPI.Response.Worker;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace backendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkerController : ControllerBase
    {
        private readonly AppDbContext DbContext;     //private => _(first letter simple)

        public WorkerController(AppDbContext DbContext)
        {
            this.DbContext = DbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetWorkers()
        {
            List<Worker> workerList =  await DbContext.Workers.Include(worker=>worker.WorkerDesignation).ToListAsync();

            List<WorkerResponse> workerResponseList = new();  
            
            if(workerList!= null)
            {
                foreach (Worker worker in workerList)
                {
                    WorkerResponse workerResponse = new WorkerResponse
                    {
                        Name = worker.Name,
                        Age = worker.Age,
                        Email = worker.Email,
                        CertifiedDate = worker.CertifiedDate,
                        Designation = worker.WorkerDesignation.name,
                    };
                    workerResponseList.Add(workerResponse);
                }
                return Ok(workerResponseList);
            }
            else
            {
                return Ok("worker List is null");
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostWorkers(WorkerRequest addWorker)
        {
            var worker = new Worker()
            {
                Name = addWorker.Name,
                CertifiedDate = addWorker.CertifiedDate,
                Email = addWorker.Email,
                Age = addWorker.Age,
                DesignationId = addWorker.DesignationId,
            };
            await DbContext.Workers.AddAsync(worker);
            var areChangesSaved = await DbContext.SaveChangesAsync();

            if (areChangesSaved > 0)
            {
                return Ok("New worker is added succesfully");
            }
            else
            {
                return Ok("saving changes were unsuccessful");
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateWorkers([FromRoute] int id, WorkerRequest updateWorker)
        {
            var  workerList = await DbContext.Workers.FindAsync(id);

            if(workerList != null)
            {
                workerList.Name = updateWorker.Name;
                workerList.CertifiedDate= updateWorker.CertifiedDate;
                workerList.Email= updateWorker.Email;
                workerList.Age= updateWorker.Age;
                workerList.DesignationId= updateWorker.DesignationId;

                var areChangesSaved = await DbContext.SaveChangesAsync();

                if (areChangesSaved > 0)
                {
                    return Ok("Updataion successfull");
                }
                else
                {
                    return Ok("saving changes were unsuccessful");
                }
            }
            return NotFound();
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteWorker([FromRoute] int id)
        {
            var worker = await DbContext.Workers.FindAsync(id);

            if(worker != null)
            {
                DbContext.Workers.Remove(worker);
                var areChangesSaved = await DbContext.SaveChangesAsync();

                if (areChangesSaved > 0)
                {
                    return Ok("Record Deleted");
                }
                else
                {
                    return Ok("saving changes were unsuccessful");
                }
            }
            else 
            {
                return Ok("worker is null");
            }
        }
    }
}
