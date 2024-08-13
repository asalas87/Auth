using Customers.Common;
using ErrorOr;
using MediatR;

namespace Application.Customers.GetAll;
public record GetAllCustomerQuery() : IRequest<ErrorOr<IReadOnlyList<CustomerResponse>>>;