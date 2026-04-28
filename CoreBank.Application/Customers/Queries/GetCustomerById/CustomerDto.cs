namespace CoreBank.Application.Customers.Queries.GetCustomerById;

public sealed record CustomerDto(
    Guid Id,
    string Tckn,
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    string PhoneNumber,
    string Email,
    string Status,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc);