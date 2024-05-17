

namespace Presentation.BlazorApp.Models.Courses;

public class CreateCourseModel
{
    public string CourseTitle { get; set; } = null!;
    public string CourseDescription { get; set; } = null!;
    public string CourseIngress { get; set; } = null!;
    public bool IsBestseller { get; set; } = false;
    public string? CourseImageUrl { get; set; }
    public string CourseCategory { get; set; } = null!;
    public RatingModel Rating { get; set; } = null!;
    public PriceModel Price { get; set; } = null!;
    public IncludedModel Included { get; set; } = null!;
    public AuthorModel Author { get; set; } = null!;
    public List<HighlightsModel> Highlights { get; set; } = null!;
    public List<ProgramDetailsModel> Content { get; set; } = null!;
}