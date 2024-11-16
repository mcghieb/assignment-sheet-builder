using Microsoft.Playwright;

namespace Scraper.Navigator;

public class CanvasNavigator(IPage page)
{
    public async Task NavigateToHomeAsync(int courseId)
    {
        await page.GotoAsync($"https://byu.instructure.com/courses/{courseId}");
        await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
    }
}