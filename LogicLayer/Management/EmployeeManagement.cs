using DataAccessLayer;
using DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace LogicLayer.Management
{
    public class EmployeeManagement
    {
        private readonly EmployeeDataAccess _employeeDataAccess;

        public EmployeeManagement(EmployeeDataAccess employeeDataAccess)
        {
            _employeeDataAccess = employeeDataAccess;
        }

        #region CREATE

        public void AddEmployee(string firstName, string lastName, string email, string password, bool isAdmin)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            Employee employee = new Employee(firstName, lastName, email, hashedPassword, isAdmin);

            _employeeDataAccess.AddEmployee(employee);
        }

        #endregion

        #region READ

        public List<Employee> GetAllEmployees()
        {
            return _employeeDataAccess.GetAllEmployees();
        }

        public Employee? GetEmployeeById(long id)
        {
            return _employeeDataAccess.GetEmployeeById(id);
        }

        #endregion

        #region UPDATE

        public void UpdateEmployee(Employee employee)
        {
            _employeeDataAccess.UpdateEmployee(employee);
        }

        #endregion

        #region DELETE

        public void DeleteEmployee(long id)
        {
            _employeeDataAccess.DeleteEmployee(id);
        }

        #endregion

        #region AUTHENTICATE


        public Employee? VerifyCredentials(string email, string password)
        {
            var employee = _employeeDataAccess.GetEmployeeByEmail(email);

            if(employee == null)
            {
                return null;
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, employee.Password);

            return isPasswordValid ? employee : null;
        }
        #endregion
    }
}
