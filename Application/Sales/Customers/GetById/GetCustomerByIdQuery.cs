using Application.Sales.Customers.Common;
using ErrorOr;
using MediatR;

namespace Application.Sales.Customers.GetById;
public record GetCustomerByIdQuery(Guid Id) : IRequest<ErrorOr<CustomerResponse>>;