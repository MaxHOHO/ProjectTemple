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
            //����ע��Appsettings
            services.AddSingleton(new Appsettings(Configuration));
            //Serviceʹ��automapͳһע��
            services.AddAutoMapper(typeof(ProjectTempleProfile));
            //�˴�����automap����ͳһע��
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

                //ʹ���Ǳ���
                x.UseDashboard();

                //ʹ��RabbitMQ
                x.UseRabbitMQ(rb =>
                {
                    //RabbitMQ������Ϣ
                    rb.HostName = Appsettings.app("Startup", "RabbitMQ", "HostName");
                    rb.ExchangeName = Appsettings.app("Startup", "RabbitMQ", "ExchangeName");
                    rb.UserName = Appsettings.app("Startup", "RabbitMQ", "UserName");
                    rb.Password = Appsettings.app("Startup", "RabbitMQ", "Password");
                });

                //���ô���ɹ������������ݿ��б����ʱ��s�����ݶ�������
                x.SucceedMessageExpiredAfter = 24 * 3600;
                //ʧ�����Դ���
                x.FailedRetryCount = 5;
                //ʧ�����Եļ��ʱ��S
                x.FailedRetryInterval = 60;

            });
            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
              


            //��¼����ͷ�������
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
