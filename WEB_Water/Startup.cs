using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB_Water.Data;
using WEB_Water.Data.Entities;
using WEB_Water.Data.Repositories;
using WEB_Water.Helpers;

namespace WEB_Water
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
            //To autenticate the WEB
            services.AddIdentity<User, IdentityRole>(cfg =>
            {
                cfg.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider;
                cfg.Password.RequireDigit = false;
                //cfg.SignIn.RequireConfirmedEmail = true;
                //send token to the email, after click to autenticate the user and have a valid email address, the user can log in
                //cfg.User.RequireUniqueEmail = true;
                //cfg.Password.RequireDigit = false;//to make it simple while we are creating the web
                cfg.Password.RequiredUniqueChars = 0;
                cfg.Password.RequireUppercase = false;
                cfg.Password.RequireLowercase = false;
                cfg.Password.RequireNonAlphanumeric = false;
                cfg.Password.RequiredLength = 6;

            })
                //.AddDefaultTokenProviders()
                .AddEntityFrameworkStores<DataContext>();
            //Link to Connection String (Appsettings.json)
            services.AddDbContext<DataContext>(cfg =>
            {
                cfg.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection"));
            });
            

            //Use the Seed the first time DB is executed
            services.AddTransient<SeedDb>();

            //Dependency injection:

            services.AddScoped<IUserHelper, UserHelper>();
            //services.AddScoped<IImageHelper, ImageHelper>();
            //services.AddScoped<IConverterHelper, ConverterHelper>();

           
            services.AddScoped<IReaderRepository, ReaderRepository>();
            services.AddScoped<IReadingRepository, ReadingRepository>();
            services.AddScoped<IBillRepository, BillRepository>();

            services.ConfigureApplicationCookie(options => 
            {
                options.LoginPath = "/Account/NotAuthorized";
                options.AccessDeniedPath = "/Account/NotAuthorized";
            });

            services.AddControllersWithViews();
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //When the controller doesn't exist
            app.UseStatusCodePagesWithReExecute("/error/{0}");


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
