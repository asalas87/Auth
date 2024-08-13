using Application.Customers.Create;
using Domain.Customers;
using Domain.DomainErrors;
using Domain.Primitives;

namespace UnitTest.Application.Customer.UnitTests.Create
{
    public class CreateCustomerCommandHandlerUnitTests
    {
        private readonly Mock<ICustomerRepository> _mockCustomerRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly CreateCustomerCommandHandler _handler;

        public CreateCustomerCommandHandlerUnitTests()
        {
            _mockCustomerRepository = new Mock<ICustomerRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _handler = new CreateCustomerCommandHandler(_mockCustomerRepository.Object, _mockUnitOfWork.Object);
        }

        [Fact]
        public async Task HandleCreateCustomerCommand_WhenPhoneNumberHasBadFormat_ShouldReturnValidationError()
        {
            //Arrange
            //Se configura los parametros de entrada de nuestra prueba unitaria
            CreateCustomerCommand command = new CreateCustomerCommand("Agustin", "Salas", "agu.nasum@gmail.com","654612321564", "","","","","","");

            //Act
            // Se ejecuta el metodo a probar de nuestra prueba unitaria
            var result = await _handler.Handle(command, default);
            //Assert
            //se verifica los datos de retorno de nuetro metodo probado en la prueba unitaria
            result.IsError.Should().BeTrue();
            result.FirstError.Type.Should().Be(ErrorType.Validation);
            result.FirstError.Code.Should().Be(Errors.Customer.PhoneNumberWithBadFormat.Code);
            result.FirstError.Description.Should().Be(Errors.Customer.PhoneNumberWithBadFormat.Description);
        }
    }
}