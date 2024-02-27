namespace dotnet_2.Tests.Tests.VirtualMachines.Customer
{
    public class Index : BaseTest
    {

        [Test]
        public async Task RetrieveProjectsFromDashboard_AsCustomer()
        {
            await Page.AsCustomer();
            await Page.GotoAsync(TestHelper.Routes.VIRTUALMACHINES);

            var projectContainer = await Page.TextContentAsync("data-test-id=project-item");

            Assert.GreaterOrEqual(projectContainer.Length, 3);
        }

        [Test, Ignore("")]
        public async Task RetrieveVirtualMachinesFromProjects_AsCustomer()
        {
            await Page.AsCustomer();
            await Page.GotoAsync(TestHelper.Routes.VIRTUALMACHINES);

            var buttons = await Page.QuerySelectorAllAsync("data-test-id=toggle-project");

            foreach (var button in buttons)
            {
                await button.ClickAsync();
            }


            var projectContainer = await Page.QuerySelectorAllAsync("data-test-id=project-item");
            var vmContainer = await Page.QuerySelectorAllAsync("data-test-id=vm-container");
            var vmItems = await Page.QuerySelectorAllAsync("data-test-id=vm-item");

            Assert.AreEqual(projectContainer.Count, vmContainer.Count);
            Assert.NotZero(vmItems.Count);
        }

        [Test, Ignore("")]
        public async Task CreateVirtualMachine_AsCustomer()
        {
            await Page.AsCustomer();
            await Page.GotoAsync(TestHelper.Routes.ADD_VIRTUALMACHINES);

            var selectProject = "[data-test-id='project-select']";
            var vmName = "data-test-id=input-name";
            var selectBackup = "data-test-id='backup-select'";
            var selectOs = "data-test-id='os-select'";
            var selectMemory = "data-test-id='memory-select'";
            var selectStorage = "data-test-id='storage-select'";
            var cpus = "data-test-id='input-cpu'";
            var submit = "button[type='submit']";



            await Page.SelectOptionAsync(selectProject, "option:first-child");
            await Page.FillAsync(vmName, "Nieuwe VM");
            await Page.SelectOptionAsync(selectBackup, "option:first-child");
            await Page.SelectOptionAsync(selectOs, "option:first-child");
            await Page.SelectOptionAsync(selectMemory, "option:first-child");
            await Page.SelectOptionAsync(selectStorage, "option:first-child");
            await Page.FillAsync(cpus, "5");
            await Page.ClickAsync(submit);


            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await Page.WaitForNavigationAsync();


            Assert.AreSame(Page.Url.ToString(), TestHelper.Routes.VIRTUALMACHINES);

        }

    }


}
