namespace Data.Models.Requests.Update
{
    public class UpdateStatusRequestModel
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; } = null!;
        public int? Position { get; set; }
        public bool? IsFirst { get; set; }
        public bool? IsLast { get; set; }
        public string? Description { get; set; }
        public ICollection<UpdateIssueRequestModel> Issues { get; set; } = null!;
    }
}
