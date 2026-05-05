using CoreBank.Domain.Repositories;
using CoreBank.Domain.Users;
using CoreBank.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CoreBank.Infrastructure.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly CoreBankDbContext _dbContext;

    public UserRepository(CoreBankDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var normalized = email.Trim().ToLowerInvariant();
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == normalized, cancellationToken);
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken)
        => await _dbContext.Users.AddAsync(user, cancellationToken);
}
