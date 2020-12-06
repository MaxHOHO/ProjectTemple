using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Project.Data.Context;
using Project.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTemple.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly MyContext _myContext;

        public WeatherForecastController(
            ILogger<WeatherForecastController> logger,
            MyContext myContext)
        {
            _logger = logger;
            _myContext = myContext;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> GetAsync()
        {
            var rng = new Random();

            //加载所有数据
            var user1 = _myContext.User.ToList();

            //筛选数据
            var user2 = _myContext.User.Where(c => c.Name.Contains("11")).ToList();

            var user3 = new User();
            user3.Name = "01";
            user3.Age = 10;
            user3.PassWord = "Aa222222";

            //添加数据
            _myContext.User.Add(user3);
            //await _myContext.SaveChangesAsync();

            var user31 = new User();
            user31.Name = "02";
            user31.Age = 11;
            _myContext.User.Add(user31);
            await _myContext.SaveChangesAsync();

            //更新数据
            user3.Age = 12;
            await _myContext.SaveChangesAsync();

            //删除数据
            var user4 = await _myContext.User.FirstAsync();
            _myContext.User.Remove(user4);
            await _myContext.SaveChangesAsync();

            //原生sql查询
            var user5 = _myContext.User.FromSqlRaw("select * from [dbo].[User]").ToList();
            //原生执行存储过程
            //user5 = _myContext.User.FromSqlRaw("EXECUTE userProcudure").ToList();

            //占位符查询  bug待修复
            //StringBuilder stringBuilder = new StringBuilder();
            //stringBuilder.Append("select * from [dbo].[User] where name=@name and age=@age");
            //List<SqlParameter> sqlParameters = new List<SqlParameter>();
            //sqlParameters.Add(new SqlParameter("name", "01"));
            //sqlParameters.Add(new SqlParameter("age", "10"));
            //var user6 = _myContext.User.
            //    FromSqlRaw(stringBuilder.ToString(), sqlParameters.ToList()).ToList();



            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
