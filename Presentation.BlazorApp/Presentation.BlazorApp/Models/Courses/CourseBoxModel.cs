namespace Presentation.BlazorApp.Models.Courses;

public class CourseBoxModel
{
    public string Id { get; set; } = null!;
    public string? CourseImageUrl { get; set; }
    public string CourseTitle { get; set; } = null!;
    public bool IsBestseller { get; set; } = false;
    public string[]? Categories { get; set; }
    public RatingModel Rating { get; set; } = null!;
    public PriceModel Price { get; set; } = null!;
    public IncludedModel Included { get; set; } = null!;
    public AuthorModel Author { get; set; } = null!;
}
