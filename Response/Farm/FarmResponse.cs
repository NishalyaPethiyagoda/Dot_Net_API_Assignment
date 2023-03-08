

namespace backendAPI.Response.Farm
{
    public class FarmResponse
    {
        public string Name { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public int NoOfCages { get; set; }

        public bool HasBarge { get; set; }

        //public byte[]? Picture { get; set; }
    }
}