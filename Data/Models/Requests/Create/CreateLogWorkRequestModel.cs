namespace Data.Models.Requests.Create
{
    public class CreateLogWorkRequestModel
    {
        public Guid IssueId { get; set; }
        public Guid UserId { get; set; }
        public int SpentTime { get; set; }
        public string? Description { get; set; }
        public int RemainingTime { get; set; }
    }
}
