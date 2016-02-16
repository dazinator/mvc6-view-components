using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNet.Mvc.Razor;
using Microsoft.AspNet.FileProviders;
using System.Reflection;
using Microsoft.AspNet.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;

namespace Web
{
    public class Startup
    {

        public static Assembly[] AdditionalAssemblies;

        private string _basePath;

        public Startup(IHostingEnvironment env)
        {
            _basePath = env.WebRootPath;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<DefaultAssemblyProvider, DefaultAssemblyProvider>();
            services.AddTransient<IAssemblyProvider, AssemblyProvider>();

            

            // Controller assembly.
            string assemblyFilePath = System.IO.Path.Combine(_basePath, "..\\..\\artifacts\\bin\\BookStore.Portal\\Debug\\net451\\BookStore.Portal.dll");
            // Assembly assembly = Assembly.LoadFile(assemblyFilePath);

            // VC Assembly.
            string componentAssemblyPath = System.IO.Path.Combine(_basePath, "..\\..\\artifacts\\bin\\BookStore.Components\\Debug\\net451\\BookStore.Components.dll");
            // Assembly componentAssembly = Assembly.LoadFile(componentAssemblyPath);

            AdditionalAssemblies = new Assembly[] { this.GetType().Assembly, Assembly.LoadFile(assemblyFilePath), Assembly.LoadFile(componentAssemblyPath) };
            
            services.AddMvc().AddControllersAsServices(AdditionalAssemblies);

            services.Configure<RazorViewEngineOptions>(options =>
                {
                    options.FileProvider = new CompositeFileProvider(
                        new EmbeddedFileProvider(
                            AdditionalAssemblies[1],
                            "BookStore.Portal"
                        ),
                        new EmbeddedFileProvider(
                            AdditionalAssemblies[2],
                            "BookStore.Components"
                        ),
                        options.FileProvider
                    );
                });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IHostingEnvironment env)
        {
            loggerFactory.AddDebug();
            //      loggerFactory.AddSerilog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseDatabaseErrorPage();
            }

            app.UseMvc(config =>
            {
                config.MapRoute(
                    name: "Default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" }
                );
            });
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
