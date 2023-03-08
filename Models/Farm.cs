using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace backendAPI.Models;

public class Farm
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Name { get; set; }

    public double Latitude  { get; set; }

    public double Longitude { get; set; }
    
    public int NoOfCages { get; set; }

    public bool HasBarge { get; set; }


    //public byte[]? Picture { get; set; }

    public ICollection<FarmWorkers> Workers { get; set; }

}
