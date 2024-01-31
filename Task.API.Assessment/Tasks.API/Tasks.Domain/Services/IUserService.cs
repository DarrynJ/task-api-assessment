using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks.Domain.Services
{
    public interface IUserService
    {
        Task<string> LoginUser(string username, string password);
    }
}
