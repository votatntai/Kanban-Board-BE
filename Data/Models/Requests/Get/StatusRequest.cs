namespace Data.Models.Requests.Get
{
    public class StatusRequest
    {
        public string? Search { get; set; }
        public Guid? ProjectId { get; set; }
    }
}
