using DotNetCore.CAP;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Common.Helper;
using Project.Model.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTemple.API.Controllers.MQ
{
    [Route("api/[controller]")]
    [ApiController]
    public class RabbitMQReceiveController : ControllerBase
    {
        //string RequestResponseLogQueue = Appsettings.app("RabbitMQ", "QueueName", "RequestResponseLogQueue").ObjToString();

        /// <summary>
        /// receive RequestResponseLogQueue
        /// </summary>
        /// <param name="requestResponseLogModel"></param>
        [NonAction]
        [CapSubscribe("RequestResponseLogQueue")]
        public void ReceiveRequestResponseLog(RequestResponseLog requestResponseLog)
        {

        }

    }
}
