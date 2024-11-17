using Models;

var assignmentsRepository = new Assignments();
var canvasScraper = new Scraper.Controller.CanvasController(assignmentsRepository);

canvasScraper.RunAsync().GetAwaiter().GetResult();

Console.WriteLine(assignmentsRepository.GetCount());

// TODO: make canvas scraping more consistent. Sometimes returns different Count().

