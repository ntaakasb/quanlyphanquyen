using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Base.Common;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Services;

namespace WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            CurrentEnvironment = environment;
        }

        public IConfiguration Configuration { get; }

        public IHostingEnvironment CurrentEnvironment { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ReadAppSettings(services);
            RegisterAppConfiguration(services);
            AddCookieAuthentication(services);
            RegisterServices(services);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);


            services.Configure<FormOptions>(options =>
            {
                options.ValueCountLimit = int.MaxValue;
            });
        }

        public void ReadAppSettings(IServiceCollection services)
        {
            services.Configure<ApplicationSetting>(Configuration.GetSection("AppSettings"));
        }

        public void RegisterAppConfiguration(IServiceCollection services)
        {
            if (CurrentEnvironment.IsProduction())
            {
                services.AddHttpsRedirection(options =>
                {
                    options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
                    options.HttpsPort = 443;
                });
            }

            services.AddHsts(options =>
            {
                options.MaxAge = TimeSpan.FromDays(365);
            });

            services.Configure<FormOptions>(options =>
            {
                options.ValueLengthLimit = int.MaxValue;
                options.MultipartBodyLengthLimit = long.MaxValue;
            });

            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes = new[] { "text/plain" };
            });

            services.AddResponseCaching();
        }

        public void AddCookieAuthentication(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.Secure = CurrentEnvironment.IsDevelopment() ? CookieSecurePolicy.None : CookieSecurePolicy.Always;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.ForwardAuthenticate = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.ForwardDefault = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.ForwardChallenge = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.Cookie.Name = CurrentEnvironment.EnvironmentName;
                    options.LoginPath = new PathString("/Account/Login");
                    options.ReturnUrlParameter = "returnUrl";
                    options.AccessDeniedPath = new PathString("/Error/403");
                });

            services.AddAntiforgery(options =>
            {
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.None;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //app.UseHttpsRedirection();
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseStaticFiles(new StaticFileOptions()
            {
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.CacheControl] = "private";
                },
                ServeUnknownFileTypes = true
            });

            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseResponseCompression();
            app.UseResponseCaching();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

            });
        }

        private void RegisterServices(IServiceCollection services)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAuthenticateServices, AuthenticateServices>();
            services.AddScoped<IUserService, UserService>();
        }
    }
}
