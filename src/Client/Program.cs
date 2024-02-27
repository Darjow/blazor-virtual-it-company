
using Append.Blazor.Sidepanel;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Client.Infrastructure;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Client.Shared;
using Shared.Users;
using Shared.Authentication;
using Blazored.LocalStorage;
using Shared.Servers;
using Shared.Projects;
using Shared.VirtualMachines;

namespace Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddSidepanel();


            //SERVICES
            builder.Services.AddScoped<IUserService, Users.UserService>();
            builder.Services.AddScoped<IAuthenticationService, Authentication.AuthenticationService>();
            builder.Services.AddScoped<AuthenticationStateProvider, AuthenticationProvider>();
            builder.Services.AddScoped<IFysiekeServerService, Servers.FysiekeServerService>();
            builder.Services.AddScoped<IProjectService, Projects.ProjectService>();
            builder.Services.AddScoped<IVirtualMachineService, VirtualMachines.VirtualMachineService>();

            builder.Services.AddBlazoredLocalStorage();

            //AUTHENTICATION
            builder.Services.AddAuthorizationCore(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
                options.AddPolicy("Guest", policy => policy.RequireClaim(ClaimTypes.Name, "Guest"));
                options.AddPolicy("Customer", policy => policy.RequireClaim(ClaimTypes.Role, "Customer"));
                options.AddPolicy("LoggedIn", policy => policy.RequireClaim(ClaimTypes.Role, "Admin", "Customer"));
            });
            builder.Services.AddHttpClient<PublicClient>(client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

            builder.Services.AddHttpClient("AuthenticatedServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                   .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();
            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
                   .CreateClient("AuthenticatedServerAPI"));

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddTransient<AuthorizationMessageHandler>();


            await builder.Build().RunAsync();

        }
    }
}
