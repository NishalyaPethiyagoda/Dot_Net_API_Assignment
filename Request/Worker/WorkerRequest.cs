using backendAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace backendAPI.Request.Worker
{
    public class WorkerRequest
    {
        public string Name { get; set; }

        //public DateOnly CertifiedDate { get; set; }

        public DateTime CertifiedDate { get; set; }

        public string Email { get; set; }

        public int Age { get; set; }

        //public bool IsAssignedToFarm { get; set; }

        public int DesignationId { get; set; }

    }
}
