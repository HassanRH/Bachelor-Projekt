using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebShop.Entity
{
    public class Customer
    {
        public Customer()
        {
        }
        public Customer(string name, string lname, string email, string address, string apartsuite, string company, string zip, string city, string phone)
        {
            Name = name;
            Email = email;
            Address = address;
            Apartsuite = apartsuite;
            Company = company;
            Zip = zip;
            City = city;
            Phone = phone;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string Company { get; set; }
        public string Apartsuite { get; set; }

        public string Zip { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }

    }
}