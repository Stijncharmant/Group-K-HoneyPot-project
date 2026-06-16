using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModels
{
    public class Manufacturer
    {
        // private fields
        private readonly int _id;
        private readonly string _name;
        private readonly string _contactEmail;
        private readonly string _phoneNumber;
        private readonly string _address;
        
        // constructor
        public Manufacturer(int id, string name, string contactEmail, string phoneNumber, string address)
        {
            _id = id;
            _name = name;
            _contactEmail = contactEmail;
            _phoneNumber = phoneNumber;
            _address = address;
        }

        // properties
        public int Id { get { return _id; } }
        public string Name { get { return _name; } }
        public string ContactEmail { get { return _contactEmail; } }
        public string PhoneNumber { get { return _phoneNumber; } }
        public string Address { get { return _address; } }
    }
}
