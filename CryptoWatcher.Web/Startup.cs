using CryptoWatcher.Web.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CryptoWatcher.Web
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // CORS
            services.ConfigureCors();

            // Automapper
            services.ConfigureAutomapper();

            // DI
            services.ConfigureDependencies(Configuration);

            // Hangfire
            services.ConfigureHangfire(Configuration);

            // Elmah
            services.ConfigureElmah(Configuration);

            // Mvc
            services.ConfigureMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerfactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();

            // Middlewares
            app.ConfigureMiddlewares();

            // Log4Net
            loggerfactory.ConfigureLog4Net(env);

            // Data seeding
            app.ConfigureDataSeeding();

            // Hangfire
            app.ConfigureHangfire(Configuration);

            // Mvc
            app.ConfigureMvc();
        }
    }
}
