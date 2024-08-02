using System;
using System.Collections.Generic;
using System.Linq;
using DataAccessLayer.Model.Interfaces;
using DataAccessLayer.Model.Models;

namespace DataAccessLayer.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IDbWrapper<Employee> _employeeDbWrapper;

        public EmployeeRepository(IDbWrapper<Employee> employeeDbWrapper)
        {
            _employeeDbWrapper = employeeDbWrapper;
        }

        public IEnumerable<Employee> GetAll()
        {
            return _employeeDbWrapper.FindAll();
        }

        public Employee GetByCode(string employeeCode)
        {
            return _employeeDbWrapper.Find(t => t.EmployeeCode.Equals(employeeCode))?.FirstOrDefault();
        }

        public bool SaveEmployee(Employee employee)
        {
            var existingEmployee = _employeeDbWrapper.Find(t =>
                t.EmployeeCode.Equals(employee.EmployeeCode))?.FirstOrDefault();
            if (existingEmployee != null)
            {
                existingEmployee.EmployeeName = employee.EmployeeName;
                existingEmployee.Occupation = employee.Occupation;
                existingEmployee.EmployeeStatus = employee.EmployeeStatus;
                existingEmployee.EmailAddress = employee.EmailAddress;
                existingEmployee.Phone = employee.Phone;
                existingEmployee.LastModified = employee.LastModified;
                return _employeeDbWrapper.Update(existingEmployee);
            }

            return _employeeDbWrapper.Insert(employee);
        }

        public bool DeleteEmployee(string employeeCode)
        {
            return _employeeDbWrapper.Delete(t => t.EmployeeCode.Equals(employeeCode));
        }
    }
}
