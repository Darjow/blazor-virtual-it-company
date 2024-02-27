using Client.Authentication;
using Client.Infrastructure;
using Client.Users;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Persistence.Data;
using Serilog;
using Shared.Authentication;
using Shared.Users;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;

namespace DatabaseIntegrationTests
{
    public class BaseTest
    {
        private const string ENDPOINT = "https://localhost:5001/api/";

        protected DotNetDbContext DbContext { get; private set; }
        protected HttpClient HttpClient { get; private set; }


        [SetUp]
        public async Task GlobalSetup()
        {
            InitializeDbContext();
            InitializeHttpClient();
            await Seed();
        }


        [OneTimeTearDown]
        public void GlobalTeardown()
        {
           DbContext.Dispose();
        }

        protected async Task LoginAsBilly()
        {
            var response = await HttpClient.PostAsJsonAsync("authentication/login", new AuthenticationRequest.Login
            {
                Email = "billyBillson1997@gmail.com",
                Password = "Password.1"
            });

            var tokenObj = await response.Content.ReadFromJsonAsync<AuthenticationResponse>();
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenObj.Token);
        }


        private void InitializeHttpClient()
        {
            HttpClient = new HttpClient();
            HttpClient.BaseAddress = new Uri(ENDPOINT);
        }

        private string GetConnectionStringFromAppSettings(string key)
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location))
               .AddJsonFile("appsettings.json");
            
            IConfigurationRoot configuration = builder.Build();

            return configuration.GetConnectionString(key);
        }

        private void InitializeDbContext()
        {
            var connString = GetConnectionStringFromAppSettings("Database");
            var options = new DbContextOptionsBuilder<DotNetDbContext>().UseSqlServer(connString).Options;


            DbContext = new DotNetDbContext(options);

        }

        private async Task Seed()
        {
            new DotNetDataInitializer(DbContext).SeedData();
        }
    }
}