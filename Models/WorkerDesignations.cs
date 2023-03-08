

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backendAPI.Models;

public class WorkerDesignations
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string name { get; set; }

    //public int WorkerId { get; set; }
    public ICollection<Worker> Workers { get; set; }

    public static implicit operator int(WorkerDesignations? v)
    {
        throw new NotImplementedException();
    }
}
