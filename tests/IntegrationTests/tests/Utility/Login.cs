using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet_2.Tests.Tests.Utility
{
    public class Login: BaseTest
    {
        const int LOGGED_IN_USER__ID = 4;
        const string LOGGED_IN_USER__NAME = "Billy";

        [Test]
        public async Task RetrievalsOfJWT()
        {
            await Page.AsBilly();
            await Page.GotoAsync(TestHelper.Routes.HOME);
            

            //name
            var name = await Page.TextContentAsync("data-test-id=logged-in-name");

            //id
            var nav = await Page.QuerySelectorAsync("[data-test-id='nav-profile']");
            await nav.ClickAsync();

            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);


            Assert.True(name.Contains(LOGGED_IN_USER__NAME));
            Assert.True(Page.Url.ToString().Contains(LOGGED_IN_USER__ID.ToString()));




        }
    }
}
