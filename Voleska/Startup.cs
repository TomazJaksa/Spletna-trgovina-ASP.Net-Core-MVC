using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Stripe;
using Voleska.Data;
using Newtonsoft.Json.Serialization;
using System.Text.Json.Serialization;
using Voleska.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Voleska.Services;

namespace Voleska
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
            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.ClientId = "";
                    options.ClientSecret = "";
                    options.SignInScheme = IdentityConstants.ExternalScheme;
                    
                
                })
                .AddFacebook(options => {
                    options.AppId = "";
                    options.AppSecret = "";
                });

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddLocalization(opts => {
                opts.ResourcesPath = "Resources";
            });

            services.AddTransient<Services.IMailService, SendGridMailService>();
            services.AddControllersWithViews().
                AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                });

            services.AddDbContext<Data.ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));


            services.AddMvc(options => {
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            })
                .AddViewLocalization(
                    opts => { opts.ResourcesPath = "Resources"; })
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.Configure<IdentityOptions>(options=> {
                options.Password.RequiredLength = 10;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireDigit = true;
            });

            services.Configure<RequestLocalizationOptions>( opts => {
                var supportedCultures = new List<CultureInfo> {
                    new CultureInfo("en"),
                    new CultureInfo("en-US"),
                    new CultureInfo("ru"),
                    new CultureInfo("ja"),
                    new CultureInfo("fr"),
                    new CultureInfo("fr-FR"),
                    new CultureInfo("sl-SI"),
                    new CultureInfo("zh-CN"),
                    new CultureInfo("ar-EG"),
                    
                };

                opts.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("sl-SI");
                //Formatting numbers, dates, etc.
                opts.SupportedCultures = supportedCultures;
                //UI strings that we have localized.
                opts.SupportedUICultures = supportedCultures;

                
            });
            
            
            
            services.AddRazorPages();
            services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));

            services.Configure<ReCAPTCHASettings>(Configuration.GetSection("GooglereCAPTCHA"));
            services.AddTransient<GooglereCaptchaService>();

            services.AddSingleton<IConfiguration>(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Data.ApplicationDbContext context, UserManager<ApplicationUser> userManager,
RoleManager<ApplicationRole> roleManager)
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
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);


            app.UseRouting();
            app.UseAuthentication();
            
            app.UseAuthorization();

            DbInitializer.Initialize(context, userManager, roleManager);
            StripeConfiguration.ApiKey = Configuration.GetSection("Stripe")["SecretKey"];

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
