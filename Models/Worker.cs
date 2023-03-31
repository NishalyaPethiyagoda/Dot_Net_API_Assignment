using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backendAPI.Models;

public class Worker
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column(TypeName = "nvarchar(60)")]
    public string Name { get; set; }


    public DateTime CertifiedDate { get; set; }

    [Column(TypeName = "nvarchar(90)")]
    public string Email { get; set; }

    [Column(TypeName = "int")]
    public int Age { get; set; }

    [Column(TypeName = "nvarchar(90)")]
    public string? WorkerPhotoName { get; set; }

    [NotMapped]
    public IFormFile? WorkerPhoto { get; set; }

    [ForeignKey("WorkerDesignation")]
    public int DesignationId { get; set; } 

    public WorkerDesignations WorkerDesignation { get; set; }

    public ICollection<FarmWorkers> FarmWorkers { get; set; }

}

