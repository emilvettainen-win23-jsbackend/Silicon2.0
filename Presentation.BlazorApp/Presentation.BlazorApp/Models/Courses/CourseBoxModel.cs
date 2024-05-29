namespace Presentation.BlazorApp.Models.Courses;

public class CourseBoxModel
{
    public string Id { get; set; } = null!;
    public string? CourseImageUrl { get; set; }
    public string CourseTitle { get; set; } = null!;
    public bool IsBestseller { get; set; } = false;
    public string? Category { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastUpdated { get; set; }
    public RatingBoxModel Rating { get; set; } = null!;
    public PriceBoxModel Prices { get; set; } = null!;
    public IncludedBoxModel Included { get; set; } = null!;
    public AuthorBoxModel Author { get; set; } = null!;
}

public class RatingBoxModel
{
    public decimal InNumbers { get; set; }
    public decimal InProcent { get; set; }
}


public class PriceBoxModel
{
    public decimal OriginalPrice { get; set; }
    public decimal? DiscountPrice { get; set; }
}


public class IncludedBoxModel
{
    public int HoursOfVideo { get; set; }
 
}


public class AuthorBoxModel
{
    public string FullName { get; set; } = null!;

}