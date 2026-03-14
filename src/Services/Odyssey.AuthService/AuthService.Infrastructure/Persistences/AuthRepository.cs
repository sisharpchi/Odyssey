using AuthService.Core.Persistence;
using AuthService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace AuthService.Infrastructure.Persistences;

internal class AuthRepository : GenericRepository<AuthDbContext>, IAuthRepository
{
    public DatabaseFacade Database => Context.Database;

    public AuthRepository(AuthDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
    {
    }

    public DbSet<T> Set<T>() where T : class
    {
        return Context.Set<T>();
    }
}
