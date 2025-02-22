﻿using System.Collections.Generic;
using BusinessLayer.Model.Models;

namespace BusinessLayer.Model.Interfaces
{
    public interface IEmployeeService
    {
        IEnumerable<EmployeeInfo> GetAllEmployees();
        EmployeeInfo GetEmployeeByCode(string employeeCode);

        bool SaveEmployee(EmployeeInfo company);
        bool DeleteEmployee(string employeeCode);
    }
}
