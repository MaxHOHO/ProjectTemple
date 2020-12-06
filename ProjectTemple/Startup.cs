using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Project.Common.Helper;
using Project.Data.Context;
using Project.Extensions.Middlewares;
using Project.Iservice;
using Project.Service;
using ProjectTemple.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTemple
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

            services.AddControllers();

            services.AddHttpContextAccessor();
            //单独注册Appsettings
            services.AddSingleton(new Appsettings(Configuration));
            //Service使用automap统一注册
            services.AddAutoMapper(typeof(ProjectTempleProfile));
            //此处采用automap进行统一注入
            //services.AddScoped<IUserService, UserService>();

            #region Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ProjectTemple", Version = "v1" });
            });
            #endregion

            #region Sqlserver
            services.AddDbContext<MyContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            #endregion

            #region  CAP+RabbitMQ
            services.AddCap(x =>
            {
                //EF
                x.UseEntityFramework<MyContext>();

                //使用仪表盘
                x.UseDashboard();

                //使用RabbitMQ
                x.UseRabbitMQ(rb =>
                {
                    //RabbitMQ基础信息
                    rb.HostName = Appsettings.app("Startup", "RabbitMQ", "HostName");
                    rb.ExchangeName = Appsettings.app("Startup", "RabbitMQ", "ExchangeName");
                    rb.UserName = Appsettings.app("Startup", "RabbitMQ", "UserName");
                    rb.Password = Appsettings.app("Startup", "RabbitMQ", "Password");
                });

                //设置处理成功的数据在数据库中保存的时间s，数据定期清理
                x.SucceedMessageExpiredAfter = 24 * 3600;
                //失败重试次数
                x.FailedRetryCount = 5;
                //失败重试的间隔时间S
                x.FailedRetryInterval = 60;

            });
            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
              


            //记录请求和返回数据
            app.UseRequestResponseLog();

            

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProjectTemple v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
