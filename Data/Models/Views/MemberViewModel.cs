namespace Data.Models.Views
{
    public class MemberViewModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime JoinAt { get; set; }
        public bool IsOwner { get; set; }
    }
}
