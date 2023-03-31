using backendAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;

public class WorkerUpdateRequest
{
    public string Name { get; set; }

    public int Age { get; set; }

    public string Email { get; set; }

    public DateTime CertifiedDate { get; set; }

    public int DesignationId { get; set; }

    public IFormFile? ImageFile { get; set; }

}
