using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading.Tasks;
using HelPall.Models;
using HelPall.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace HelPall
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {


            // Register cultures here
            //services.Configure<RequestLocalizationOptions>(
            //    opts =>
            //    {
            //        var supportedCultures = new List<CultureInfo>
            //        {
            //            new CultureInfo("en-UK"),
            //            new CultureInfo("en-US"),
            //            new CultureInfo("tr"),
            //        };

            //        opts.DefaultRequestCulture = new RequestCulture("en-UK");
            //        opts.SupportedCultures = supportedCultures;
            //        opts.SupportedUICultures = supportedCultures;
            //    });


            //services.Configure<RequestLocalizationOptions>(options =>
            //{
            //    var supportedCultures = new[]
            //    {
            //        new CultureInfo("en-UK"),
            //        new CultureInfo("tr")
            //     };

            //    options.DefaultRequestCulture = new RequestCulture(culture: "en-UK", uiCulture: "en-UK");
            //    options.SupportedCultures = supportedCultures;
            //    options.SupportedUICultures = supportedCultures;

            //    options.AddInitialRequestCultureProvider(new CustomRequestCultureProvider(async context =>
            //    {
            //        // My custom request culture logic
            //        return new ProviderCultureResult("en-UK");
            //    }));
            //});

            services.AddLocalization(opts => opts.ResourcesPath = "Resources");


            services.AddControllers();

            // requires using Microsoft.Extensions.Options
            services.Configure<PersonDatabaseSettings>(
                Configuration.GetSection(nameof(PersonDatabaseSettings)));

            // services.AddSingleton(new ResourceManager("HelpallAPI.Resources.Controllers.PersonsController", typeof(Startup).GetTypeInfo().Assembly));

            services.AddSingleton<IPersonDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<PersonDatabaseSettings>>().Value);

            services.AddSingleton<PersonService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Helpal API",
                    Description = "Helpal Project ASP.NET Core Web API",
                    TermsOfService = new Uri("https://berkut.tech/helpal/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Berk Ozel",
                        Email = "berk@berkut.tech",
                        Url = new Uri("https://twitter.com/ozel_berk"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = new Uri("https://berkut.tech/helpal/license"),
                    }
                });

                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "The Bearer Token needed for Authorizing requests",
                    Name = "",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {


            var supportedCultures = new[] {
                new CultureInfo("en-UK"),
                new CultureInfo("en-US"),
                new CultureInfo("tr")
            };

            var requestLocalizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-UK"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            };

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRequestLocalization(requestLocalizationOptions);
            // app.UseRequestLocalization(app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>().Value);


            app.UseHttpsRedirection();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;

            });


            //app.UseMiddleware<RequestCorrelationMiddleware>();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
