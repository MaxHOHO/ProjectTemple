using Project.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Project.Iservice
{
    public interface IUserService 
    {
        Task<List<User>> GetUser();

        Task<int> Add(User user);
    }
}
