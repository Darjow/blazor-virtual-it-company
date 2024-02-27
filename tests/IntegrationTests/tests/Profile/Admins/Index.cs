using Shouldly;

namespace dotnet_2.Tests.Tests.Profile.Admins
{
    public class Index : BaseTest { 
        
        const int LOGGED_IN_USER__ID = 4;
        const string LOGGED_IN_USER__NAME = "Billy";


        [Test]
        public async Task RetrieveCustomerProfileInformation_AsAdmin()
        {
            await Page.AsAdmin();
            await Page.GotoAsync($"{TestHelper.Routes.CUSTOMER_PROFILE}/{LOGGED_IN_USER__ID}");

            var customerTitle = await Page.TextContentAsync("data-test-id=customer-header");


            Assert.True(customerTitle.Contains(LOGGED_IN_USER__NAME));


        }

    }
}
