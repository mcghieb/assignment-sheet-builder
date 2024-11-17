using Microsoft.Playwright;
using static Scraper.Authenticator.Authenticator;
using Models;

namespace Scraper.Controller;

public class LsController(Assignments assignmentsRepository, IBrowser browser)
{
    private const string LsBaseUrl = "https://learningsuite.byu.edu/";

    public async Task RunAsync()
    {
        var page = await browser.NewPageAsync();
        
        // Authenticate User
        await SignInAsync(page, LsBaseUrl);
        
        // TODO: store links for each class given on home page
        // TODO: loop through links for each class and determine if it has assignments
        // TODO: grab assignments from the Grades page?
    }


}