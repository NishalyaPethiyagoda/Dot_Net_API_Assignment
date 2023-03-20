using backendAPI.Data;
using backendAPI.Models;
using backendAPI.Request.Farm;
using backendAPI.Response.Farm;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Azure.Core.HttpHeader;

namespace backendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FarmController : ControllerBase
    {
        private readonly AppDbContext DbContext;

        public FarmController(AppDbContext DbContext)
        {
            this.DbContext = DbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetFarms()
        {
            List <Farm> farmsList = await DbContext.Farms.ToListAsync();
            List <FarmResponse> farmResponseList = new();

           if(farmsList != null)
           {
                foreach(Farm farm in farmsList)
                {
                    FarmResponse farmResponse = new();

                    farmResponse.Id = farm.Id;
                    farmResponse.Name = farm.Name;
                    farmResponse.Latitude = farm.Latitude;
                    //farmResponse.Latitude = farm.Latitude.ToString();
                    farmResponse.Longitude = farm.Longitude.ToString();
                    farmResponse.NoOfCages = farm.NoOfCages;
                    farmResponse.HasBarge = farm.HasBarge;
                    //include pic

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
        public async Task<IActionResult> AddFarm(Request.Farm.FarmRequest addFarm)
        {
            //DAL
            //obtain farm data
            var FarmObj = new Farm()
            {
                Name = addFarm.Name,
                Latitude = addFarm.Latitude,
                Longitude = addFarm.Longitude,
                NoOfCages = addFarm.NoOfCages,
                HasBarge = addFarm.HasBarge,
                //include farm pic
            };

            //check if farm is duplicated or unique
            List<Farm> farmList = await DbContext.Farms.ToListAsync();
            bool IsFarmDuplicated = false;

            if (farmList != null)
            {
                for(int i = 0; i < farmList.Count; i++)
                {
                    if (farmList[i].Name != FarmObj.Name) 
                    {
                        if (farmList[i].Latitude== FarmObj.Latitude && farmList[i].Longitude == FarmObj.Longitude)
                        {
                            IsFarmDuplicated = true;
                            return Ok("A farm with same GPS location exits under;\n farm Id: "+ farmList[i].Id + "\nfarm name: "+ farmList[i].Name);
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

            if(!IsFarmDuplicated)
            {
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
        public async Task<IActionResult> UpdateFarm([FromRoute] int Id, FarmRequest updateFarm )
        {
            var FarmObj = await DbContext.Farms.FindAsync(Id);

            if (FarmObj != null)
            {
                FarmObj.Name = updateFarm.Name;
                FarmObj.Latitude = updateFarm.Latitude;
                FarmObj.Longitude = updateFarm.Longitude;
                FarmObj.NoOfCages = updateFarm.NoOfCages;
                FarmObj.HasBarge = updateFarm.HasBarge;
                //include farm pic

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
    }



}
