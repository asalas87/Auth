using Domain.Sales.Customers;
using Domain.Sales.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Sales.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;
        public CustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(Customer customer) => _context.Customers.AddAsync(customer);

        public void Delete(Customer customer)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(CustomerId id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Customer>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<Customer?> GetByIdAsync(CustomerId id) => await _context.Customers.SingleOrDefaultAsync(c => c.Id == id);

        public void Update(Customer customer)
        {
            throw new NotImplementedException();
        }
    }
}
