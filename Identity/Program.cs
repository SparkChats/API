using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;
using Sparkle.Application.Models;
using Sparkle.DataAccess;
using System.Reflection;

namespace Sparkle.Identity
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            IServiceCollection services = builder.Services;

            services.AddDatabase(builder.Configuration);
            services.AddControllers();
            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
            });

            services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssembly(Assembly
                    .GetExecutingAssembly());
            });

            //add IdentitySettings to DI as IOptions

            services.Configure<IdentitySettings>(builder.Configuration
                .GetSection(IdentitySettings.SectionName));

            services.AddIdentity<User, Role>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Lockout.MaxFailedAccessAttempts = 5;

                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;

                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.AddIdentityServer4WithConfiguration()
                .AddDeveloperSigningCredential();

            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "Sparkle.Identity.Cookie";
                config.LoginPath = "/Authentication/Login";
                config.LogoutPath = "/Authentication/Logout";
            });

            WebApplication app = builder.Build();

            app.MapControllers();
            app.UseMvc();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(app.Environment.ContentRootPath, "wwwroot")),
                RequestPath = "/wwwroot"
            });

            app.UseIdentityServer();

            app.Run();
        }
    }
}