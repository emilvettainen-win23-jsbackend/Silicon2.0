namespace Infrastructure.Dtos.User
{
    public class UserDto
    {
        public string Id { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Biography { get; set; }
        public string? ProfileImageUrl { get; set; }
        public bool IsExternalAccount { get; set; }
        public bool DarkMode { get; set; }
        public bool Newsletter { get; set; }
        public string? NewsletterEmail { get; set; }

    }
}
