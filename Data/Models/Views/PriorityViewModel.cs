namespace Data.Models.Views
{
    public class PriorityViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public int Value { get; set; }
        public string? Description { get; set; }
    }
}
