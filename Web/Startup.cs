using Application;
using Infrastructure;
using Infrastructure.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Hosting;
using Persistense;
using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Web
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
            services.AddHttpContextAccessor();
            services.AddInfrastructure(Configuration);
            services.AddPersistence(Configuration);
            services.AddApplication();
            services.Configure<ForwardedHeadersOptions>(options =>
                {
                    options.ForwardedHeaders = 
                        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                });
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local;
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            var identityUrl = Configuration.GetValue<string>("IdentityUrl");

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                 .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                 {
                     options.Authority = identityUrl;
                     options.RequireHttpsMetadata = true;
                     options.Audience = "resource.api";
                     options.TokenValidationParameters = new TokenValidationParameters()
                     {
                         ValidateAudience = true,
                         ValidateLifetime = true,
                         ValidateIssuerSigningKey = true,
                         ClockSkew = TimeSpan.FromSeconds(10)
                     };
                 });

            services.AddCors(o => o.AddPolicy("Policy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "spa/dist/spa";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders();
            app.UseCors("Policy");
            var supportedCultures = new[] { new System.Globalization.CultureInfo("th-TH") };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("th-TH"),
                // Formatting numbers, dates, etc.
                SupportedCultures = supportedCultures,
                // UI strings that we have localized.
                SupportedUICultures = supportedCultures,
                RequestCultureProviders = new System.Collections.Generic.List<IRequestCultureProvider>{
                    // Order is important, its in which order they will be evaluated
                    new QueryStringRequestCultureProvider(),
                    new CookieRequestCultureProvider()
                }
            });
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
            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseStaticFiles();
            if (env.EnvironmentName == "Test")
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();          
            var pattern = env.EnvironmentName == "Test" ? "{controller}/{action=Index}/{id?}" : "{controller=Empty}/{action=Index}";

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: pattern);
            });

            if (env.EnvironmentName == "Test")
            {
                app.UseSpa(spa =>
                {
                    // To learn more about options for serving an Angular SPA from ASP.NET Core,
                    // see https://go.microsoft.com/fwlink/?linkid=864501
                    spa.Options.SourcePath = "spa";
                });
            }
        }
    }
}
