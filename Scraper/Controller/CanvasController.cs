using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Models;
using Scraper.Navigator;
using static Scraper.Authenticator.Authenticator;

namespace Scraper.Controller;

public class CanvasController
{
    private const string CanvasBaseUrl = "https://byu.instructure.com";
    private readonly string Username = Environment.GetEnvironmentVariable("BYU_USER");
    private readonly string Password = Environment.GetEnvironmentVariable("BYU_PASS");

    public async Task RunAsync()
    {
        using var playwright = await Playwright.CreateAsync();
        var browser = await playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false,
            SlowMo = 50
        });

        var page = await browser.NewPageAsync();

        // Authenticate user
        await SignInAsync(page, Username, Password, CanvasBaseUrl);

        // Get valid class names
        var validClassNames = await GetValidClassesAsync();

        foreach (var className in validClassNames)
        {
            var navigator = new CanvasNavigator(page);
            var scraper = new AssignmentScraper(page);

            try
            {
                // Navigate to assignments page
                await navigator.NavigateToAssignmentsAsync(className);

                // Scrape assignments
                var assignments = await scraper.ScrapeAssignmentsAsync();

                Console.WriteLine($"{className}: {System.Text.Json.JsonSerializer.Serialize(assignments)}\n");

                // Return to Canvas homepage
                await page.GotoAsync(CanvasBaseUrl);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing class {className}: {ex.Message}");
            }
        }
    }

    // TODO: make the api call for this
    private static Task<List<string>> GetValidClassesAsync()
    {
        // Replace this with logic to fetch valid class names.
        return Task.FromResult<List<string>>(["C S 180-001: Intro to Data Science"]);
    }
}


public partial class AssignmentScraper(IPage page)
{
    public async Task<Assignments> ScrapeAssignmentsAsync()
    {
        var assignments = new Assignments();

        await page.WaitForSelectorAsync("li.assignment");
        var rawAssignments = page.Locator("li.assignment");

        var count = await rawAssignments.CountAsync();
        Console.WriteLine($"\n {count} assignments found!\n");

        for (var i = 0; i < count; i++)
        {
            var rawAssignment = rawAssignments.Nth(i);

            var name = await rawAssignment.Locator(".ig-title").TextContentAsync();
            var link = await rawAssignment.Locator(".ig-title").GetAttributeAsync("href");

            var rawDueDate = await rawAssignment.Locator(".assignment-date-due .screenreader-only").TextContentAsync();
            var dueDate = (rawDueDate != null) ? ParseDate(rawDueDate) : DateTime.Now;

            if (dueDate > DateTime.Now) 
                break;
            
            if (name != null && link != null) 
                assignments.AddAssignment(new Assignment("DUMMY-CC", name, link, dueDate));
        }

        Console.WriteLine($"{assignments.GetCount()} future assignments!\n");
        return assignments;
    }

    private static DateTime ParseDate(string rawDueDate)
    {
        if (string.IsNullOrEmpty(rawDueDate))
            throw new ArgumentException("The due date cannot be null or empty.", nameof(rawDueDate));

        var regex = MyRegex();
        var match = regex.Match(rawDueDate);

        if (!match.Success)
            throw new FormatException("The due date format is invalid.");

        var monthName = match.Groups[1].Value;
        var day = int.Parse(match.Groups[2].Value);
        var hour = int.Parse(match.Groups[3].Value);
        var minute = int.Parse(match.Groups[4].Value);
        var period = match.Groups[5].Value;

        if (period == "pm" && hour != 12) hour += 12;

        var months = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
        var month = Array.IndexOf(months, monthName) + 1;

        if (month == 0)
            throw new FormatException($"Invalid month name: {monthName}");

        return new DateTime(DateTime.Now.Year, month, day, hour, minute, 0);
    }

    [GeneratedRegex(@"([a-zA-Z]+) (\d{1,2}) at (\d{1,2}):(\d{2})(am|pm)")]
    private static partial Regex MyRegex();
}
