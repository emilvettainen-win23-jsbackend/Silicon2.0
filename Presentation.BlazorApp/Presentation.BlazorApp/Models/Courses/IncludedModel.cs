namespace Presentation.BlazorApp.Models.Courses;

public class IncludedModel
{
    public int HoursOfVideo { get; set; }
    public int Articles { get; set; }
    public int Resourses { get; set; }
    public bool LifetimeAccess { get; set; } = false;
    public bool Certificate { get; set; } = false;
}
