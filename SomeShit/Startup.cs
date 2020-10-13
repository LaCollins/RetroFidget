using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Blazorise;
using Blazorise.Material;
using Blazorise.Icons.Material;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Server.IISIntegration;
using RetroFidget.Data.Authorize;
using RetroFidget.Data;

namespace RetroFidget
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //Add Blazorise and Material Middleware.
            services.AddBlazorise(options =>
            {
                options.ChangeTextOnKeyPress = false; //Resolves Performance issues that arise when deployed to IIS
            }).AddMaterialProviders()
                .AddMaterialIcons();

            //Add Authentication Middleware. Configured for use with IIS
            //services.AddAuthentication(IISDefaults.AuthenticationScheme);
            //services.Configure<IISOptions>(options => options.AutomaticAuthentication = true);

            //Add Authorization Middleware. 
            //services.AddAuthorization(config =>
            //{
                //config.AddPolicy("ExamplePolicy", policy => policy.RequireClaim("ExampleClaim", "Test"));
                //Use the Above Format to Add as many additional policies as needed. See https://docs.microsoft.com/en-us/aspnet/core/blazor/security/?view=aspnetcore-3.1 for more info on Blazor Authorization.
            //});

            services.AddRazorPages();
            services.AddServerSideBlazor();
            //example service provided by Microsoft. Remove this Singleton and the WeatherForecast objects in the Data folder if desired.
            services.AddSingleton<WeatherForecastService>();
            //Add Custom Scoped Service to handle individual User Authorization.
            //services.AddScoped<IClaimsTransformation, UserAuthorizationService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            //Enable Authentication Middleware
            //app.UseAuthentication();
            //Enable Authorization Middleware
            //app.UseAuthorization();

            //Required to use Material Design
            app.ApplicationServices
                .UseMaterialProviders()
                .UseMaterialIcons();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
