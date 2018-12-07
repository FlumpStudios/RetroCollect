using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ApplicationLayer.ApplicationData;
using ApplicationLayer.Models;
using ApplicationLayer.Services;
using ApplicationLayer.ApplicationData.WorkUnits;
using ApplicationLayer.Business_Logic.Sorting;
using DataAccess.EntityFramework.Repositories;
using ModelData;
using DataAccess.Repositories;
using DataAccess.WorkUnits;
using ApplicationLayer.Business_Logic.Builders;
using ApplicationLayer.Business_Logic.FileHandling;
using HttpAccess;

namespace ApplicationLayer
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

 
            services.AddAuthentication().AddTwitter(twitterOptions =>
            {
                twitterOptions.ConsumerKey = Configuration["Authentication:Twitter:ConsumerKey"];
                twitterOptions.ConsumerSecret = Configuration["Authentication:Twitter:ConsumerSecret"];
            }).AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = Configuration["Authentication:Facebook:AppId"];
                facebookOptions.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
            }).AddLinkedIn(options =>
            {
                options.ClientId = Configuration["Authentication:LinkedIn:AppId"];
                options.ClientSecret = Configuration["Authentication:LinkedIn:AppSecret"];
            });
            

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddTransient<IClientRepository, ClientRepository>();
            services.AddTransient<IGameRepository, GameRepository>();
            services.AddTransient<IClientRepository, ClientRepository>();
            services.AddTransient<IUnitOFWork, UnitOFWork>();
            services.AddTransient<ISortingManager, SortingManager>();
            services.AddTransient<IGameListResponseBuilder, GameListResponseBuilder>();
            services.AddTransient<IFileHandler, FileHandler>();
            services.AddTransient<IHttpManager, HttpManager>();


            services.AddMvc();

            services.AddDbContext<RetroCollectNewContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("RetroCollectNewContext")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ApplicationDbContext db, IServiceProvider serviceProvider)
        {
            db.Database.Migrate();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            CreateRoles(serviceProvider).Wait();
        }

        private async Task CreateRoles(IServiceProvider serviceProvider)

        {

            //adding custom roles

            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roleNames = { "Admin", "Manager", "Member" };

            IdentityResult roleResult;
            foreach (var roleName in roleNames)
            {
                //creating the roles and seeding them to the database
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            //creating a super user who could maintain the web app

            //var poweruser = new ApplicationUser
            //{
            //    UserName = Configuration.GetSection("UserSettings")["UserEmail"],
            //    Email = Configuration.GetSection("UserSettings")["UserEmail"]
            //};

            //string UserPassword = Configuration.GetSection("UserSettings")["UserPassword"];
            //var _user = await UserManager.FindByEmailAsync(Configuration.GetSection("UserSettings")["UserEmail"]);

            //TODO: read from config file
            var poweruser = new ApplicationUser
            {
                UserName = "paul.marrable@iress.co.uk",
                Email = "paul.marrable@iress.co.uk"
            };

            string UserPassword = "Pacman_1981";
            var _user = await UserManager.FindByEmailAsync("paul.marrable@iress.co.uk");

            if (_user == null)
            {
                var createPowerUser = await UserManager.CreateAsync(poweruser, UserPassword);
                if (createPowerUser.Succeeded)
                {
                    //here we tie the new user to the "Admin" role
                    await UserManager.AddToRoleAsync(poweruser, "Admin");
                }
            }
        }
    }
}
