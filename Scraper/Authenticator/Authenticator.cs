using Microsoft.Playwright;

namespace Scraper.Authenticator;

public static class Authenticator
{
    
    public static async Task SignInAsync(IPage page, string lmsBaseUrl)
    {
        var username = Environment.GetEnvironmentVariable("BYU_USER");
        var password = Environment.GetEnvironmentVariable("BYU_PASS");
        
        await page.GotoAsync(lmsBaseUrl);
        await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
        {
            await page.FillAsync("input[name=\"username\"]", username);
            await page.FillAsync("input[name=\"password\"]", password);
            await page.ClickAsync("input[name=\"submit\"]");

            await page.WaitForSelectorAsync("#trust-browser-button");
            await page.ClickAsync("#trust-browser-button");

            await page.WaitForURLAsync(lmsBaseUrl);
        }
    }
}