namespace Data.Models.View
{
    public class AuthViewModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public IEnumerable<string> Roles { get; set; } = null!;
        public string Token { get; set; } = null!;
    }
}
