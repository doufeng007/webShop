using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Castle.Facilities.Logging;
using Swashbuckle.AspNetCore.Swagger;
using Abp.AspNetCore;
using Abp.Castle.Logging.Log4Net;
using Abp.Extensions;
using ZCYX.FRMSCore.Authentication.JwtBearer;
using ZCYX.FRMSCore.Configuration;
using ZCYX.FRMSCore.Identity;
//using Abp.SignalR.Core;
using Microsoft.Extensions.Hosting;
using Abp.SignalR.Core.Hubs;
using Abp.Hangfire;
using Hangfire;
using System.Collections.Generic;
using Abp.Modules;
using Microsoft.AspNetCore.Identity;
using System.Reflection;
using Microsoft.AspNetCore.Http.Features;
using SkyWalking.AspNetCore;
using ZCYX.FRMSCore.Extensions;
using Swashbuckle.AspNetCore.SwaggerUI;

//using Abp.AspNetCore.SignalR.Hubs;



namespace ZCYX.FRMSCore.Web.Host.Startup
{
    public class Startup
    { 
        private const string _defaultCorsPolicyName = "localhost";

        private readonly IConfigurationRoot _appConfiguration;
        private readonly IConfiguration _configuration;
        public Startup(Microsoft.AspNetCore.Hosting.IHostingEnvironment env, IConfiguration configuration)
        {
            _appConfiguration = env.GetAppConfiguration();
            _configuration = configuration;
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // MVC
            services.AddMvc(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory(_defaultCorsPolicyName));
                options.Filters.Add(typeof(ActionFilter));
            });
       

            services.AddScoped<Abp.WorkFlow.WorkFlowRunFilterAttribute>();
            services.AddScoped<Abp.WorkFlow.WorkFlowCommitFilterAttribute>();
            
            //services.AddScoped<Abp.WorkFlow.WorkFlowBusinessListAttribute>();

            IdentityRegistrar.Register(services);
            AuthConfigurer.Configure(services, _appConfiguration);


            //解决Multipart body length limit 134217728 exceeded
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue; // In case of multipart
            });
            //接入业务接口性能监控
            if ((_configuration["Monitor:SkyWalking:Open"] ?? "0") == "1")
            {
                services.AddSkyWalking(option =>
                {
                    option.ApplicationCode = _configuration["Monitor:SkyWalking:ApplicationCode"] ?? "FRMSCore";
                    option.DirectServers = _configuration["Monitor:SkyWalking:DirectServers"] ?? "127.0.0.1:11800";
                });
            }
            // Configure CORS for angular2 UI
            services.AddCors(options =>
            {
                options.AddPolicy(_defaultCorsPolicyName, builder =>
                {
                    // App:CorsOrigins in appsettings.json can contain more than one address separated by comma.
                    builder
                        .WithOrigins(_appConfiguration["App:CorsOrigins"].Split(",", StringSplitOptions.RemoveEmptyEntries)
                                                                         .Select(o => o.RemovePostFix("/"))
                                                                         .ToArray())
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
            services.Configure<IdentityOptions>(options => {
                options.Password.RequireDigit = _appConfiguration.GetValue<bool>("Password:RequireDigit");
                options.Password.RequiredLength = _appConfiguration.GetValue<int>("Password:RequiredLength");
                options.Password.RequireNonAlphanumeric = _appConfiguration.GetValue<bool>("Password:RequireNonAlphanumeric");
                options.Password.RequireUppercase = _appConfiguration.GetValue<bool>("Password:RequireUppercase");
                options.Password.RequireLowercase = _appConfiguration.GetValue<bool>("Password:RequireLowercase");
            });
            // Swagger - Enable this line and the related lines in Configure method to enable swagger UI
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info { Title = "FRMSCore API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);

                // Define the BearerAuth scheme that's in use
                options.AddSecurityDefinition("bearerAuth", new ApiKeyScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                // Assign scope requirements to operations based on AuthorizeAttribute
                options.OperationFilter<SecurityRequirementsOperationFilter>();
                var commentsFiles = GetXmlCommentsPath();
                foreach (var item in commentsFiles)
                {
                    options.IncludeXmlComments(item);
                }
            });

            services.AddSignalR();
            //services.AddSingleton<IHostedService, NoticeHostedService>();
            //services.AddSingleton<IServiceProvider, ServiceProvider>();


            services.AddHangfire(config =>
            {
                config.UseSqlServerStorage(_appConfiguration.GetConnectionString("Default"));
            });


            // Configure Abp and Dependency Injection
            return services.AddAbp<FRMSCoreWebHostModule>(options =>
            {
                // Configure Log4Net logging
                options.IocManager.IocContainer.AddFacility<LoggingFacility>(
                    f => f.UseAbpLog4Net().WithConfig("log4net.config")
                );
            });



        }

        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseAbp(options => { options.UseAbpRequestLocalization = false; }); // Initializes ABP framework.

            app.UseCors(_defaultCorsPolicyName); // Enable CORS!

            //接入日志异常监控
            if ((_configuration["Monitor:Exceptionless:Open"] ?? "0") == "1")
            {
                app.AddExceptionlog4Net("log4net.config", _configuration);
            }


            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseJwtTokenMiddleware();

           
            app.UseAbpRequestLocalization();



            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "defaultWithArea",
                    template: "{area}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {

               // options.InjectOnCompleteJavaScript("/swagger/ui/abp.js");
                //options.InjectOnCompleteJavaScript("/swagger/ui/on-complete.js");
              //  options.InjectOnCompleteJavaScript("/src/SwaggerLogin.js");
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "FRMSCore API V1");
                options.DocExpansion(DocExpansion.None);

                options.IndexStream = () => Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream("ZCYX.FRMSCore.Web.Host.wwwroot.swagger.ui.index.html");

            }); // URL: /swagger


            app.UseSignalR(routes =>
            {
                //routes.MapHub<Abp.AspNetCore.SignalR.Hubs.AbpCommonHub>("/signalr");
                routes.MapHub<NoticeHub>("/signalr");
            });

            app.UseHangfireServer();

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new HangfireCustomAuthorizeFilter() }
            });


        }


        private List<string> GetXmlCommentsPath()
        {
            var docs = new List<string>();
            var modules = AbpModule.FindDependedModuleTypesRecursivelyIncludingGivenModule(typeof(FRMSCoreWebHostModule));
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            foreach (var module in modules)
            {
                var commentsFileName = module.Assembly.GetName().Name + ".XML";
                var commentsFile = System.IO.Path.Combine(baseDirectory, commentsFileName);
                if (System.IO.File.Exists(commentsFile))
                    docs.Add(commentsFile);
            }

            return docs;
        }



    }


}
