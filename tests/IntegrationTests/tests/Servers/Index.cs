using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet_2.Tests.Tests.Servers
{
    public class Index: BaseTest
    {
        const int SERVERS_IN_SEEDING = 3; //could use config or static constants file here maybe. 
        const int SERVER_ID = 1;


        [Test]
        public async Task RetrieveServersOnIndex_AsAdmin()
        {
            await Page.AsAdmin();
            await Page.GotoAsync(TestHelper.Routes.SERVERS);

            var serverContainers = await Page.TextContentAsync("data-test-id=server-item");


            Assert.GreaterOrEqual(serverContainers.Length, SERVERS_IN_SEEDING);
        }

        [Test]
        public async Task RetrieveVirtualMachinesFromDetails_AsAdmin()
        {
            await Page.AsAdmin();
            await Page.GotoAsync($"{TestHelper.Routes.BY_ID_SERVERS}/{SERVER_ID}");

            var vmContainers = await Page.TextContentAsync("data-test-id=vm-item");


            Assert.GreaterOrEqual(vmContainers.Length, 1);
        }
    }

}
