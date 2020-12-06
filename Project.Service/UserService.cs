using Project.Data.Context;
using Project.Iservice;
using Project.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Project.Service
{
    public class UserService : IUserService
    {
        private readonly MyContext _myContext;

        public UserService(MyContext myContext)
        {
            _myContext = myContext;
        }


        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<int> Add(User user)
        {
            await _myContext.User.AddAsync(user);
            return await _myContext.SaveChangesAsync();
        }


        /// <summary>
        /// 获取所有用户信息
        /// </summary>
        /// <returns></returns>
        public async Task<List<User>> GetUser()
        {
            return await _myContext.User.ToListAsync();


        }
    }
}
