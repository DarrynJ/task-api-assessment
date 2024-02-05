using System.Linq.Expressions;
using Moq;
using Tasks.Application.Implementation.Task;
using Tasks.Domain.Common.Interfaces;
using Tasks.Domain.Repositories.Tasks;
using Xunit;

public class TaskServiceTests
{
    [Fact]
    public async Task CreateTask_ValidInput_ReturnsCreatedTask()
    {
        // Arrange
        var taskRepositoryMock = new Mock<ITaskRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var taskService = new TaskService(taskRepositoryMock.Object);
        var cancellationToken = new CancellationToken();
        var newTask = new Tasks.Domain.Entities.Task("Title", "Description", Guid.NewGuid(), DateTime.Now.AddDays(7));
        taskRepositoryMock.Setup(x => x.UnitOfWork).Returns(unitOfWorkMock.Object);

        // Act
        var result = await taskService.CreateTask(newTask.Title, newTask.Description, newTask.Assignee, newTask.DueDate, cancellationToken);

        // Assert
        Assert.Equal(newTask.Title, result.Title);
        Assert.Equal(newTask.Description, result.Description);
        Assert.Equal(newTask.Assignee, result.Assignee);
        Assert.Equal(newTask.DueDate, result.DueDate);
        taskRepositoryMock.Verify(x => x.Add(It.IsAny<Tasks.Domain.Entities.Task>()), Times.Once);
        taskRepositoryMock.Verify(x => x.UnitOfWork.SaveChangesAsync(cancellationToken), Times.Once);
    }

    [Fact]
    public async Task DeleteTask_ExistingTaskId_ReturnsTrue()
    {
        // Arrange
        var taskRepositoryMock = new Mock<ITaskRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var taskService = new TaskService(taskRepositoryMock.Object);
        var cancellationToken = new CancellationToken();
        var existingTaskId = Guid.NewGuid();
        var existingTask = new Tasks.Domain.Entities.Task("Title", "Description", Guid.NewGuid(), DateTime.Now.AddDays(7));
        taskRepositoryMock.Setup(x => x.UnitOfWork).Returns(unitOfWorkMock.Object);
        taskRepositoryMock.Setup(x => x.FindAsync(It.IsAny<Expression<Func<Tasks.Domain.Entities.Task, bool>>>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(existingTask);

        // Act
        var result = await taskService.DeleteTask(existingTaskId, cancellationToken);

        // Assert
        Assert.True(result);
        taskRepositoryMock.Verify(x => x.Remove(existingTask), Times.Once);
        taskRepositoryMock.Verify(x => x.UnitOfWork.SaveChangesAsync(CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task GetAllTasks_ReturnsListOfTasks()
    {
        // Arrange
        var taskRepositoryMock = new Mock<ITaskRepository>();
        var taskService = new TaskService(taskRepositoryMock.Object);
        var cancellationToken = new CancellationToken();
        var tasks = new List<Tasks.Domain.Entities.Task>
        {
            new Tasks.Domain.Entities.Task("Title1", "Description1", Guid.NewGuid(), DateTime.Now.AddDays(7)),
            new Tasks.Domain.Entities.Task("Title2", "Description2", Guid.NewGuid(), DateTime.Now.AddDays(14))
        };
        taskRepositoryMock.Setup(x => x.FindAllAsync(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(tasks);

        // Act
        var result = await taskService.GetAllTasks(cancellationToken);

        // Assert
        Assert.Equal(tasks, result);
    }

    [Fact]
    public async Task GetAllAssigneeTasks_ReturnsListOfTasksForAssignee()
    {
        // Arrange
        var taskRepositoryMock = new Mock<ITaskRepository>();
        var taskService = new TaskService(taskRepositoryMock.Object);
        var cancellationToken = new CancellationToken();
        var assigneeId = Guid.NewGuid();
        var assigneeTasks = new List<Tasks.Domain.Entities.Task>
        {
            new Tasks.Domain.Entities.Task("Title1", "Description1", assigneeId, DateTime.Now.AddDays(7)),
            new Tasks.Domain.Entities.Task("Title2", "Description2", assigneeId, DateTime.Now.AddDays(14))
        };
        taskRepositoryMock.Setup(x => x.FindAllAsync(It.IsAny<Expression<Func<Tasks.Domain.Entities.Task, bool>>>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(assigneeTasks);

        // Act
        var result = await taskService.GetAllAssigneeTasks(assigneeId, cancellationToken);

        // Assert
        Assert.Equal(assigneeTasks, result);
    }

    [Fact]
    public async Task GetAllExpiredTasks_ReturnsListOfExpiredTasks()
    {
        // Arrange
        var taskRepositoryMock = new Mock<ITaskRepository>();
        var taskService = new TaskService(taskRepositoryMock.Object);
        var cancellationToken = new CancellationToken();
        var expiredTasks = new List<Tasks.Domain.Entities.Task>
        {
            new Tasks.Domain.Entities.Task("Title1", "Description1", Guid.NewGuid(), DateTime.Now.AddDays(-1)),
            new Tasks.Domain.Entities.Task("Title2", "Description2", Guid.NewGuid(), DateTime.Now.AddDays(-2))
        };
        taskRepositoryMock.Setup(x => x.FindAllAsync(It.IsAny<Expression<Func<Tasks.Domain.Entities.Task, bool>>>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(expiredTasks);

        // Act
        var result = await taskService.GetAllExpiredTasks(cancellationToken);

        // Assert
        Assert.Equal(expiredTasks, result);
    }

    [Fact]
    public async Task GetAllActiveTasks_ReturnsListOfActiveTasks()
    {
        // Arrange
        var taskRepositoryMock = new Mock<ITaskRepository>();
        var taskService = new TaskService(taskRepositoryMock.Object);
        var cancellationToken = new CancellationToken();
        var activeTasks = new List<Tasks.Domain.Entities.Task>
        {
            new Tasks.Domain.Entities.Task("Title1", "Description1", Guid.NewGuid(), DateTime.Now.AddDays(1)),
            new Tasks.Domain.Entities.Task("Title2", "Description2", Guid.NewGuid(), DateTime.Now.AddDays(2))
        };
        taskRepositoryMock.Setup(x => x.FindAllAsync(It.IsAny<Expression<Func<Tasks.Domain.Entities.Task, bool>>>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(activeTasks);

        // Act
        var result = await taskService.GetAllActiveTasks(cancellationToken);

        // Assert
        Assert.Equal(activeTasks, result);
    }

    [Fact]
    public async Task GetAllTasksFromDate_ReturnsListOfTasksFromDate()
    {
        // Arrange
        var taskRepositoryMock = new Mock<ITaskRepository>();
        var taskService = new TaskService(taskRepositoryMock.Object);
        var cancellationToken = new CancellationToken();
        var fromDate = DateTime.Now.AddDays(7);
        var tasksFromDate = new List<Tasks.Domain.Entities.Task>
        {
            new Tasks.Domain.Entities.Task("Title1", "Description1", Guid.NewGuid(), DateTime.Now.AddDays(7)),
            new Tasks.Domain.Entities.Task("Title2", "Description2", Guid.NewGuid(), DateTime.Now.AddDays(14))
        };
        taskRepositoryMock.Setup(x => x.FindAllAsync(It.IsAny<Expression<Func<Tasks.Domain.Entities.Task, bool>>>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(tasksFromDate);

        // Act
        var result = await taskService.GetAllTasksFromDate(fromDate, cancellationToken);

        // Assert
        Assert.Equal(tasksFromDate, result);
    }

    [Fact]
    public async Task GetTask_ExistingTaskId_ReturnsTask()
    {
        // Arrange
        var taskRepositoryMock = new Mock<ITaskRepository>();
        var taskService = new TaskService(taskRepositoryMock.Object);
        var cancellationToken = new CancellationToken();
        var existingTaskId = Guid.NewGuid();
        var existingTask = new Tasks.Domain.Entities.Task("Title", "Description", Guid.NewGuid(), DateTime.Now.AddDays(7));
        taskRepositoryMock.Setup(x => x.FindAsync(It.IsAny<Expression<Func<Tasks.Domain.Entities.Task, bool>>>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(existingTask);

        // Act
        var result = await taskService.GetTask(existingTaskId, cancellationToken);

        // Assert
        Assert.Equal(existingTask, result);
    }

    [Fact]
    public async Task UpdateTask_ExistingTaskId_ReturnsUpdatedTask()
    {
        // Arrange
        var taskRepositoryMock = new Mock<ITaskRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var taskService = new TaskService(taskRepositoryMock.Object);
        var cancellationToken = new CancellationToken();
        var existingTaskId = Guid.NewGuid();
        var existingTask = new Tasks.Domain.Entities.Task("Title", "Description", Guid.NewGuid(), DateTime.Now.AddDays(7)) { Id = existingTaskId };
        var updatedTask = new Tasks.Domain.Entities.Task("NewTitle", "NewDescription", Guid.NewGuid(), DateTime.Now.AddDays(14)) { Id = existingTaskId };
        taskRepositoryMock.Setup(x => x.UnitOfWork).Returns(unitOfWorkMock.Object);
        taskRepositoryMock.Setup(x => x.FindAsync(It.IsAny<Expression<Func<Tasks.Domain.Entities.Task, bool>>>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(existingTask);

        // Act
        var result = await taskService.UpdateTask(existingTaskId, updatedTask, cancellationToken);

        // Assert
        Assert.Equal(updatedTask.Id, result.Id);
        Assert.Equal(updatedTask.Title, result.Title);
        Assert.Equal(updatedTask.Description, result.Description);
        Assert.Equal(updatedTask.Assignee, result.Assignee);
        Assert.Equal(updatedTask.DueDate, result.DueDate);
        taskRepositoryMock.Verify(x => x.UnitOfWork.SaveChangesAsync(cancellationToken), Times.Once);
    }

    [Fact]
    public async void Dispose_CallsSaveChangesAsync()
    {
        // Arrange
        var taskRepositoryMock = new Mock<ITaskRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        taskRepositoryMock.Setup(x => x.UnitOfWork).Returns(unitOfWorkMock.Object);
        var taskService = new TaskService(taskRepositoryMock.Object);

        // Act
        taskService.Dispose();

        // Assert
        unitOfWorkMock.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Once);
    }
}
