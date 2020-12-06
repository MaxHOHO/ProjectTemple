using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Common.Helper;
using Project.Iservice;
using Project.Model;
using Project.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ProjectTemple.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;


        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// 获取总用户列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<PageModel<User>>> Get(int page,int PageSize)
        {
            //var data = new MessageModel<List<User>>();

            var data = await _userService.GetUser();


            return new MessageModel<PageModel<User>>()
            {
                msg = "获取成功",
                success = true,
                response = new PageModel<User>()
                {
                    page = page,
                    dataCount = data.Count(),
                    data = data,
                    pageCount = data.Count()/PageSize
                }
            };
        }

        

        /// <summary>
        /// 添加一个用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel<string>> Post(User user)
        {
            var data = new MessageModel<string>();
            var id = (await _userService.Add(user));
            data.success = id > 0;
            if(data.success)
            {
                data.response = id.ObjToString();
                data.msg = "添加成功";
            }
            return data;

        }



    }
}
