using Microsoft.EntityFrameworkCore;
using Project.Data.Mapping;
using Project.Model.CommonModels;
using Project.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Data.Context
{
    public class MyContext : DbContext
    {
        //配置迁移
        //Add-Migration InitialCreate  
        //列出迁移
        //Get-Migration
        //迁移
        //Update-Database

        public MyContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<RequestResponseLog> RequestResponseLog { get; set; }

        /// <summary>
        /// 进行实体类属性配置，采用Fluent API配置模型，未使用数据注释实现
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region 此方法迁移至单独类中,如下调用
            //为减少OnModelCreating方法的大小，将实体类的配置信息提取到单独的类中
            //modelBuilder.Entity<User>()
            //    .Property(c => c.ID)
            //    .IsRequired();
            #endregion

            //配置User表
            new UserMap().Configure(modelBuilder.Entity<User>());

            //配置api请求响应表
             

        }

    }
}
