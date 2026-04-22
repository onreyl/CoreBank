using CoreBank.Domain.Accounts;
using CoreBank.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CoreBank.Infrastructure.Persistence.Repositories
{
    internal class AccountRepository : IAccountRepository
    {
        private readonly CoreBankDbContext _context;
        public AccountRepository(CoreBankDbContext context) => _context = context;
        public async Task AddAsync(Account account, CancellationToken cancellationToken = default)
            => await _context.Accounts.AddAsync(account, cancellationToken);

        public Task<Account?> GetByAccountNumberAsync(string accountNumber, CancellationToken cancellationToken = default)
            => _context.Accounts.FirstOrDefaultAsync(x => x.AccountNumber == accountNumber, cancellationToken);

        public async Task<IEnumerable<Account>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default)
            => await _context.Accounts.Where(x => x.CustomerId == customerId).ToListAsync(cancellationToken);

        public Task<Account?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => _context.Accounts.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        public Task UpdateAsync(Account account, CancellationToken cancellationToken = default)
        {
            _context.Accounts.Update(account);
            return Task.CompletedTask;
        }
    }
}
