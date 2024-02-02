using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasks.Domain.Repositories.Users;
using Tasks.Domain.Services;

namespace Tasks.Application.Implementation.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<string> LoginUser(string username, string password)
        {
            var existingUser = await _userRepository.FindAsync(x => x.Username == username && x.Password == password);
            if (existingUser is null)
            {
                throw new Exception($"Invalid user credentials");
            }

            return existingUser.Username;
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
