//using backendAPI.Data;
//using backendAPI.Models;
//using Microsoft.AspNetCore.Mvc;

//namespace backendAPI.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class WorkerDesignationsController : Controller
//    {
//        private readonly AppDbContext DbContext;
        
//        public WorkerDesignationsController(AppDbContext dbContext)
//        {
//            DbContext = dbContext;
//        }

//        [HttpPost]
//        public async Task<IActionResult> AddWorkerDesignation(Request.WorkerDesignaions.AddDesignation addDesignation)
//        {
//            var WorkerDesignation = new WorkerDesignations()
//            {

//            }
//            return Ok();
//        }
//    }
//}
