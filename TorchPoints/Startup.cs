using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using FluentScheduler;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TorchPoints.Core.DataAccess;
using TorchPoints.Service;
using TorchPoints.Service.TaskSchedule;
using static TorchPoints.Service.TaskSchedule.MyRegistry;

namespace TorchPoints
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
            //����sqlserver
            services.AddDapper("SqlServer", m =>
            {
                m.ConnectionString = Configuration.GetConnectionString("DefaultConnection"); 
                m.DbType = DbStoreType.SqlServer;
            });

            services.AddControllers();

            services.AddSingleton<IPointService, PointService>();
            services.AddSingleton<ISetting, SettingService>();
            services.AddSingleton<IConsumeHistoryService, ConsumeHistoryService>();
            services.AddHostedService<SchedulerHostedService>();
            services.AddTransient<MyRegistry>();
            services.AddTransient<Job>();
            #region swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "My API", Version = "v1" });

                var basePath = AppContext.BaseDirectory;

                var xmlPath = Path.Combine(basePath, "TorchPoints.xml");
                c.IncludeXmlComments(xmlPath, true); //��ӿ�������ע�ͣ�true��ʾ��ʾ������ע�ͣ�
            });
            #endregion

         
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            #region swagger 
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = "";//·�����ã�����Ϊ�գ���ʾֱ�ӷ��ʸ��ļ�����ʾֱ���ڸ�������localhost:8001�����ʸ��ļ�
                //���ʱ��ȥlaunchSettings.json�а�"launchUrl": "swagger/index.html"ȥ���� Ȼ��ֱ�ӷ���localhost:8001/index.html����    
            });
            #endregion
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

          
        }

        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            //containerBuilder.RegisterModule<ConfigureAutofac>();

        }
    }
}
