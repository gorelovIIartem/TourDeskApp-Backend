using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BLL.Interfaces;
using BLL.Services;
using DAL.EF;
using DAL.Entities;
using DAL.Identity;
using DAL.Interfaces;
using DAL.Repositories;
using Microsoft.AspNetCore.Identity;

namespace ProjectSettings
{
    public static class DIRegistration
    {
        public static void RegistrationService(this IServiceCollection services, string connectionName)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder().
                SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            var connectionString = configuration.GetConnectionString(connectionName);
            services.AddDbContext<ApplicationContext>(
                options =>
                {
                    options.UseSqlServer(connectionString);
                    options.UseLazyLoadingProxies();
                });
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITourService, TourService>();
            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<IFeedbackService, FeedbackService>();

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddUserManager<ApplicationUserManager>()
                .AddRoleManager<ApplicationRoleManager>();
        }
    }
}
