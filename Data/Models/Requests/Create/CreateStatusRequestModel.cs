namespace Data.Models.Requests.Create
{
    public class CreateStatusRequestModel
    {
        public string Name { get; set; } = null!;
        public Guid ProjectId { get; set; }
        public int Position { get; set; }
        public string? Description { get; set; }
    }
}
