namespace dotnet_2.Tests.Tests.VirtualMachines.Admins
{
    public class Index : BaseTest
    {

        [Test]
        public async Task RetrieveProjectsFromDashboard_AsAdmin()
        {
            await Page.AsAdmin();
            await Page.GotoAsync(TestHelper.Routes.VIRTUALMACHINES);

            var projectContainer = await Page.TextContentAsync("data-test-id=project-item");


            Assert.GreaterOrEqual(projectContainer.Length, 3);
        }

        [Test, Ignore("")]
        public async Task RetrieveVirtualMachinesFromProjects_AsAdmin()
        {
            await Page.AsAdmin();
            await Page.GotoAsync(TestHelper.Routes.VIRTUALMACHINES);

            var buttons = await Page.QuerySelectorAllAsync("[data-test-id='toggle-project']");

            foreach (var button in buttons)
            {
                await button.ClickAsync();
            }


            var projectContainer = await Page.QuerySelectorAllAsync("[data-test-id='project-item']");
            var vmContainer = await Page.QuerySelectorAllAsync("[data-test-id='vm-container']");
            var vmItems = await Page.QuerySelectorAllAsync("[data-test-id='vm-item']");

            Assert.AreEqual(projectContainer.Count, vmContainer.Count);
            Assert.NotZero(vmItems.Count);
        }

    }
}
