public static class PageExtensions
{

    public static async Task<IPage> AsCustomer(this IPage page)
    {
        await page.GotoAsync(TestHelper.Routes.LOGIN);
        await page.FillAsync("input#email", "billyBillson1997@gmail.com");
        await page.FillAsync("input#password", "Password.1");
        await page.ClickAsync("button[type=\"submit\"]");
        await page.WaitForNavigationAsync();



        return page;
    }

    public static async Task<IPage> AsBilly(this IPage page)
    {
        await page.GotoAsync(TestHelper.Routes.LOGIN);
        await page.FillAsync("input#email", "billyBillson1997@gmail.com");
        await page.FillAsync("input#password", "Password.1");
        await page.ClickAsync("button[type=\"submit\"]");
        await page.WaitForNavigationAsync();



        return page;
    }

    public static async Task<IPage> AsAdmin(this IPage page)
    {
        await page.GotoAsync(TestHelper.Routes.LOGIN);
        await page.FillAsync("input#email", "darjow@hotmail.com");
        await page.FillAsync("input#password", "Password.1");
        await page.ClickAsync("button[type=\"submit\"]");
        await page.WaitForNavigationAsync();



        return page;
    }
}
