using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks.Domain.Entities
{
    public class User
    {
        public User(string username, string emailAddress, string password)
        {
            Id = Guid.NewGuid();
            Username = username;
            EmailAddress = emailAddress;
            Password = password;

            ApiKey = $"API-{Guid.NewGuid().ToString().Replace('-', '_').Trim()}_{DateTime.Now:yyyy-mm-dd}";
        }

        public Guid Id { get; set; }
        public string Username { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string ApiKey { get; set; }
    }
}
