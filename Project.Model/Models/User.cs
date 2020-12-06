using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Model.Models
{
    public class User
    {
        public int ID { get; set; }

        public string Name { get; set; }
        public string PassWord { get; set; }

        public decimal Age { get; set; }
        
        //并发标记，乐观并发控制
        public byte[] Timestamp { get; set; }
    }
}
