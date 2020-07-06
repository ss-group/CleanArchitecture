using Application.Common.Constants;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Email;
using Infrastructure.Services;
using Infrastructure.Services.Email;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICurrentUserAccessor, CurrentUserAccessorDev>();
            services.AddScoped<IEmailSetting,EmailSetting>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddTransient<IContentService, ContentService>();
            services.AddTransient((provider) =>
            {
                return new Func<ContentType, IContentService>(
                    (type) => new ContentService(provider.GetService<ICleanDbContext>(), type)
                );
            });
            return services;
        }
    }
}
