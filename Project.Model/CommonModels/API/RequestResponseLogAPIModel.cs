using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Model.CommonModels.API
{
    public class RequestResponseLogAPIModel
    {
        public int ID { get; set; }

        public string MQQueueName { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        /// 请求IP
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// 请求Agent 浏览器
        /// </summary>
        public string Agent { get; set; }
        /// <summary>
        /// 请求的方法
        /// </summary>
        public string RequestMethod { get; set; }
        /// <summary>
        /// 请求的API地址
        /// </summary>
        public string API { get; set; }
        /// <summary>
        /// 请求的Data JSON
        /// </summary>
        public string RequestData { get; set; }
        /// <summary>
        /// 返回的Data Json
        /// </summary>
        public string ResponseData { get; set; }
        /// <summary>
        /// 响应时间 毫秒
        /// </summary>
        public string OPTime { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime BeginTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
    }
}
