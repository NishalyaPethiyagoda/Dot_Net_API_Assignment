namespace backendAPI.Request.Farm;


public class FarmUpdateRequest
{
    public string Name { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public int NoOfCages { get; set; }

    public bool HasBarge { get; set; }

    public IFormFile ImageFile { get; set; }
}
