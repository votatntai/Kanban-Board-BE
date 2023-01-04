namespace Data.Models.Requests.Create
{
    public class CreateProjectRequestModel
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
