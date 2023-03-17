using backendAPI.Data;
using backendAPI.Models;
using backendAPI.Request.Worker;
using backendAPI.Response.Worker;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;


namespace backendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkerController : ControllerBase
    {
        private readonly AppDbContext _dbContext;     

        public WorkerController(AppDbContext DbContext)
        {
            this._dbContext = DbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetWorkers()
        {
            List<Worker> workerList =  await _dbContext.Workers.Include(worker=>worker.WorkerDesignation).ToListAsync();

            List<WorkerResponse> workerResponseList = new();  
            
            if(workerList!= null)
            {
                foreach (Worker worker in workerList)
                {
                    WorkerResponse workerResponse = new()
                    {
                        Id= worker.Id,
                        Name = worker.Name,
                        Age = worker.Age,
                        Email = worker.Email,
                        //CertifiedDate = DateOnly.FromDateTime(worker.CertifiedDate),
                        CertifiedDate = worker.CertifiedDate,

                        DesignationName = worker.WorkerDesignation.name,
                        DesignationId = worker.DesignationId.ToString(),
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
            var workerList = await _dbContext.Workers.Include(worker=>worker.WorkerDesignation).ToListAsync();

            var newWorker = new Worker()
            {
                Name = addWorker.Name,
                CertifiedDate = addWorker.CertifiedDate,
                Email = addWorker.Email,
                Age = addWorker.Age,
                DesignationId = addWorker.DesignationId,
            };
            bool isWorkerDuplicated = false;

            if (workerList!= null)
            {
                
                foreach (Worker worker in workerList)
                {
                    if(worker.Name == newWorker.Name)
                    {
                        isWorkerDuplicated = true;
                        return Ok("A worker exists under the same name. Please check and add new worker");
                    }
                }
            }
            else
            {
                return Ok("worker list not retrieved successfully");
            }

            if(!isWorkerDuplicated)
            {
                await _dbContext.Workers.AddAsync(newWorker);
                var areChangesSaved = await _dbContext.SaveChangesAsync();

                if (areChangesSaved > 0)
                {
                    return Ok("New worker is added succesfully");
                }
                else
                {
                    return Ok("saving changes were unsuccessful");
                }
            }
            else
            {
                return Ok("A worker exists under the same name. Please check and add new worker");
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateWorkers([FromRoute] int id, WorkerRequest updateWorker)
        {
            //var  workerList = await DbContext.Workers.ToListAsync();
            var worker = await _dbContext.Workers.FindAsync(id);

            if (worker != null)
            {
                worker.Name = updateWorker.Name;
                worker.CertifiedDate = updateWorker.CertifiedDate;
                worker.Email = updateWorker.Email;
                worker.Age = updateWorker.Age;
                worker.DesignationId = updateWorker.DesignationId;

                var areChangesSaved = await _dbContext.SaveChangesAsync();

                if (areChangesSaved > 0)
                {
                    return Ok("Updataion successfull");
                }
                else
                {
                    return Ok("saving changes were unsuccessful");
                }

            }
            else
            {
                return Ok("the selected worker details are not retrieved correctly");
            }         
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteWorker([FromRoute] int id)
        {
            var worker = await _dbContext.Workers.FindAsync(id);

            if(worker != null)
            {
                _dbContext.Workers.Remove(worker);
                var areChangesSaved = await _dbContext.SaveChangesAsync();

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
