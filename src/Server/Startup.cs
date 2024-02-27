using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Persistence.Data;
using Server.Middleware;
using Services.Authentication;
using Services.FysiekeServers;
using Services.Projecten;
using Services.Users;
using Services.VirtualMachines;
using Shared.Authentication;
using Shared.Projects;
using Shared.Servers;
using Shared.Users;
using Shared.VirtualMachines;
using System;
using System.Text;

namespace Server
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(logger =>
            {
                logger.ClearProviders();
                logger.AddDebug();
                logger.AddConsole();
            });

            services.AddDbContext<DotNetDbContext>(options =>
             {
                 options.UseSqlServer
                 (
                     Configuration.GetConnectionString("Database")
                 );
             });
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyMethod()
                               .AllowAnyHeader()
                               .AllowAnyOrigin();


                    });
            });

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IFysiekeServerService, FysiekeServerService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IVirtualMachineService, VirtualMachineService>();
            services.AddHttpContextAccessor();
            services.AddTransient(sp => sp.GetRequiredService<IHttpContextAccessor>().HttpContext.User);
            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwtBearerOptions =>
            {
                jwtBearerOptions.RequireHttpsMetadata = true;
                jwtBearerOptions.SaveToken = true;
                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["JWTSettings:SecretKey"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });


            services.AddScoped<DotNetDataInitializer>();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DotNetDbContext dbContext, DotNetDataInitializer seeder)
        {

            if (env.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseHsts();

            }
            //app.UseHttpsRedirection(); remove for android. 
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors("AllowAllOrigins");
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });

            seeder.SeedData();
        }


    }

}