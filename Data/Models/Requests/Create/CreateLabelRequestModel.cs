namespace Data.Models.Requests.Create
{
    public class CreateLabelRequestModel
    {
        public string Name { get; set; } = null!;
        public Guid ProjectId { get; set; }
    }
}
