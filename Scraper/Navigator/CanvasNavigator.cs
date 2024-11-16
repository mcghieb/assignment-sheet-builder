using Microsoft.Playwright;

namespace Scraper.Navigator;

public class CanvasNavigator(IPage page)
{
    // TODO: Refactor to navigate to Home Page
    public async Task NavigateToAssignmentsAsync(string className)
    {
        var classLink = page.Locator($"h3.ic-DashboardCard__header-title:has-text('{className}')");
        await classLink.ClickAsync();
        await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

        var assignmentLink = page.Locator($"a.assignments:has-text('{className}')");
        await assignmentLink.ClickAsync();
        await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
    }
}