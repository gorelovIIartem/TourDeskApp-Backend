using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using BLL.DTO;
using ProjectSettings;
using WebApi.Models;
using WebApi.Configuration;
using Serilog;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.RegistrationService("DefaultConnection");

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<MapperSettings>();
                cfg.CreateMap<TourDTO, TourModel>().ReverseMap();
                cfg.CreateMap<FeedbackDTO, FeedbackModel>().ReverseMap();
                cfg.CreateMap<TicketDTO, TicketModel>().ReverseMap();
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
                options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = AuthOptions.ISSUER,
                        ValidateAudience = true,
                        ValidAudience = AuthOptions.AUDIENCE,
                        ValidateLifetime = true,
                        IssuerSigningKey = AuthOptions.GetSummetricSecurityKey(),
                        ValidateIssuerSigningKey = true
                    };
                });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalHost4200", builder => builder.WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials());
            });
            services.AddSignalR();

            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("AllowLocalHost4200"));
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            Log.Logger = new LoggerConfiguration().ReadFrom
                .Configuration(Configuration)
                .CreateLogger();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseAuthentication();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseCors("AllowLocalHost4200");
            app.UseErrorHandlingMiddleware();
            app.UseMvc();
        }
    }
}
