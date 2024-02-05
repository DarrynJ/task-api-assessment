using Moq;
using System.Linq.Expressions;
using Tasks.Application.Implementation.User;
using Tasks.Domain.Common.Interfaces;
using Tasks.Domain.Repositories.Users;
using Xunit;

public class UserServiceTests
{
    [Fact]
    public async Task LoginUser_ValidCredentials_ReturnsApiKey()
    {
        // Arrange
        var userRepositoryMock = new Mock<IUserRepository>();
        var userService = new UserService(userRepositoryMock.Object);
        var existingUser = new Tasks.Domain.Entities.User("testuser", "test@email.com", "password") { ApiKey = "123456" };
        userRepositoryMock.Setup(x => x.FindAsync(It.IsAny<Expression<Func<Tasks.Domain.Entities.User, bool>>>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(existingUser);

        // Act
        var apiKey = await userService.LoginUser("testuser", "password", CancellationToken.None);

        // Assert
        Assert.Equal(existingUser.ApiKey, apiKey);
    }

    [Fact]
    public async Task LoginUser_InvalidCredentials_ThrowsException()
    {
        // Arrange
        var userRepositoryMock = new Mock<IUserRepository>();
        var userService = new UserService(userRepositoryMock.Object);
        userRepositoryMock.Setup(x => x.FindAsync(It.IsAny<Expression<Func<Tasks.Domain.Entities.User, bool>>>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync((Tasks.Domain.Entities.User)null);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => userService.LoginUser("nonexistentuser", "wrongpassword", CancellationToken.None));
    }

    [Fact]
    public async Task CreateUser_NewUser_SuccessfullyCreated()
    {
        // Arrange
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var userRepositoryMock = new Mock<IUserRepository>();
        var userService = new UserService(userRepositoryMock.Object);
        userRepositoryMock.Setup(x => x.UnitOfWork).Returns(unitOfWorkMock.Object);
        userRepositoryMock.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Tasks.Domain.Entities.User, bool>>>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(false);

        // Act
        var result = await userService.CreateUser("newuser", "newuser@example.com", "password", CancellationToken.None);

        // Assert
        Assert.True(result);
        userRepositoryMock.Verify(x => x.Add(It.IsAny<Tasks.Domain.Entities.User>()), Times.Once);
        userRepositoryMock.Verify(x => x.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetUser_ExistingUserId_ReturnsUser()
    {
        // Arrange
        var userRepositoryMock = new Mock<IUserRepository>();
        var userService = new UserService(userRepositoryMock.Object);
        var existingUserId = Guid.NewGuid();
        var existingUser = new Tasks.Domain.Entities.User("testuser", "test@email.com", "password") { Id = existingUserId };
        userRepositoryMock.Setup(x => x.FindAsync(It.IsAny<Expression<Func<Tasks.Domain.Entities.User, bool>>>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(existingUser);

        // Act
        var result = await userService.GetUser(existingUserId, CancellationToken.None);

        // Assert
        Assert.Equal(existingUser, result);
    }

    [Fact]
    public async Task GetUser_NonexistentUserId_ThrowsException()
    {
        // Arrange
        var userRepositoryMock = new Mock<IUserRepository>();
        var userService = new UserService(userRepositoryMock.Object);
        userRepositoryMock.Setup(x => x.FindAsync(It.IsAny<Expression<Func<Tasks.Domain.Entities.User, bool>>>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync((Tasks.Domain.Entities.User)null);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => userService.GetUser(Guid.NewGuid(), CancellationToken.None));
    }

    [Fact]
    public async Task UpdateUser_ExistingUser_ReturnsUpdatedUser()
    {
        // Arrange
        var userRepositoryMock = new Mock<IUserRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var userService = new UserService(userRepositoryMock.Object);
        var existingUserId = Guid.NewGuid();
        var existingApiKey = $"API-{Guid.NewGuid().ToString().Replace('-', '_').Trim()}_{DateTime.Now:yyyy-mm-dd}";
        var existingUser = new Tasks.Domain.Entities.User("testuser", "test@email.com", "password") { Id = existingUserId, Username = "oldUsername", EmailAddress = "old@example.com", Password = "oldPassword", ApiKey = existingApiKey };
        var updatedUser = new Tasks.Domain.Entities.User("testuser", "test@email.com", "password") { Id = existingUserId, Username = "newUsername", EmailAddress = "new@example.com", Password = "newPassword", ApiKey = existingApiKey };
        userRepositoryMock.Setup(x => x.UnitOfWork).Returns(unitOfWorkMock.Object);
        userRepositoryMock.Setup(x => x.FindAsync(It.IsAny<Expression<Func<Tasks.Domain.Entities.User, bool>>>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync(existingUser);

        // Act
        var result = await userService.UpdateUser(updatedUser, CancellationToken.None);

        // Assert
        Assert.Equal(updatedUser.Id, result.Id);
        Assert.Equal(updatedUser.ApiKey, result.ApiKey);
        Assert.Equal(updatedUser.Username, result.Username);
        Assert.Equal(updatedUser.Password, result.Password);
        Assert.Equal(updatedUser.EmailAddress, result.EmailAddress);
        userRepositoryMock.Verify(x => x.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateUser_NonexistentUserId_ThrowsException()
    {
        // Arrange
        var userRepositoryMock = new Mock<IUserRepository>();
        var userService = new UserService(userRepositoryMock.Object);
        userRepositoryMock.Setup(x => x.FindAsync(It.IsAny<Expression<Func<Tasks.Domain.Entities.User, bool>>>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync((Tasks.Domain.Entities.User)null);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => userService.UpdateUser(new Tasks.Domain.Entities.User("testuser", "test@email.com", "password") { Id = Guid.NewGuid() }, CancellationToken.None));
    }

    [Fact]
    public async Task DeleteUser_ExistingUserId_ReturnsTrue()
    {
        // Arrange
        var userRepositoryMock = new Mock<IUserRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var userService = new UserService(userRepositoryMock.Object);
        var existingUserId = Guid.NewGuid();
        var existingUser = new Tasks.Domain.Entities.User("testuser", "test@email.com", "password") { Id = existingUserId };
        userRepositoryMock.Setup(x => x.UnitOfWork).Returns(unitOfWorkMock.Object);
        userRepositoryMock.Setup(x => x.FindAsync(It.IsAny<Expression<Func<Tasks.Domain.Entities.User, bool>>>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(existingUser);

        // Act
        var result = await userService.DeleteUser(existingUserId, CancellationToken.None);

        // Assert
        Assert.True(result);
        userRepositoryMock.Verify(x => x.Remove(existingUser), Times.Once);
        userRepositoryMock.Verify(x => x.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteUser_NonexistentUserId_ThrowsException()
    {
        // Arrange
        var userRepositoryMock = new Mock<IUserRepository>();
        var userService = new UserService(userRepositoryMock.Object);
        userRepositoryMock.Setup(x => x.FindAsync(It.IsAny<Expression<Func<Tasks.Domain.Entities.User, bool>>>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync((Tasks.Domain.Entities.User)null);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => userService.DeleteUser(Guid.NewGuid(), CancellationToken.None));
    }

    [Fact]
    public void Dispose_CallsSaveChangesAsync()
    {
        // Arrange
        var userRepositoryMock = new Mock<IUserRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        userRepositoryMock.Setup(x => x.UnitOfWork).Returns(unitOfWorkMock.Object);
        var userService = new UserService(userRepositoryMock.Object);

        // Act
        userService.Dispose();

        // Assert
        unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
