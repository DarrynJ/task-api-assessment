using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tasks.Domain.Common.Interfaces;
using Tasks.Domain.Entities;
using Tasks.Domain.Repositories;
using Tasks.Domain.Repositories.Users;
using Tasks.Infrastructure.Persistence;

namespace Tasks.Infrastructure.Repositories.Users
{
    public class UserRepository : RepositoryBase<User, User, ApplicationDbContext>, IUserRepository
    {
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
