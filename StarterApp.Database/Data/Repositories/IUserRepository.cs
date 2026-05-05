using StarterApp.Database.Models;
namespace StarterApp.Database.Data.Repositories;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User> CreateAsync(User user);
    Task<User> UpdateAsync(User user);
    Task AssignRoleAsync(int userId, int roleId);
}