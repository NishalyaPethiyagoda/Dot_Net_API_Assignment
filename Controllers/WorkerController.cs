using backendAPI.Data;
using backendAPI.Models;
using backendAPI.Request.Worker;
using backendAPI.Response.Worker;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Globalization;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

using System.Net;
using static Azure.Core.HttpHeader;



namespace backendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkerController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _hostEnvironment;

        public WorkerController(AppDbContext DbContext, IWebHostEnvironment hostEnvironment)
        {
            this._dbContext = DbContext;
            this._hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> GetWorkers()
        {
            try
            {
                List<Worker> workerList = await _dbContext.Workers.Include(worker => worker.WorkerDesignation).ToListAsync();

                List<WorkerResponse> workerResponseList = new();

                if (workerList != null)
                {
                    foreach (Worker worker in workerList)
                    {
                        WorkerResponse workerResponse = new()
                        {
                            Id = worker.Id,
                            Name = worker.Name,
                            Age = worker.Age,
                            Email = worker.Email,
                            CertifiedDate = DateOnly.FromDateTime(worker.CertifiedDate),
                            //CertifiedDate = worker.CertifiedDate,

                            DesignationName = worker.WorkerDesignation.name,
                            DesignationId = worker.DesignationId.ToString(),
                            
                            WorkerPhotoSrc = Path.Combine("http://localhost:12759", "Images", worker.WorkerPhotoName ?? "defaultWorkerImage.jpg"),
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
            catch (Exception ex)
            {

                throw;
            }
        }


        [HttpPost]
        public async Task<IActionResult> PostWorkers([FromForm] WorkerRequest addWorker)
        {
            var workerList = await _dbContext.Workers.Include(worker=>worker.WorkerDesignation).ToListAsync();

            var newWorker = new Worker()
            {
                Name = addWorker.Name,
                CertifiedDate = addWorker.CertifiedDate,
                Email = addWorker.Email,
                Age = addWorker.Age,
                DesignationId = addWorker.DesignationId,
                WorkerPhoto= addWorker.WorkerPhoto,
            };

            bool isWorkerDuplicated = false;

            if (workerList!= null)
            { 
                
                foreach (Worker worker in workerList)
                {
                    if(worker.Name == newWorker.Name)
                    {
                        isWorkerDuplicated = true;
                        //return Ok("A worker exists under the same name. Please check and add new worker");
                    }
                }
            }
            else
            {
                return Ok("worker list not retrieved successfully");
            }

            if(!isWorkerDuplicated)
            {
                if(newWorker.WorkerPhoto!= null)
                {
                    newWorker.WorkerPhotoName = await SaveImage(newWorker.WorkerPhoto);
                }
                

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
        [Route("{Id:int}")]
        public async Task<IActionResult> UpdateWorkers([FromRoute] int Id,[FromForm] WorkerUpdateRequest updateWorker)
        {
            //var  workerList = await DbContext.Workers.ToListAsync();
            var worker = await _dbContext.Workers.FindAsync(Id);

            if (worker != null)
            {
                worker.Name = updateWorker.Name;
                worker.CertifiedDate = updateWorker.CertifiedDate;
                worker.Email = updateWorker.Email;
                worker.Age = updateWorker.Age;
                worker.DesignationId = updateWorker.DesignationId;

                //worker.WorkerPhoto = await SaveImage(updateWorker.ImageFile);
                if(updateWorker.ImageFile != null)
                {
                    worker.WorkerPhoto = updateWorker.ImageFile;
                    worker.WorkerPhotoName = await SaveImage(updateWorker.ImageFile);
                }
                

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
        [NonAction]
        public async Task<string> SaveImage(IFormFile imageFile)
        {
            if (imageFile != null)
            {
                string imageName = new string(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-');
                imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(imageFile.FileName);
                var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", imageName);

                using (var fileStream = new FileStream(imagePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }
                return imageName;
            }
            else
            {
                string imageName = Path.Combine("http://localhost:12759", "Images", "defaultWorkerImage.jpg");
                return imageName;
            }
        }
        //[NonAction]
        //public async Task<string> SaveImage(IFormFile imageFile)
        //{
        //    if (imageFile != null)
        //    {
        //        string imageName = new string(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-');
        //        imageName += Path.GetExtension(imageFile.FileName);
        //        var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images1", "FarmImages", imageName);

        //        using (var fileStream = new FileStream(imagePath, FileMode.Create))
        //        {
        //            await imageFile.CopyToAsync(fileStream);
        //        }
        //        return imageName;
        //    }
        //    else
        //    {
        //        string imageName = Path.Combine("http://localhost:12759", "Images1", "FarmImages", "defaultWorkerImage.jpg");
        //        return imageName;
        //    }
        //}
        //[NonAction]
        //public async Task<IFormFile> GetImage(string ImageNme)
        //{
        //    if(ImageNme == null) 
        //    { 
        //        return await 
        //    }
        //    else
        //    {
        //        return 
        //    }
        //}
    }
}
