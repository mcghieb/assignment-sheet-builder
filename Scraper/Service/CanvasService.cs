using System.Net.Http.Headers;
using Models;
using Newtonsoft.Json;

namespace Scraper.Service;

public static class CanvasService
{
    public static async Task<List<int>> GetCourseIds()
    {
        var CANVAS_TOKEN = Environment.GetEnvironmentVariable("CANVAS_TOKEN");
        var CANVAS_URL = Environment.GetEnvironmentVariable("CANVAS_URL");

        var client = new HttpClient();
        // Add the Authorization header with Bearer token
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CANVAS_TOKEN);

        // Send the GET request
        var response = await client.GetAsync(CANVAS_URL + "/courses");
        
        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"Failed to get courses from CANVAS: {response.StatusCode}");
        
        // Read the response content as a string
        var responseData = await response.Content.ReadAsStringAsync();
        var courses = JsonConvert.DeserializeObject<List<Course>>(responseData);
        
        return ParseCorrectEnrollmentTerm(courses ?? throw new InvalidOperationException());
    }

    private static List<int> ParseCorrectEnrollmentTerm(List<Course> courses)
    {
        var highestTermId = courses.Select(course => course.EnrollmentTermId).Max();

        return courses.Where(course => course.EnrollmentTermId == highestTermId)
                      .Select(course => course.Id)
                      .ToList();
    }
}