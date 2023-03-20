


namespace backendAPI.Response.Worker
{
    public class WorkerResponse
    {
        public int Id { get; set; } 
        public string Name { get; set; }

        public DateOnly CertifiedDate { get; set; }

        //public DateTime CertifiedDate { get; set; }

        public string Email { get; set; }

        public int Age { get; set; }

        public string DesignationId { get; set; }

        public string DesignationName { get; set; }


        //public static implicit operator List<object>(WorkerResponse v)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
