using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks.Domain.Repositories
{
    public interface ITaskRepository
    {
        Task<Task> CreateTask(string title, string description, Guid assignee, DateTime dueDate);
        Task<Task> GetTask(Guid id);
        Task<List<Task>> GetAllTasks();
        Task<bool> DeleteTask(Guid id);
        Task<Task> UpdateTask(Guid id,  Task task);
    }
}
