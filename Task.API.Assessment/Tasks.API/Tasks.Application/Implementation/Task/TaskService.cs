using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasks.Application.Interfaces.Task;
using Tasks.Domain.Repositories.Tasks;

namespace Tasks.Application.Implementation.Task
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<Domain.Entities.Task> CreateTask(string title, string description, Guid assignee, DateTime dueDate, CancellationToken cancellationToken)
        {
            var newTask = new Domain.Entities.Task(title, description, assignee, dueDate);

            _taskRepository.Add(newTask);

            await _taskRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return newTask;
        }

        public async Task<bool> DeleteTask(Guid id, CancellationToken cancellationToken)
        {
            var existingTask = await _taskRepository.FindAsync(x => x.Id == id, cancellationToken);
            if (existingTask is null)
            {
                throw new Exception($"No task found for id: {id}");
            }

            _taskRepository.Remove(existingTask);

            await _taskRepository.UnitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<List<Domain.Entities.Task>> GetAllTasks(CancellationToken cancellationToken)
        {
            var tasks = await _taskRepository.FindAllAsync(cancellationToken);

            return tasks;
        }

        public async Task<List<Domain.Entities.Task>> GetAllAssigneeTasks(Guid assigneeId, CancellationToken cancellationToken)
        {
            var assigneeTasks = await _taskRepository.FindAllAsync(x => x.Assignee == assigneeId, cancellationToken);

            return assigneeTasks;
        }

        public async Task<Domain.Entities.Task> GetTask(Guid id, CancellationToken cancellationToken)
        {
            var existingTask = await _taskRepository.FindAsync(x => x.Id == id, cancellationToken);
            if (existingTask is null)
            {
                throw new Exception($"No task found for id: {id}");
            }

            return existingTask;
        }

        public async Task<Domain.Entities.Task> UpdateTask(Guid id, Domain.Entities.Task task, CancellationToken cancellationToken)
        {
            var existingTask = await _taskRepository.FindAsync(x => x.Id == id, cancellationToken);
            if (existingTask is null)
            {
                throw new Exception($"No task found for id: {id}");
            }

            existingTask.Title = task.Title;
            existingTask.Description = task.Description;
            existingTask.Assignee = task.Assignee;
            existingTask.DueDate = task.DueDate;

            await _taskRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return existingTask;
        }

        public async void Dispose()
        {
            if (_taskRepository != null)
            {
                await _taskRepository.UnitOfWork.SaveChangesAsync();
            }
        }
    }
}
