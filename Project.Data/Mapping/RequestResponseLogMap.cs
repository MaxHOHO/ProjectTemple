using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Model.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.Mapping
{
    public class RequestResponseLogMap : IEntityTypeConfiguration<RequestResponseLog>
    {
        public void Configure(EntityTypeBuilder<RequestResponseLog> builder)
        {
            //设置ID主键
            builder
                .HasKey(c => c.ID)
                .HasName("PrimaryKey_ID");

            //ID 不可空，自增长
            builder
                .Property(c => c.ID)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder
                .Property(c => c.UserID);
        }
    }
}
