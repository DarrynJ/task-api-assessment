using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasks.Domain.Repositories.Users;
using Tasks.Application.Interfaces.User;

namespace Tasks.Application.Implementation.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<string> LoginUser(string username, string password, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.FindAsync(x => x.Username == username && x.Password == password, cancellationToken);
            if (existingUser is null)
            {
                throw new Exception($"Invalid user credentials");
            }

            return existingUser.Username;
        }

        public async Task<bool> CreateUser(string username, string emailAddress, string password, CancellationToken cancellationToken)
        {
            var isExistingUser = await _userRepository.AnyAsync(x => x.Username == username && x.EmailAddress == emailAddress, cancellationToken);
            if (isExistingUser)
            {
                throw new Exception($"User already exists.");
            }

            var newUser = new Domain.Entities.User(username, emailAddress, password);

            _userRepository.Add(newUser);

            await _userRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<Domain.Entities.User> GetUser(Guid id, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FindAsync(x => x.Id == id, cancellationToken);
            if (user is null)
            {
                throw new Exception($"No user found for id.");
            }

            return user;
        }

        public async Task<Domain.Entities.User> UpdateUser(Domain.Entities.User user, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.FindAsync(x => x.Id == user.Id, cancellationToken);
            if (existingUser is null)
            {
                throw new Exception($"No user found for id: {user.Id}");
            }

            existingUser.Username = user.Username;
            existingUser.EmailAddress = user.EmailAddress;
            existingUser.Password = user.Password;

            await _userRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return existingUser;
        }

        public async Task<bool> DeleteUser(Guid id, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.FindAsync(x => x.Id == id, cancellationToken);
            if (existingUser is null)
            {
                throw new Exception($"No user found for Id: {id}");
            }

            _userRepository.Remove(existingUser);

            await _userRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }

        public void Dispose()
        {
            if (_userRepository != null)
            {
                _userRepository.UnitOfWork.SaveChangesAsync();
            }
        }
    }
}
