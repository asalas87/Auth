using Application.Sales.Customers.Common;
using Domain.Primitives;
using Domain.Sales.Customers;
using Domain.Sales.Entities;
using ErrorOr;
using MediatR;

namespace Application.Sales.Customers.GetAll
{
    internal sealed class GetAllCustomerQueryHandler : IRequestHandler<GetAllCustomerQuery, ErrorOr<IReadOnlyList<CustomerResponse>>>
    {
        private readonly ICustomerRepository _customerRepository;

        public GetAllCustomerQueryHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        }

        public async Task<ErrorOr<IReadOnlyList<CustomerResponse>>> Handle(GetAllCustomerQuery request, CancellationToken cancellationToken)
        {
            IReadOnlyList<Customer> customers = await _customerRepository.GetAll();

            return customers.Select(customer => new CustomerResponse(
                customer.Id.Value,
                customer.FullName,
                customer.Email,
                customer.PhoneNumber.Value,
                new AddressResponse(customer.Address.Country,
                customer.Address.Line1,
                customer.Address.Line2,
                customer.Address.City,
                customer.Address.State,
                customer.Address.ZipCode),
                customer.Active
                )).ToList();
        }
    }
}
