using CoreBank.Domain.Common;
using CoreBank.Domain.Events;

namespace CoreBank.Domain.Customers
{
    public class Customer : Entity
    {
        public string Tckn { get; private set; } = null!;
        public string FirstName { get; private set; } = null!;
        public string LastName { get; private set; } = null!;
        public DateOnly DateOfBirth { get; private set; }
        public string PhoneNumber { get; private set; } = null!;
        public string Email { get; private set; } = null!;
        public CustomerStatus Status { get; private set; }

        private Customer() { }
        
        private Customer(
            string tckn,
            string firstName,
            string lastName,
            DateOnly dateOfBirth,
            string phoneNumber,
            string email,
            CustomerStatus status)
        {
            Tckn = tckn;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            PhoneNumber = phoneNumber;
            Email = email;
            Status = status;
        }

        public static Result<Customer> Create(
            string tckn,
            string firstName,
            string lastName,
            DateOnly dateOfBirth,
            string phoneNumber,
            string email)
        {
            if (string.IsNullOrWhiteSpace(tckn))
                return Result<Customer>.Failure("Tckn boş olamaz");
            if (tckn.Length != 11)
                return Result<Customer>.Failure("Tckn 11 hane olmalı");
            if (!tckn.All(char.IsDigit))
                return Result<Customer>.Failure("Tckn sadece rakam içermeli");

            if (string.IsNullOrWhiteSpace(firstName))
                return Result<Customer>.Failure("FirstName boş olamaz");
            if (firstName.Length < 2)
                return Result<Customer>.Failure("FirstName en az 2 karakter olmalı");

            if (string.IsNullOrWhiteSpace(lastName))
                return Result<Customer>.Failure("LastName boş olamaz");
            if (lastName.Length < 2)
                return Result<Customer>.Failure("LastName en az 2 karakter olmalı");

            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var age = today.Year - dateOfBirth.Year;
            if (dateOfBirth > today.AddYears(-age))
                age--;
            if (age < 18)
                return Result<Customer>.Failure("Müşteri 18 yaşından büyük olmalı");

            if (string.IsNullOrWhiteSpace(phoneNumber))
                return Result<Customer>.Failure("PhoneNumber boş olamaz");

            if (string.IsNullOrWhiteSpace(email))
                return Result<Customer>.Failure("Email boş olamaz");
            if (!email.Contains('@'))
                return Result<Customer>.Failure("Email geçerli formatta olmalı");

            var customer = new Customer(
                tckn,
                firstName,
                lastName,
                dateOfBirth,
                phoneNumber,
                email,
                CustomerStatus.PendingVerification);

            customer.RaiseDomainEvent(new CustomerCreated(
                CustomerId: customer.Id,
                Tckn: tckn,
                Email: email,
                EventId: Guid.NewGuid(),
                OccurredOn: DateTime.UtcNow
            ));

            return Result<Customer>.Success(customer);
        }

        public Result Verify()
        {
            if (Status != CustomerStatus.PendingVerification)
                return Result.Failure($"Sadece PendingVerification durumundaki müşteri verify edilebilir. Şu anki durum: {Status}");

            var oldStatus = Status;
            Status = CustomerStatus.Active;
            UpdatedAtUtc = DateTime.UtcNow;
            
            RaiseDomainEvent(new CustomerVerified(
                CustomerId: Id,
                EventId: Guid.NewGuid(),
                OccurredOn: DateTime.UtcNow
            ));
            
            return Result.Success();
        }

        public Result Suspend() 
        {
            if (Status != CustomerStatus.Active)
                return Result.Failure($"Sadece Active durumundaki müşteri suspend edilebilir. Şu anki durum: {Status}");

            var oldStatus = Status;
            Status = CustomerStatus.Passive;
            UpdatedAtUtc = DateTime.UtcNow;
            
            RaiseDomainEvent(new CustomerStatusChanged(
                CustomerId: Id,
                OldStatus: oldStatus,
                NewStatus: Status,
                EventId: Guid.NewGuid(),
                OccurredOn: DateTime.UtcNow
            ));
            
            return Result.Success();
        }

        public Result Close()
        {
           if (Status == CustomerStatus.Closed || Status == CustomerStatus.PendingVerification)
                return Result.Failure($"Müşteri zaten kapalı veya doğrulama bekliyor. Şu anki durum: {Status}");

            var oldStatus = Status;
            Status = CustomerStatus.Closed;
            UpdatedAtUtc = DateTime.UtcNow;
            
            RaiseDomainEvent(new CustomerStatusChanged(
                CustomerId: Id,
                OldStatus: oldStatus,
                NewStatus: Status,
                EventId: Guid.NewGuid(),
                OccurredOn: DateTime.UtcNow
            ));
            
            return Result.Success();
        }

        public Result Reactivate()
        {
            if (Status != CustomerStatus.Passive)
                return Result.Failure($"Sadece Passive durumundaki müşteri reaktif edilebilir. Şu anki durum: {Status}");

            var oldStatus = Status;
            Status = CustomerStatus.Active;
            UpdatedAtUtc = DateTime.UtcNow;
            
            RaiseDomainEvent(new CustomerStatusChanged(
                CustomerId: Id,
                OldStatus: oldStatus,
                NewStatus: Status,
                EventId: Guid.NewGuid(),
                OccurredOn: DateTime.UtcNow
            ));
            
            return Result.Success();
        }
    }
}