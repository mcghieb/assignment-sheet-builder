using Microsoft.Playwright;
using Models;

using var playwright = await Playwright.CreateAsync();
var browser = await playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions
{
    Headless = false,
    SlowMo = 50
});

var assignmentsRepository = new Assignments();
var canvasScraper = new Scraper.Controller.CanvasController(assignmentsRepository, browser);

canvasScraper.RunAsync().GetAwaiter().GetResult();

Console.WriteLine(assignmentsRepository.GetCount());

// TODO: make canvas scraping more consistent. Sometimes returns different Count().

