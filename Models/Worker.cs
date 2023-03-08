using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backendAPI.Models;

public class Worker
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Name { get; set; }

    public DateTime CertifiedDate { get; set; }

    public string Email { get; set; }

    public int Age { get; set; }

    //public bool IsAssignedToFarm { get; set; } = false;


    [ForeignKey("WorkerDesignation")]
    public int DesignationId { get; set; } 
    public WorkerDesignations WorkerDesignation { get; set; }
    public ICollection<FarmWorkers> FarmWorkers { get; set; }

}

