


namespace backendAPI.Response.FarmWorker
{
    public class FarmWorkerResponse
    {
        public int WorkerId { get; set; }

        public string WorkerName { get; set; }

        public int Age { get; set; }
        public string Designation { get; set; }

        public DateOnly CertifiedDate { get; set; }

        //public List<int>? AlreadyAssignedFarms { get; set; }


    }
}
