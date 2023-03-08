

namespace backendAPI.Request.Farm
{
    public class FarmRequest
    {
        public string Name { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public int NoOfCages { get; set; }

        public bool HasBarge { get; set; }

        //public byte[]? Picture { get; set; }  //farm should have a picture. cant be nullable
    }
}
