using Democms2.Models.VisitorGroups;
using Democms2.Services;
using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.Cms.UI.VisitorGroups;
using EPiServer.DependencyInjection;
using EPiServer.Framework.Web;
using EPiServer.Scheduler;
using EPiServer.Web.Routing;
using Optimizely.Cms.DependencyInjection;


namespace Democms2
{
    public class Startup(IWebHostEnvironment webHostingEnvironment)
    {
        public void ConfigureServices(IServiceCollection services)
        {
            if (webHostingEnvironment.IsDevelopment())
            {
                AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(webHostingEnvironment.ContentRootPath, "App_Data"));

                services.Configure<SchedulerOptions>(options => options.Enabled = false);
            }

            services
                .AddCmsAspNetIdentity<ApplicationUser>()
                .AddCms()
                .AddVisitorGroupsFrameworkWeb()
                .AddVisitorGroupsCmsCoreWeb()
                .AddVisitorGroupsTemplating()
                .AddVisitorGroupsCriterion<AuthenticatedUserRoleCriterion>()
                .AddVisitorGroupsUI()
                .AddAdminUserRegistration()
                .AddForms()
                .AddGraphContentClient()
                .AddEmbeddedLocalization<Startup>();

            services.AddScoped<INavService, NavigationService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapContent();
            });
        }
    }
}
