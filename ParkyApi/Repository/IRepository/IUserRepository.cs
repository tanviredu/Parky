using ParkyApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyApi.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUniqeUser(string userName);
        User Authenticate(string userName, string password);
        User Register(string userName, string password);
    }
}
