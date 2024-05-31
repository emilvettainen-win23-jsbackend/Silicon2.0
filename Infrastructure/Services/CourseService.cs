using Infrastructure.Data.Entities;
using Infrastructure.Factories;
using Infrastructure.Helpers.Responses;
using Infrastructure.Repositories.Course;
using System.Diagnostics;

namespace Infrastructure.Services;

public class CourseService
{
    private readonly SavedCourseRepository _savedCourseRepository;

    public CourseService(SavedCourseRepository savedCourseRepository)
    {
        _savedCourseRepository = savedCourseRepository;
    }

    public async Task<ResponseResult> SaveCourseAsync(string userId, string courseId)
    {
        try
        {
            var existingCourse = await _savedCourseRepository.ExistsAsync(x => x.UserId == userId && x.CourseId == courseId);
            if (existingCourse)
            {
                return ResponseFactory.Exists();
            }
            var result = await _savedCourseRepository.CreateAsync(new SavedCourseEntity { UserId = userId, CourseId = courseId });
            return result != null ? ResponseFactory.Ok() : ResponseFactory.Error();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return ResponseFactory.Error();
        }
    }


    public async Task<IEnumerable<string>> GetSavedCourseAsync(string userId)
    {
        try
        {
            return await _savedCourseRepository.GetSavedCoursesAsync(userId);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return [];
        }
    }


    public async Task<ResponseResult> DeleteOneCourseAsync(string userId, string courseId)
    {
        try
        {
            var response = await _savedCourseRepository.DeleteAsync(x => x.UserId == userId && x.CourseId == courseId);
            return response ? ResponseFactory.Ok() : ResponseFactory.NotFound();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return ResponseFactory.Error();
        }
    }


    public async Task<ResponseResult> DeleteAllCoursesAsync(string userId)
    {
        try
        {
            var result = await _savedCourseRepository.DeleteAllSavedcoursesAsync(userId);
            return result ? ResponseFactory.Ok() : ResponseFactory.NotFound();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return ResponseFactory.Error();
        }
    }
}
