using System.Threading.Tasks;

namespace Tasks.Application.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        string? UserId { get; }
        string? UserName { get; }
        Task<bool> IsInRoleAsync(string role);
        Task<bool> AuthorizeAsync(string policy);
    }
}