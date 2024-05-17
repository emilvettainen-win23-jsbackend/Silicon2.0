namespace Presentation.BlazorApp.Models.Courses;

public class AuthorModel
{
    public string FullName { get; set; } = null!;
    public string Biography { get; set; } = null!;
    public string? ProfileImageUrl { get; set; }
    public SocialMediaModel? SocialMedia { get; set; }
}
