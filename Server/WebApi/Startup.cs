using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Model.BaseModels.Configuration;
using WebApi.Extensions;
using WebApi.Filters;
using WebApi.Hubs;
using WebApi.Middleware;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            //AppSetting�]�w�ন�j��O , �����s��WebApiAppConfig�Y�i�ϥ�          
            configuration.GetSection("ConnectionStrings").Bind(new ConnectionStrings());
            configuration.GetSection("LoginSettings").Bind(new LoginSettings());

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                x.IncludeXmlComments(xmlPath);
            });

            // IIS �i�i��P�B (���MDynamic�|�ϥΫD�P�BŪ��):
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            //���UFilter
            //services.AddAutoMapper();
            services.AddMvc(config =>
            {
                config.Filters.Add(new AuthorizationFilter());
            });

            //�N.Net Core �w�]�ϥΪ��֤�System.Text.Json�󴫬�NewtonsoftJson
            //���ӵ�.Net Core�o�@�ӮM�󧹦��׸�����A���^��
            services.AddMvc().AddNewtonsoftJson();

            //�`�J�ۭq�A��
            services.AddScopedServices();
            services.AddSingletonServices();
            services.AddTransientServices();

            services.AddHttpClient();

            services.AddSignalR();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            

            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                //c.RoutePrefix = string.Empty;
            });

            

            //app.UseHttpsRedirection();
            //CORS
            //app.UseCors(policy =>
            //{
            //    policy.AllowAnyHeader();
            //    policy.AllowAnyMethod();
            //    policy.AllowCredentials();
            //    //policy.WithOrigins("http://localhost");
            //    policy.WithOrigins("https://localhost:44314");
            //    policy.WithOrigins("https://localhost:51271");
            //    policy.WithOrigins("https://localhost:8080");
            //    policy.WithOrigins("http://localhost:800");
            //    policy.WithOrigins("http://122.116.211.180:800");
            //    //policy.AllowAnyOrigin();

            //    //
            //});

            app.UseRouting();

            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials()); // allow credentials

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatHub>("/chathub");
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                name: "Default",
                pattern: "{controller=default}/{action=Index}/{id?}");
            });


        }
    }
}
