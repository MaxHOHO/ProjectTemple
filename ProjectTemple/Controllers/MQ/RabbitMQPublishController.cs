using DotNetCore.CAP;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Model;
using Project.Model.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTemple.API.Controllers.MQ
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RabbitMQPublishController : ControllerBase
    {

        public readonly ICapPublisher _capPublisher;

        public RabbitMQPublishController(ICapPublisher capPublisher)
        {
            _capPublisher = capPublisher;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel<string>> RequestResponseLog(string RabbitMQQueue,RequestResponseLog requestResponseLog)
        {
            var data = new MessageModel<string>();
            await _capPublisher.PublishAsync(RabbitMQQueue, requestResponseLog);
            
            data.response = "";
            data.msg = "推送成功";
            return data;

        }
    }
}
