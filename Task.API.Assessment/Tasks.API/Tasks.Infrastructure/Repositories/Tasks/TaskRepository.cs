using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tasks.Domain.Common.Interfaces;
using Tasks.Domain.Repositories;
using Tasks.Domain.Repositories.Tasks;
using Tasks.Infrastructure.Persistence;

namespace Tasks.Infrastructure.Repositories.Tasks
{
    public class TaskRepository : RepositoryBase<Domain.Entities.Task, Domain.Entities.Task, ApplicationDbContext>, ITaskRepository
    {
        public TaskRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
