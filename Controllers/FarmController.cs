using backendAPI.Data;
using backendAPI.Models;
using backendAPI.Request.Farm;
using backendAPI.Response.Farm;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Net;
using static Azure.Core.HttpHeader;

namespace backendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FarmController : ControllerBase
    {
        private readonly AppDbContext DbContext;
        private readonly IWebHostEnvironment _hostEnvironment;

        public FarmController(AppDbContext DbContext, IWebHostEnvironment hostEnvironment)
        {
            this.DbContext = DbContext;
            this._hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> GetFarms()
        {
            List<Farm> farmsList = await DbContext.Farms.ToListAsync();
            List<FarmResponse> farmResponseList = new();

            if (farmsList != null)
            {
                foreach (Farm farm in farmsList)
                {
                    FarmResponse farmResponse = new();


                    farmResponse.Id = farm.Id;
                    farmResponse.Name = farm.Name;
                    farmResponse.Latitude = farm.Latitude;
                    farmResponse.Longitude = farm.Longitude;
                    farmResponse.NoOfCages = farm.NoOfCages;
                    farmResponse.HasBarge = farm.HasBarge;
              
                    farmResponse.ImageName = Path.Combine("http://localhost:12759", "Images", farm.ImageName);
                  
                    farmResponseList.Add(farmResponse);
                }
                return Ok(farmResponseList);
            }
            else
            {
                return Ok("farm List is null");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddFarm([FromForm] FarmRequest addFarm)
        {
            var FarmObj = new Farm()
            {
                Name = addFarm.Name,
                Latitude = addFarm.Latitude,
                Longitude = addFarm.Longitude,
                NoOfCages = addFarm.NoOfCages,
                HasBarge = addFarm.HasBarge,
                ImageFile = addFarm.ImageFile,
            };
            //check if farm is duplicated or unique
            List<Farm> farmList = await DbContext.Farms.ToListAsync();
            bool IsFarmDuplicated = false;

            if (farmList != null)
            {
                for (int i = 0; i < farmList.Count; i++)
                {
                    if (farmList[i].Name != FarmObj.Name)
                    {
                        if (farmList[i].Latitude == FarmObj.Latitude && farmList[i].Longitude == FarmObj.Longitude)
                        {
                            IsFarmDuplicated = true;
                            return Ok("A farm with same GPS location exits under;\n farm Id: " + farmList[i].Id + "\nfarm name: " + farmList[i].Name);
                        }
                        else
                        {
                            IsFarmDuplicated = false;
                        }
                    }
                    else
                    {
                        IsFarmDuplicated = true;
                        return Ok("A farm is registered under the same name");
                    }
                }
            }
            else
            {
                return NotFound("farm list not retrieved from database");
            }

            if (!IsFarmDuplicated)
            {
                FarmObj.ImageName = await SaveImage(FarmObj.ImageFile);
                await DbContext.Farms.AddAsync(FarmObj);
                var areChangesSaved = await DbContext.SaveChangesAsync();

                if (areChangesSaved > 0)
                {
                    return Ok("farm added successfully");
                }
                else
                {
                    return Ok("saving changes were unsuccessful");
                }
            }
            return Ok("farm add task finished");
        }

        [HttpPut]
        [Route("{Id:int}")]
        public async Task<IActionResult> UpdateFarm([FromRoute] int Id, [FromForm] FarmUpdateRequest updateFarm)
        {
            var FarmObj = await DbContext.Farms.FindAsync(Id);

            if (FarmObj != null)
            {
                FarmObj.Name = updateFarm.Name;
                FarmObj.Latitude = updateFarm.Latitude;
                FarmObj.Longitude = updateFarm.Longitude;
                FarmObj.NoOfCages = updateFarm.NoOfCages;
                FarmObj.HasBarge = updateFarm.HasBarge;
                FarmObj.ImageFile = updateFarm.ImageFile;

                FarmObj.ImageName = await SaveImage(updateFarm.ImageFile);


                var areChangesSaved = await DbContext.SaveChangesAsync();

                if (areChangesSaved > 0)
                {
                    return Ok("Updation successful");
                }
                else
                {
                    return Ok("saving changes were unsuccessful");
                }
            }
            else
            {
                return Ok("farm object not found");
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteFarm([FromRoute] int id)
        {
            var farmObj = await DbContext.Farms.FindAsync(id);

            if (farmObj != null)
            {
                DbContext.Farms.Remove(farmObj);
                var areChangesSaved = await DbContext.SaveChangesAsync();

                if (areChangesSaved > 0)
                {
                    return Ok("Deleted");
                }
                else
                {
                    return Ok("saving changes were unsuccessful");
                }
            }
            return Ok("Error: Id passed not available within database");
        }

        [NonAction]
        public async Task<string> SaveImage(IFormFile imageFile)
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
    }
    
}

