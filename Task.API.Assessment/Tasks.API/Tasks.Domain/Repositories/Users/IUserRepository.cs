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
        
    }
}
