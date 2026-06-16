using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModels
{
    public class Employee
    {
        // Properties
        public long Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public bool IsAdmin { get; private set; }

        // constructor
        private Employee()
        {
        }

        public Employee(
            string firstName,
            string lastName,
            string email,
            string password,
            bool isAdmin)
        {
            if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentException("First name cannot be empty.");
            if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentException("Last name cannot be empty.");
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email cannot be empty.");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Password cannot be empty.");

            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            IsAdmin = isAdmin;
        }

        // domain methods
        public void UpdateProfile(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentException("First name cannot be empty.");
            if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentException("Last name cannot be empty.");

            FirstName = firstName;
            LastName = lastName;
        }

        public void UpdateEmail(string newEmail)
        {
            if (string.IsNullOrWhiteSpace(newEmail)) throw new ArgumentException("Email cannot be empty.");

            Email = newEmail;
        }

        public void ChangePassword(string newHashedPassword)
        {
            if (string.IsNullOrWhiteSpace(newHashedPassword)) throw new ArgumentException("Password cannot be empty.");

            Password = newHashedPassword;
        }

        public void SetAdminStatus(bool isAdmin)
        {
            IsAdmin = isAdmin;
        }

    }
}
