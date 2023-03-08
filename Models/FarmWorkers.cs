

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backendAPI.Models;

public class FarmWorkers
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int FarmId { get; set; }
    public Farm Farm { get; set; }

    public int WorkerId { get; set; }
    public Worker Worker { get; set; }

    
}
