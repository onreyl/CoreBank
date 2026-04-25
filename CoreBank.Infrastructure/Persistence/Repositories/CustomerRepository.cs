using CoreBank.Domain.Customers;
using CoreBank.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CoreBank.Infrastructure.Persistence.Repositories;

internal class CustomerRepository : ICustomerRepository
{
    private readonly CoreBankDbContext _context;

    public CustomerRepository(CoreBankDbContext context)
    {
        _context = context;
    }

    public Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => _context.Customers.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<Customer?> GetByTcknAsync(string tckn, CancellationToken cancellationToken = default)
        => _context.Customers.FirstOrDefaultAsync(x => x.Tckn == tckn, cancellationToken);

    public Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => _context.Customers.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

    public async Task AddAsync(Customer customer, CancellationToken cancellationToken = default)
        => await _context.Customers.AddAsync(customer, cancellationToken);

    public Task UpdateAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        _context.Customers.Update(customer);
        return Task.CompletedTask;
    }
}