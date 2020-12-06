using AutoMapper;
using Project.Iservice;
using Project.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTemple.Application
{
    public class ProjectTempleProfile : Profile
    {
        /// <summary>
        /// 统一注入ProjectTemple项目的service
        /// </summary>
        public ProjectTempleProfile()
        {
            CreateMap<IUserService, UserService>();
        }

    }
}
