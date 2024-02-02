using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasks.Domain.Entities;

namespace Tasks.Domain.Repositories.Users
{
    public interface IUserRepository : IEFRepository<User, User>
    {
        Task<User> CreateUser(string username, string emailAddress, string password);
        Task<User> GetUser(Guid id);
        Task<User> UpdateUser(Guid id, User user);
        Task<bool> DeleteUser(Guid id);
    }
}
