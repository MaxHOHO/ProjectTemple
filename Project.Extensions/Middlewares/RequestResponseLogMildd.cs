using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Project.Common.Helper;
using Project.Common.HttpContextHelper;
using Project.Common.HttpWebHelper;
using Project.Model.CommonModels;
using Project.Model.CommonModels.API;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Project.Extensions.Middlewares
{
    /// <summary>
    /// 中间件
    /// 记录请求和响应数据
    /// </summary>
    public class RequestResponseLogMildd
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLogMildd> _logger;
        private Stopwatch _stopwatch;
        private readonly IHttpClientFactory _httpClientFactory;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        public RequestResponseLogMildd(
            RequestDelegate next, 
            ILogger<RequestResponseLogMildd> logger,
            IHttpClientFactory httpClientFactory
            )
        {
            _next = next;
            _logger = logger;
            _stopwatch = new Stopwatch();
            _httpClientFactory = httpClientFactory;
        }



        public async Task InvokeAsync(HttpContext context)
        {
            if (Appsettings.app("Middleware", "RequestResponseLog", "Enabled").ObjToBool())
            {
                // 过滤，只有接口
                if (context.Request.Path.Value.ToLower().Contains("api"))
                {
                    _stopwatch.Restart();
                    HttpRequest request = context.Request;
                    RequestResponseLogAPIModel requestResponseLogAPIModel = new RequestResponseLogAPIModel();

                    requestResponseLogAPIModel.UserID = "";//用户ID待后续增加用户再增加
                    requestResponseLogAPIModel.IP = HttpContextHelper.GetClientIP(context);  //获取IP
                    requestResponseLogAPIModel.Agent = request.Headers["User-Agent"].ObjToString();  //获取Agent浏览器信息
                    requestResponseLogAPIModel.RequestMethod = request.Method;  //获取请求类型
                    requestResponseLogAPIModel.API = request.Host.Value.ObjToString() + request.Path.Value.ObjToString();
                    requestResponseLogAPIModel.BeginTime = DateTime.Now;  //请求开始时间

                    //获取请求body内容
                    if (request.Method.ToLower().Equals("post") || request.Method.ToLower().Equals("put"))
                    {
                        // 启用倒带功能，就可以让 Request.Body 可以再次读取
                        request.EnableBuffering();
                        Stream stream = request.Body;
                        byte[] buffer = new byte[request.ContentLength.Value];
                        stream.Read(buffer, 0, buffer.Length);
                        requestResponseLogAPIModel.RequestData = Encoding.UTF8.GetString(buffer);  //获取请求信息data
                    }
                    else if (request.Method.ToLower().Equals("get") || request.Method.ToLower().Equals("delete"))
                    {
                        //获取请求信息data
                        requestResponseLogAPIModel.RequestData = HttpUtility.UrlDecode(request.QueryString.ObjToString(), Encoding.UTF8);
                    }

                    // 获取Response.Body内容
                    var originalBodyStream = context.Response.Body;
                    using (var responseBody = new MemoryStream())
                    {
                        context.Response.Body = responseBody;

                        await _next(context);

                        var responseBodyData = await GetResponse(context.Response);
                        requestResponseLogAPIModel.ResponseData = responseBodyData;  //获取返回信息data
                        await responseBody.CopyToAsync(originalBodyStream);
                    }

                    // 响应完成记录时间和存入日志
                    context.Response.OnCompleted(() =>
                    {
                        _stopwatch.Stop();
                        requestResponseLogAPIModel.EndTime = DateTime.Now;  // 响应结束时间
                        requestResponseLogAPIModel.OPTime = _stopwatch.ElapsedMilliseconds.ObjToString() ;  //请求响应时间毫秒

                        //// 自定义log输出
                        //var requestInfo = JsonConvert.SerializeObject(userAccessModel);
                        //Parallel.For(0, 1, e =>
                        //{
                        //    LogLock.OutSql2Log("RecordAccessLogs", new string[] { requestInfo + "," }, false);
                        //});

                        return Task.CompletedTask;
                    });

                    //将请求响应信息推送至rabbitMQ中，让Receive程序进行记录
                    //await _eventBusRabbitMQ.PublishAsync(Appsettings.app("RabbitMQ", "QueueName", "RequestResponseLogMildd"), requestResponseLogModel);
                    HttpWebClient httpWebClient = new HttpWebClient(_httpClientFactory);
                    requestResponseLogAPIModel.MQQueueName = Appsettings.app("RabbitMQ", "QueueName", "RequestResponseLogMildd");
                    await httpWebClient.PostAsync(Appsettings.app("API", "Url", "HostUrl") + Appsettings.app("API", "MQ", "RabbitMQPublishUrl"),
                        JsonConvert.SerializeObject(requestResponseLogAPIModel));

                }
                else
                {
                    await _next(context);
                }
            }
            else
            {
                await _next(context);
            }
        }

        /// <summary>
        /// 获取响应内容
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public async Task<string> GetResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            return text;
        }

    }
}
