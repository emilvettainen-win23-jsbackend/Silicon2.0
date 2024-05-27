namespace Presentation.BlazorApp.Models.Courses;

public class CourseResultModel
{
    public bool Succeeded { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
    public IEnumerable<CourseModel>? Courses { get; set; }
}