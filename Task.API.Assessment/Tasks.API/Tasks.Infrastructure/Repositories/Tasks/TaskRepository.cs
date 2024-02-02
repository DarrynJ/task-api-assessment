using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tasks.Domain.Common.Interfaces;
using Tasks.Domain.Repositories;
using Tasks.Domain.Repositories.Tasks;

namespace Tasks.Infrastructure.Repositories.Tasks
{
    public class TaskRepository : ITaskRepository
    {
        IUnitOfWork IEFRepository<Task, Task>.UnitOfWork => throw new NotImplementedException();

        public Task<Task> CreateTask(string title, string description, Guid assignee, DateTime dueDate)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteTask(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Task>> GetAllTasks()
        {
            throw new NotImplementedException();
        }

        public Task<Task> GetTask(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Task> UpdateTask(Guid id, Task task)
        {
            throw new NotImplementedException();
        }

        void IRepository<Task>.Add(Task entity)
        {
            throw new NotImplementedException();
        }

        Task<bool> IEFRepository<Task, Task>.AnyAsync(Expression<Func<Task, bool>> filterExpression, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<bool> IEFRepository<Task, Task>.AnyAsync(Func<IQueryable<Task>, IQueryable<Task>>? queryOptions, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<int> IEFRepository<Task, Task>.CountAsync(Expression<Func<Task, bool>> filterExpression, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<int> IEFRepository<Task, Task>.CountAsync(Func<IQueryable<Task>, IQueryable<Task>>? queryOptions, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<List<Task>> IEFRepository<Task, Task>.FindAllAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<List<Task>> IEFRepository<Task, Task>.FindAllAsync(Expression<Func<Task, bool>> filterExpression, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<List<Task>> IEFRepository<Task, Task>.FindAllAsync(Expression<Func<Task, bool>> filterExpression, Func<IQueryable<Task>, IQueryable<Task>> queryOptions, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<IPagedResult<Task>> IEFRepository<Task, Task>.FindAllAsync(int pageNo, int pageSize, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<IPagedResult<Task>> IEFRepository<Task, Task>.FindAllAsync(Expression<Func<Task, bool>> filterExpression, int pageNo, int pageSize, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<IPagedResult<Task>> IEFRepository<Task, Task>.FindAllAsync(Expression<Func<Task, bool>> filterExpression, int pageNo, int pageSize, Func<IQueryable<Task>, IQueryable<Task>> queryOptions, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<List<Task>> IEFRepository<Task, Task>.FindAllAsync(Func<IQueryable<Task>, IQueryable<Task>> queryOptions, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<IPagedResult<Task>> IEFRepository<Task, Task>.FindAllAsync(int pageNo, int pageSize, Func<IQueryable<Task>, IQueryable<Task>> queryOptions, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<Task?> IEFRepository<Task, Task>.FindAsync(Expression<Func<Task, bool>> filterExpression, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<Task?> IEFRepository<Task, Task>.FindAsync(Expression<Func<Task, bool>> filterExpression, Func<IQueryable<Task>, IQueryable<Task>> queryOptions, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<Task?> IEFRepository<Task, Task>.FindAsync(Func<IQueryable<Task>, IQueryable<Task>> queryOptions, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        void IRepository<Task>.Remove(Task entity)
        {
            throw new NotImplementedException();
        }

        void IRepository<Task>.Update(Task entity)
        {
            throw new NotImplementedException();
        }
    }
}
