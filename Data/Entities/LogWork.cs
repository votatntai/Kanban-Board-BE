namespace Data.Entities
{
    public partial class LogWork
    {
        public Guid Id { get; set; }
        public Guid IssueId { get; set; }
        public Guid UserId { get; set; }
        public int SpentTime { get; set; }
        public string? Description { get; set; }
        public int RemainingTime { get; set; }
        public DateTime CreateAt { get; set; }

        public virtual Issue Issue { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
