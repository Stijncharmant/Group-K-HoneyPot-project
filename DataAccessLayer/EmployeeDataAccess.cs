using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainModels;

namespace DataAccessLayer
{
    public class EmployeeDataAccess
    {
        private readonly AppDbContext _context;

        public EmployeeDataAccess(AppDbContext context)
        {
            _context = context;
        }

        #region CREATE

        public void AddEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();
        }

        #endregion

        #region READ

        public List<Employee> GetAllEmployees()
        {
            return _context.Employees.ToList();
        }

        public Employee? GetEmployeeById(long id)
        {
            return _context.Employees
                .FirstOrDefault(e => e.Id == id);
        }

        public Employee? GetEmployeeByEmail(string email)
        {
            return _context.Employees.FirstOrDefault(e => e.Email == email);
        }
        #endregion

        #region UPDATE

        public void UpdateEmployee(Employee updatedEmployee)
        {
            var existingEmployee = _context.Employees.FirstOrDefault(e => e.Id == updatedEmployee.Id);

            if (existingEmployee == null)
                throw new Exception("Employee not found");

            _context.Entry(existingEmployee).CurrentValues.SetValues(updatedEmployee);

            _context.SaveChanges();
        }

        #endregion

        #region DELETE

        public void DeleteEmployee(long id)
        {
            var employee = _context.Employees.FirstOrDefault(e => e.Id == id);

            if (employee == null)
                throw new Exception("Employee not found");

            // Note: Since the 'employees' table schema does not include an 'is_archived' column,
            // we perform a standard database physical/hard delete here.
            _context.Employees.Remove(employee);

            _context.SaveChanges();
        }

        #endregion
    }
}
