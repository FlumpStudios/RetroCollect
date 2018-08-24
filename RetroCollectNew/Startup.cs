using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RetroCollectNew.Data;
using RetroCollectNew.Models;
using RetroCollectNew.Services;
using RetroCollectNew.Data.Repositories;
using RetroCollectNew.Data.WorkUnits;
using RetroCollectNew.Business_Logic;

namespace RetroCollectNew
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

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddTransient<IClientRepository, ClientRepository>();
            services.AddTransient<IGameRepository, GameRepository>();
            services.AddTransient<IClientRepository, ClientRepository>();
            services.AddTransient<IUnitOFWork, UnitOFWork>();
            services.AddTransient<ISortingManager, SortingManager>();
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
