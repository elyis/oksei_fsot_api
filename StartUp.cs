using System.Text;
using oksei_fsot_api.src.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using webApiTemplate.src.App.IService;
using webApiTemplate.src.App.Service;
using webApiTemplate.src.App.Provider;
using webApiTemplate.src.Domain.Entities.Config;
using oksei_fsot_api.src.Utility;
using oksei_fsot_api.src.Domain.Entities.Response;
using Microsoft.EntityFrameworkCore;
using oksei_fsot_api.src.Domain.Models;
using oksei_fsot_api.src.Domain.Enums;

namespace oksei_fsot_api
{

    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var jwtSettings = _config.GetSection("JwtSettings").Get<JwtSettings>();
            var aes256Settings = _config.GetSection("Aes256Settings").Get<Aes256Settings>();

            services.AddControllers(config =>
            {
                config.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>();
            })
                .AddNewtonsoftJson()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressMapClientErrors = true;
                });

            services.AddCors(setup =>
            {
                setup.AddDefaultPolicy(options =>
                {
                    options.AllowAnyHeader();
                    options.AllowAnyOrigin();
                    options.AllowAnyMethod();
                });
            });
            services.AddEndpointsApiExplorer();

            services.AddDbContext<AppDbContext>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
                });

            services.AddAuthorization();
            services.AddLogging(builder =>
            {
                builder.AddConsole();
            });


            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "oksei_fsot_api",
                    Description = "Api",
                });

                options.EnableAnnotations();
            });

            services.AddSwaggerGenNewtonsoftSupport();

            services.AddSingleton(jwtSettings);
            services.AddSingleton(aes256Settings);

            services.AddSingleton<IJwtService, JwtService>();
            services.AddSingleton<Aes256CryptoProvider>();

            services.AddSingleton<PasswordGenerator>();
            services.AddSingleton<LoginGenerator>();

            services.AddSingleton<ExcelGenerator<MarkLogBody>>();
            services.AddSingleton<ExcelGenerator<TeacherPerformanceSummary>>();
            // services.AddHostedService<MonthlyHostedWorker>();

            services.Scan(scan =>
            {
                scan.FromCallingAssembly()
                    .AddClasses(classes =>
                        classes.Where(type =>
                            type.Name.EndsWith("Repository")))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime();
            });

            services.Scan(scan =>
            {
                scan.FromCallingAssembly()
                    .AddClasses(classes =>
                        classes.Where(type =>
                            type.Name.EndsWith("Manager")))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime();
            });

            services.Scan(scan =>
            {
                scan.FromCallingAssembly()
                    .AddClasses(classes =>
                        classes.Where(type =>
                            type.Name.EndsWith("Service")))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime();
            });
        }

        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors();
            app.UseHttpLogging();
            app.UseRequestLocalization();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            InitDatabase();
            app.Run();
        }

        private void InitDatabase()
        {
            var context = new AppDbContext(new DbContextOptions<AppDbContext>(), _config);
            var admin = context.Users.FirstOrDefault(e => e.Login == "torvalid1969");
            if (admin == null)
            {
                var user = new UserModel
                {
                    Login = "torvalid1969",
                    Password = "roottoor",
                    Fullname = "Админ Админ Админович",
                    RoleName = UserRole.Appraiser.ToString(),
                };
                context.Users.Add(user);
                context.SaveChanges();
            }
        }
    }
}