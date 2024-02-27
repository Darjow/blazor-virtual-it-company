using Shouldly;

namespace dotnet_2.Tests.Tests.Profile.Customers
{
    public class Index : BaseTest
    {
        const int LOGGED_IN_USER__ID = 4;
        const string LOGGED_IN_USER__NAME = "Billy";


        [Test]
        public async Task RetrieveOwnProfileInformation_AsCustomer()
        {
            await Page.AsBilly();
            await Page.GotoAsync($"{TestHelper.Routes.CUSTOMER_PROFILE}/{LOGGED_IN_USER__ID}");

            var customerTitle = await Page.TextContentAsync("data-test-id=customer-header");


            Assert.True(customerTitle.Contains(LOGGED_IN_USER__NAME));


        }

        [Test]
        public async Task RetrieveOtherProfileInformation_AsCustomer()
        {
            bool visible = true;

            await Page.AsCustomer();
            await Page.GotoAsync($"{TestHelper.Routes.CUSTOMER_PROFILE}/{LOGGED_IN_USER__ID + 1}");

            try
            {
                await Page.WaitForSelectorAsync("data-test-id=customer-header");
            }
            catch (TimeoutException)
            {
                visible = false;
            }

            var customerError = await Page.TextContentAsync("data-test-id=customer-not-allowed");

            Assert.True(!visible);
            Assert.That(customerError.Contains("Klant bestaat niet of dit is niet jouw profiel"));


        }

    }
}
