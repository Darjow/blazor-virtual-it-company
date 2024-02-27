using Client.Authentication;
using Client.Users;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Shared.Authentication;
using Shared.Users;
using System.Net.Http.Json;

namespace DatabaseIntegrationTests.Tests.Authentication
{
    public class Login : BaseTest
    {

        [Test]
        public async Task TestLogin()
        {
            var response = await HttpClient.PostAsJsonAsync("authentication/login", new AuthenticationRequest.Login
            {
                Email = "billyBillson1997@gmail.com",
                Password = "Password.1"
            });

            var content = await response.Content.ReadFromJsonAsync<AuthenticationResponse>();


            Assert.IsNotNull(response.IsSuccessStatusCode);
            Assert.IsNotNull(content?.Token);


        }

    }
}
