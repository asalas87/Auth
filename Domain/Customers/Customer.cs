﻿using Domain.Primitives;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Customers
{
    public sealed class Customer : AggergateRoot
    {
        public Customer(CustomerId id, string name, string lastName, string email, PhoneNumber phoneNumber, Address address, bool active)
        { 
            Id = id;
            Name = name;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            Address = address;
            Active = active;
        }
        public Customer() 
        { 
        }
        public CustomerId Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public string FullName => $"{Name} {LastName}";
        public string Email { get; private set; } = string.Empty;
        public PhoneNumber PhoneNumber { get; private set; }
        public Address Address { get; private set; }
        public bool Active { get; set; }
        public static Customer UpdateCustomer(Guid id, string name, string lastName, string email, PhoneNumber phoneNumber, Address address, bool active)
        {
            return new Customer(new CustomerId(id), name, lastName, email, phoneNumber, address, active);
        }
    }
}
