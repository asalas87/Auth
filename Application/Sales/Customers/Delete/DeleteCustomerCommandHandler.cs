﻿using Domain.Primitives;
using Domain.Sales;
using Domain.Sales.Customers;
using Domain.Sales.Entities;
using Domain.ValueObjects;
using ErrorOr;
using MediatR;

namespace Application.Sales.Customers.Delete
{
    internal sealed class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, ErrorOr<Unit>>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCustomerCommandHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
        {
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<ErrorOr<Unit>> Handle(DeleteCustomerCommand command, CancellationToken cancellationToken)
        {

            if (await _customerRepository.GetByIdAsync(new CustomerId(command.Id)) is not Customer customer)
            {
                return Error.NotFound("Customer.NotFound", "The customer with the provide Id was not found");
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
