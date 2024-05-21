

using System.ComponentModel.DataAnnotations;

namespace Presentation.BlazorApp.Models.Courses;

public class CourseModel
{

    public string Id { get; set; } = null!;
    public string CourseTitle { get; set; } = null!;
    public string CourseIngress { get; set; } = null!;
    public string CourseDescription { get; set; } = null!;
    public string? CourseImageUrl { get; set; }
    public bool IsBestseller { get; set; } = false;
    public string CourseCategory { get; set; } = null!;
    public DateTime Created { get; set; }
    public DateTime LastUpdated { get; set; }
    public RatingModel Rating { get; set; } = null!;
    public PriceModel Price { get; set; } = null!;
    public IncludedModel Included { get; set; } = null!;
    public AuthorModel Author { get; set; } = null!;
    public List<HighlightsModel> Highlights { get; set; } = null!;
    public List<ProgramDetailsModel> Content { get; set; } = null!;
}


public class RatingModel
{
    public decimal InNumbers { get; set; }
    public decimal InProcent { get; set; }
}


public class PriceModel
{
    public decimal OriginalPrice { get; set; }
    public decimal? DiscountPrice { get; set; }
}

public class IncludedModel
{
    public int HoursOfVideo { get; set; }
    public int Articles { get; set; }
    public int Resourses { get; set; }
    public bool LifetimeAccess { get; set; } = false;
    public bool Certificate { get; set; } = false;
}



public class AuthorModel
{
    public string FullName { get; set; } = null!;
    public string Biography { get; set; } = null!;
    public string? ProfileImageUrl { get; set; }
    public SocialMediaModel? SocialMedia { get; set; }
}

public class SocialMediaModel
{
    public string? YouTubeUrl { get; set; }
    public string? Subscribers { get; set; }
    public string? FacebookUrl { get; set; }
    public string? Followers { get; set; }
}



public class HighlightsModel
{
    public string Highlight { get; set; } = null!;
}


public class ProgramDetailsModel
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
}