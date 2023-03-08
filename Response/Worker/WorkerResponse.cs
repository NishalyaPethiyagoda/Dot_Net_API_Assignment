


namespace backendAPI.Response.Worker
{
    public class WorkerResponse
    {
        public string Name { get; set; }

        public DateTime CertifiedDate { get; set; }

        public string Email { get; set; }

        public int Age { get; set; }

        public string Designation { get; set; }

        //public static implicit operator List<object>(WorkerResponse v)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
