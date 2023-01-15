namespace Data.Models.Requests.Create
{
    public class CreateIssueRequestModel
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; } = null!;
        public Guid? AssigneeId { get; set; }
        public int? EstimateTime { get; set; }
        public Guid? PriorityId { get; set; }
        public Guid ProjectId { get; set; }
        public Guid StatusId { get; set; }
        public int Position { get; set; }
        public Guid? TypeId { get; set; }
        public DateTime? DueDate { get; set; }
        public Guid? ReporterId { get; set; }
    }
}
