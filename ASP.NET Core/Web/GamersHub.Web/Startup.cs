using System;
using System.IO;
using CloudinaryDotNet;
using GamersHub.Services.Data.Categories;
using GamersHub.Services.Data.ForumCategories;
using GamersHub.Services.Data.Forums;
using GamersHub.Services.Data.Games;
using GamersHub.Services.Data.Posts;
using GamersHub.Services.Data.Replies;
using GamersHub.Services.Data.Reviews;
using GamersHub.Services.Data.Users;
using GamersHub.Web.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace GamersHub.Web
{
    using System.Reflection;
    using GamersHub.Data;
    using GamersHub.Data.Common;
    using GamersHub.Data.Common.Repositories;
    using GamersHub.Data.Models;
    using GamersHub.Data.Repositories;
    using GamersHub.Data.Seeding;
    using GamersHub.Services.Data;
    using GamersHub.Services.Mapping;
    using GamersHub.Services.Messaging;
    using GamersHub.Web.ViewModels;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile("appsettings.Development.json", true)
                .AddJsonFile("appsettings.Production.json", true);

            this.configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(this.configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<CookiePolicyOptions>(
                options =>
                {
                    options.CheckConsentNeeded = context => true;
                    options.MinimumSameSitePolicy = SameSiteMode.None;
                });

            services.AddDistributedSqlServerCache(options =>
            {
                options.ConnectionString = this.configuration.GetConnectionString("DefaultConnection");
                options.SchemaName = "dbo";
                options.TableName = "CacheRecords";
            });

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(2);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddResponseCompression(options => { options.EnableForHttps = true; });


            services.AddControllersWithViews(
                options =>
                {
                    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                    options.Conventions.Add(new ControllerNameAttributeConvention());
                });

            services.AddRazorPages();

            Account account = new Account
            (
                this.configuration["Cloudinary:CloudName"],
                this.configuration["Cloudinary:ApiKey"],
                this.configuration["Cloudinary:ApiSecret"]
            );

            Cloudinary cloudinary = new Cloudinary(account);

            services.AddAuthentication().AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = this.configuration["Facebook:AppId"];
                facebookOptions.AppSecret = this.configuration["Facebook:AppSecret"];
            });


            services.AddSingleton(this.configuration);
            services.AddSingleton(cloudinary);


            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // Application services
            services.AddTransient<IEmailSender, NullMessageSender>();
            services.AddTransient<ISettingsService, SettingsService>();
            services.AddTransient<IForumCategoriesService, ForumCategoriesService>();
            services.AddTransient<IPostsService, PostsService>();
            services.AddTransient<IForumsService, ForumsService>();
            services.AddTransient<ICategoriesService, CategoriesService>();
            services.AddTransient<IRepliesService, RepliesService>();
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<IGamesService, GamesService>();
            services.AddTransient<IReviewsService, ReviewsService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            // Seed data on application startup
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                if (env.IsDevelopment())
                {
                    dbContext.Database.Migrate();
                }

                new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter()
                    .GetResult();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseResponseCompression();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseSession();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(
                endpoints =>
                {
                    endpoints.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                    endpoints.MapControllerRoute(
                        "forum",
                        "Forums/{name:minlength(3)}/{id?}",
                        new {controller = "Forums", action = "ByName"});
                    endpoints.MapControllerRoute(
                        "category",
                        "Categories/{name:minlength(3)}/{id}",
                        new {controller = "Categories", action = "ByName"});
                    endpoints.MapControllerRoute(
                        "post",
                        "Posts/{name:minlength(3)}/{id?}",
                        new {controller = "Posts", action = "ByName"});
                    endpoints.MapControllerRoute(
                        "game",
                        "Games/{name:minlength(3)}/{id?}",
                        new {controller = "Games", action = "ByName"});
                    endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                    endpoints.MapRazorPages();
                });
        }
    }
}