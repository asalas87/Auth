using ErrorOr;
using MediatR;

namespace Application.Sales.Customers.Delete;
public record DeleteCustomerCommand(Guid Id) : IRequest<ErrorOr<Unit>>;
