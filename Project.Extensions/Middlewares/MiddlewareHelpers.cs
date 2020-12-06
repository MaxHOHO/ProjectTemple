using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Extensions.Middlewares
{
    public static class MiddlewareHelpers
    {

        /// <summary>
        /// 请求响应中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseRequestResponseLog(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RequestResponseLogMildd>();
        }

        /// <summary>
        /// 异常处理中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        //public static IApplicationBuilder UseExceptionHandlerMidd(this IApplicationBuilder app)
        //{
        //    return app.UseMiddleware<ExceptionHandlerMidd>();
        //}

    }
}
