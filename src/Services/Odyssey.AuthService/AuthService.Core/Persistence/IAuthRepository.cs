using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace AuthService.Core.Persistence
{
    public interface IAuthRepository : IRepository
    {
        DbSet<T> Set<T>() where T : class;
        DatabaseFacade Database { get; }
    }
}
