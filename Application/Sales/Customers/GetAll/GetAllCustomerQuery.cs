using Application.Sales.Customers.Common;
using ErrorOr;
using MediatR;

namespace Application.Sales.Customers.GetAll;
public record GetAllCustomerQuery() : IRequest<ErrorOr<IReadOnlyList<CustomerResponse>>>;