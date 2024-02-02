using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasks.Domain.Entities;

namespace Tasks.Application.Interfaces.User
{
    public interface IUserService : IDisposable
    {
        Task<string> LoginUser(string username, string password, CancellationToken cancellationToken);
        Task<bool> CreateUser(string username, string emailAddress, string password, CancellationToken cancellationToken);
        Task<Domain.Entities.User> GetUser(Guid id, CancellationToken cancellationToken);
        Task<Domain.Entities.User> UpdateUser(Domain.Entities.User user, CancellationToken cancellationToken);
        Task<bool> DeleteUser(Guid id, CancellationToken cancellationToken);
    }
}
