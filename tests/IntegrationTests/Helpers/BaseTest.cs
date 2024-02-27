public class BaseTest
{
    protected IPage Page { get; private set; }

    [SetUp]
    public async Task SetUp()
    {
        var playwright = await Playwright.CreateAsync();
        var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false});
        Page = await browser.NewPageAsync();
    }

    [TearDown]
    public async Task TearDown()
    {
        await Page.CloseAsync();
    }
}
