using Domain.Customers;
using ErrorOr;
using MediatR;

namespace Application.Customers.Create;
public record DeleteCustomerCommand(Guid Id) : IRequest<ErrorOr<Unit>>;
