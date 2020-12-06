using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Data.Mapping
{
    /// <summary>
    /// 减少Context中OnModelCreating方法的大小，将对应的实体类配置迁移至单独类中
    /// </summary>
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            //自定义表名称
            //haskey设置ID为主键
            //hasname设置主键名称
            builder
                .ToTable("User")
                .HasKey(c => c.ID)
                .HasName("PrimaryKey_ID");

            //指定单列索引
            //指定索引名称
            //指定索引唯一
            builder
                .HasIndex(c => c.Name)
                .HasDatabaseName("Index_Name");
            //.IsUnique();

            //指定多列索引
            builder
                .HasIndex(c => new { c.Name, c.Age });
                //.IsUnique();

            #region 组合键
            //设置组合键，无法使用数据注释来配置
            //builder.HasKey(c => new { c.ID, c.Name });
            #endregion
            #region 备用键
            //备用键唯一且只读，后续研究
            #endregion

            //将可选属性配置为必须
            //并且不区分大小写排序
            //自增长
            builder
                .Property(c => c.ID)
                .IsRequired()
                //.UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .ValueGeneratedOnAdd();

            builder.Property(c => c.Name)
                .HasMaxLength(100)
                .IsRequired()
                .HasDefaultValue("");

            //定义列名称
            //定义列类型
            builder.Property(c => c.Age)
                .HasColumnName("Age")
                .HasColumnType("decimal(12,2)");

            //两种方式解决并发
            builder
                .Property(c => c.Timestamp)
                .IsRowVersion()
                .IsConcurrencyToken();

            //配置种子数据
            builder.HasData(
                new User
                {
                    ID = 1,
                    Name = "test",
                    Age = 10,
                    PassWord = "Aa222222"
                });

        }
    }
}
