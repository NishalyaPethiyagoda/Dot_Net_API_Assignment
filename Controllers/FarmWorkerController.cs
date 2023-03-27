using backendAPI.Data;
using backendAPI.Models;
using backendAPI.Request.FarmWorkers;
using backendAPI.Response.FarmWorker;
using backendAPI.Response.Worker;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace backendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FarmWorkerController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public FarmWorkerController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        [HttpGet]
        [Route("{farmId:int}")]
        public async Task<IActionResult> GetFarmWorkers([FromRoute] int farmId)
        {
            //List<Worker> dbWorkersList = await DbContext.Workers.ToListAsync();
            
            List<FarmWorkers> dbFarmWorkersList = await _dbContext.FarmWorkers.Include(a=> a.Worker).ToListAsync();

            List<Worker> workerList = new ();                     //to store worker objs which assigned to that farm    

            //identifying all workers associated to that farm and add to workerList
            if(dbFarmWorkersList != null)
            {
                foreach (FarmWorkers farmWorker in dbFarmWorkersList)
                {
                    if (farmWorker.FarmId == farmId)
                    {
                        workerList.Add(farmWorker.Worker);
                    }
                }
            }
            
            List<FarmWorkerResponse> farmWorkerResponseList = new();       //store response list 

            //forming reponse list - worker object wise
            foreach (Worker worker in workerList)
            {
                var farmWorkerResponse = new FarmWorkerResponse();
                List<int> workersFarmIds = new();                //store list of workerIDs assigned to that farm

                //compiling reponse object with data 
                farmWorkerResponse.WorkerId = worker.Id;
                farmWorkerResponse.WorkerName = worker.Name;
                farmWorkerResponse.Age = worker.Age;
                farmWorkerResponse.CertifiedDate = DateOnly.FromDateTime(worker.CertifiedDate);

                var designatedWorker = await _dbContext.WorkerDesignations.FindAsync(worker.DesignationId);
                if(designatedWorker != null)
                {
                    farmWorkerResponse.Designation = designatedWorker.name;
                }
                else
                {
                    return Ok("error: designations not retrieved correctly");
                }

                farmWorkerResponseList.Add(farmWorkerResponse);
            }
            return Ok(farmWorkerResponseList);
        }

         
        [HttpPost]
        [Route("{farmId:int}/{workerId:int}")]
        public async Task<IActionResult> AddFarmWorkers([FromRoute] int farmId,[FromRoute] int workerId)
        {
            var farmWorkerlist = await _dbContext.FarmWorkers.ToListAsync();   //famWorkers.where.firstOrdefault

            //create farmWorker object with user data
            var newFarmWorker = new FarmWorkers()
            {
                FarmId = farmId,
                WorkerId = workerId,

            };

            //check if duplicated farm woker assignment is present and assign worker
            var isAlreadyNotAssigned = true;
             
            foreach (FarmWorkers farmWorker in farmWorkerlist)
            {
                if (newFarmWorker.FarmId == farmWorker.FarmId)
                {
                    if (newFarmWorker.WorkerId == farmWorker.WorkerId)
                    {
                        isAlreadyNotAssigned = false;
                        return Ok("Input data are duplicating a worker assignment");
                    }
                }
            }

            if (isAlreadyNotAssigned == true)
            {
                await _dbContext.FarmWorkers.AddAsync(newFarmWorker);
                var areChangesSaved = await _dbContext.SaveChangesAsync();

                if (areChangesSaved > 0)
                {
                    return Ok("The worker is successfully assigned to the farm");
                }
                else
                {
                    return Ok("saving changes were unsuccessful");
                }
            }
            else
            {
                return Ok("Worker assignment wasunsuccessful");
            }
        }

        [HttpDelete]
        [Route("{farmId:int}/{workerId:int}")]
        public async Task<IActionResult> DeleteFarmWorker([FromRoute] int farmId, [FromRoute] int workerId)
        {
            var farmWorkerList = await _dbContext.FarmWorkers.ToListAsync();

            if (farmWorkerList != null)
            {
                foreach (FarmWorkers farmWorker in farmWorkerList)
                {
                    if(farmId == farmWorker.FarmId)
                    {
                        if(workerId == farmWorker.WorkerId) 
                        {
                            _dbContext.FarmWorkers.Remove(farmWorker);
                            var areChangesSaved = await _dbContext.SaveChangesAsync();

                            if(areChangesSaved>0)
                            {
                                return Ok("farm worker removed successfully");
                            }
                            else
                            {
                                return Ok("saving changes were unsuccessful");
                            }
                            
                        }
                    }
                }
                return Ok("worker not found. worker not deleted");
            }
            else
            {
                return Ok("farm worker list not retrieved correctly");
            }
            
        }
    }
}
