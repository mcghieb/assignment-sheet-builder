using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Models;
using Scraper.Navigator;
using Scraper.Service;
using static Scraper.Authenticator.Authenticator;

namespace Scraper.Controller;

public class CanvasController(Assignments assignments, IBrowser browser)
{
    private const string CanvasBaseUrl = "https://byu.instructure.com/";
    
    public async Task RunAsync()
    {
        var page = await browser.NewPageAsync();

        // Authenticate user
        await SignInAsync(page, CanvasBaseUrl);

        // Get valid class names
        var validCourseIdList = await CanvasService.GetCourseIds();
        
        foreach (var courseId in validCourseIdList)
        {
            var navigator = new CanvasNavigator(page);
            var scraper = new AssignmentScraper(page);

            try
            {
                // Navigate to assignments page
                await navigator.NavigateToHomeAsync(courseId);

                // Scrape assignments
                await scraper.ScrapeAssignmentsAsync(assignments);
                
                // Return to Canvas homepage
                await page.GotoAsync(CanvasBaseUrl);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing class {courseId}: {ex.Message}");
            }
        }

        
    }
}

public partial class AssignmentScraper(IPage page)
{
    [GeneratedRegex(@"([a-zA-Z]+) (\d{1,2})")]
    private static partial Regex DateRegex();
    
    [GeneratedRegex(@"([A-Z] [A-Z]) (\d{1,3}).*")]
    private static partial Regex CourseCodeRegex();
    
    public async Task ScrapeAssignmentsAsync(Assignments assignments)
    {
        try 
        {
            var rawAssignments = page.Locator("div.ig-row.ig-published.student-view");
            var count = await rawAssignments.CountAsync();
            Console.WriteLine($"Starting to scrape {count} assignments...");
        
            for (var i = 0; i < count; i++)
            {
                var assignment = rawAssignments.Nth(i);
                await ProcessAssignmentAsync(assignment, assignments);
            }

            Console.WriteLine($"Successfully processed {count} assignments");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during assignment scraping: {ex.Message}");
            throw;
        }
    }

    private static async Task ProcessAssignmentAsync(ILocator assignment, Assignments assignments)
    {
        try
        {
            var title = await assignment.Locator("a.ig-title").TextContentAsync() ?? 
                        throw new Exception("Assignment title cannot be null");
            var url = await assignment.Locator("a.ig-title").GetAttributeAsync("href") ?? 
                      throw new Exception("Assignment URL cannot be null");

            // Fix for the timeout issue
            var hasDueDate = await assignment.Locator("div.due_date_display.ig-details__item").CountAsync() > 0;
            var rawDueDate = hasDueDate ? 
                await assignment.Locator("div.due_date_display").TextContentAsync() : null;


            if (!string.IsNullOrWhiteSpace(rawDueDate))
            {
                var dueDate = ParseDate(rawDueDate);
                
                // Console.WriteLine($"{title} - {dueDate} - {url}");
                assignments.AddAssignment(new Assignment(
                    "CS101",
                    title.Trim(),
                    url.Trim(),
                    dueDate
                ));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing assignment: {ex.Message}");
            // Decide if you want to throw here or just log the error
        }
    }

    private static DateTime ParseDate(string? rawDueDate)
    {
        if (string.IsNullOrEmpty(rawDueDate))
            throw new ArgumentException("The due date cannot be null or empty.", nameof(rawDueDate));

        var regex = DateRegex();
        var match = regex.Match(rawDueDate);

        if (!match.Success)
            throw new FormatException("The due date format is invalid.");

        var monthName = match.Groups[1].Value;
        var day = int.Parse(match.Groups[2].Value);

        var months = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
        var month = Array.IndexOf(months, monthName) + 1;

        if (month == 0)
            throw new FormatException($"Invalid month name: {monthName}");

        return new DateTime(DateTime.Now.Year, month, day, 23, 59, 0);
    }

    private static string ParseCourseCode(string? rawCourse)
    {
        if (string.IsNullOrEmpty(rawCourse))
            throw new ArgumentException("The class string cannot be null or empty.", nameof(rawCourse));
        
        var regex = CourseCodeRegex();
        var match = regex.Match(rawCourse);
        
        if (!match.Success)
            throw new FormatException("The class string is invalid.");
        
        var resultString = $"{match.Groups[1].Value.Replace(" ", "")}{match.Groups[2].Value}";
        return resultString;
    }
}
