using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks.Application.Interfaces.Task
{
    public interface ITaskService : IDisposable
    {
        Task<Domain.Entities.Task> CreateTask(string title, string description, Guid assignee, DateTime dueDate, CancellationToken cancellationToken);
        Task<Domain.Entities.Task> GetTask(Guid id, CancellationToken cancellationToken);
        Task<List<Domain.Entities.Task>> GetAllTasks(CancellationToken cancellationToken);
        Task<List<Domain.Entities.Task>> GetAllAssigneeTasks(Guid assigneeId, CancellationToken cancellationToken);
        Task<bool> DeleteTask(Guid id, CancellationToken cancellationToken);
        Task<Domain.Entities.Task> UpdateTask(Guid id, Domain.Entities.Task task, CancellationToken cancellationToken);
    }
}
