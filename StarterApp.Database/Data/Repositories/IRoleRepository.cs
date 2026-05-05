using StarterApp.Database.Models;

namespace StarterApp.Database.Data.Repositories;

public interface IRoleRepository
{
    Task<Role?> GetDefaultRoleAsync();
}
