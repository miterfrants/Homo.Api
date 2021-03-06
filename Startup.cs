using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

namespace Homo.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, Microsoft.AspNetCore.Hosting.IWebHostEnvironment env)
        {
            Console.WriteLine(env.EnvironmentName);
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile($"secrets.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }
        readonly string AllowSpecificOrigins = "";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            AppSettings appSettings = new AppSettings();
            Configuration.GetSection("Config").Bind(appSettings);
            services.Configure<AppSettings>(Configuration.GetSection("Config"));

            // setup CROS if config file includ CROS section
            IConfigurationSection CROSSection = Configuration.GetSection("CROS");

            string stringCrossOrigins = Configuration.GetSection("CROS").GetValue<string>("Origin");
            if (stringCrossOrigins != null)
            {
                string[] crossOrigins = stringCrossOrigins.Split(",");
                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(crossOrigins));
                services.AddCors(options =>
                {
                    options.AddPolicy(AllowSpecificOrigins,
                        builder =>
                        {
                            builder.WithOrigins(crossOrigins)
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials()
                                .SetPreflightMaxAge(TimeSpan.FromSeconds(600));
                        });
                });
            }
            CultureInfo currentCultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
            services.AddSingleton<ErrorMessageLocalizer>(new ErrorMessageLocalizer(appSettings.Common.LocalizationResourcesPath));
            services.AddSingleton<CommonLocalizer>(new CommonLocalizer(appSettings.Common.LocalizationResourcesPath));
            services.AddSingleton<ValidationLocalizer>(new ValidationLocalizer(appSettings.Common.LocalizationResourcesPath));
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var supportedCultures = new[] {
                new CultureInfo("zh-TW"),
                new CultureInfo("en-US"),
                new CultureInfo("fr"),
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures,
                RequestCultureProviders = new List<IRequestCultureProvider>
                {
                    new QueryStringRequestCultureProvider{QueryStringKey= "culture"}
                }
            });

            String apiPrefix = Configuration.GetSection("Config").GetSection("Common").GetValue<string>("ApiPrefix");
            app.UseCors(AllowSpecificOrigins);
            app.UseHttpsRedirection();
            app.UsePathBase(new PathString($"/{apiPrefix}"));
            app.UseRouting();
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}