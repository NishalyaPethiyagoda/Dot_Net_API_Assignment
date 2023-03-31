using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace backendAPI.Models;

public class Farm
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column(TypeName = "nvarchar(50)")]
    public string Name { get; set; }

    [Column(TypeName = "float")]
    public double Latitude  { get; set; }

    [Column(TypeName = "float")]
    public double Longitude { get; set; }

    [Column(TypeName = "int")]
    public int NoOfCages { get; set; }

    [Column(TypeName = "bit")]
    public bool HasBarge { get; set; }

    [Column(TypeName = "nvarchar(130)")]
    public string? ImageName { get; set; }

    [NotMapped]
    public IFormFile? ImageFile { get; set; }

    public ICollection<FarmWorkers> Workers { get; set; }

}
