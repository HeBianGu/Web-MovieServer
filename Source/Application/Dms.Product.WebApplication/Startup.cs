using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dms.Product.General.LocalDataBase;
using Dms.Product.Respository.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Dms.Product.WebApplication
{
    public class Startup
    {
        ILogger _logger;

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;

            _logger = logger;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            _logger.LogInformation("Step ConfigureServices");

            //  Do：获取数据库连接字符串
            string cs = this.Configuration.GetConnectionString("MySqlConnection");

            //  Do：注册数据上下文
            services.AddDbContextWithConnectString<DataContext>(cs);
            //services.AddDefaultDbContextWithConnectString(cs);


            //  Do：依赖注入
            services.AddRespositorys();

            //  Message：注册内服务领域模型
            //services.AddScoped<TestService>();

            //services.AddTransient(l => new HomeControler(new TestServer("fdfdd"))); 

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //  Message：验证用户权限
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
           {
               //options.LoginPath = new PathString("/login");
               //options.AccessDeniedPath = new PathString("/denied");
           }
               );



            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //  Message：注册过滤器
            //services.AddMvc(l=>l.Filters.Add(new SamepleGlobalActionFilter())) ;

            //  Do：注册日志
            var loggingConfiguration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("logging.json", optional: false, reloadOnChange: true)
                .Build();

            services.AddLogging(builder =>
            {
                builder
                    .AddConfiguration(loggingConfiguration.GetSection("Logging"))
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("Dms.Product.WebApplication", LogLevel.Debug)
                    .AddConsole();
            });

            //Session服务
            services.AddSession();


            //services.AddIdentity<ApplicationUser, IdentityRole>()

            //    .AddEntityFrameworkStores<ApplicationDbContext>()

            //    .AddDefaultTokenProviders(); 

            ////Password Strength Setting 
            //services.Configure<IdentityOptions>(options => 
            //{ 
            //    // Password settings

            //    options.Password.RequireDigit = true;

            //    options.Password.RequiredLength = 8;

            //    options.Password.RequireNonAlphanumeric = false;

            //    options.Password.RequireUppercase = true;

            //    options.Password.RequireLowercase = false;

            //    options.Password.RequiredUniqueChars = 6; 

            //    // Lockout settings 
            //    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30); 
            //    options.Lockout.MaxFailedAccessAttempts = 10; 
            //    options.Lockout.AllowedForNewUsers = true; 

            //    // User settings 
            //    options.User.RequireUniqueEmail = true;

            //}); 

            ////Setting the Account Login page 
            //services.ConfigureApplicationCookie(options => 
            //{ 
            //    // Cookie settings 
            //    options.Cookie.HttpOnly = true; 
            //    options.ExpireTimeSpan = TimeSpan.FromMinutes(30); 
            //    options.LoginPath = "/Account/Login"; // If the LoginPath is not set here, 
            //    // ASP.NET Core will default to /Account/Login 
            //    options.LogoutPath = "/Account/Logout"; // If the LogoutPath is not set here, 
            //    // ASP.NET Core will default to /Account/Logout 
            //    options.AccessDeniedPath = "/Account/AccessDenied"; // If the AccessDeniedPath 
            //    // is not set here, ASP.NET Core will default to /Account/AccessDenied 
            //    options.SlidingExpiration = true;

            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //  Do：注册日志

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

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            //app.UseCookiePolicy();


            //  Message：验证中间件
            app.UseAuthentication();

            //  Message：Session has not been configured for this application or request.
            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Login}/{id?}");
            });

        }
    }
}
