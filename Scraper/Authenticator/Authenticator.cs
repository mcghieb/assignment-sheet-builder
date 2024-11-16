using Microsoft.Playwright;

namespace Scraper.Authenticator;

public static class Authenticator
{
    public static async Task SignInAsync(IPage page, string username, string password, string lmsBaseUrl)
    {
        await page.GotoAsync(lmsBaseUrl);
        await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

        await page.FillAsync("input[name=\"username\"]", username);
        await page.FillAsync("input[name=\"password\"]", password);
        await page.ClickAsync("input[name=\"submit\"]");

        await page.WaitForSelectorAsync("#trust-browser-button");
        await page.ClickAsync("#trust-browser-button");

        await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
    }
}