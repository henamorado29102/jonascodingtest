using System;
using System.Collections.Generic;
using System.Web.Http;
using AutoMapper;
using BusinessLayer.Model.Interfaces;
using BusinessLayer.Model.Models;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class EmployeeController : ApiController
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;

        public EmployeeController(IEmployeeService employeeService, IMapper mapper)
        {
            _employeeService = employeeService;
            _mapper = mapper;
        }

        // GET api/employee
        public IEnumerable<EmployeeDto> GetAll()
        {
            var items = _employeeService.GetAllEmployees();
            return _mapper.Map<IEnumerable<EmployeeDto>>(items);
        }

        // GET api/employee/{id}
        public EmployeeDto Get(string id)
        {
            var item = _employeeService.GetEmployeeByCode(id);
            return _mapper.Map<EmployeeDto>(item);
        }

        // POST api/employee
        public IHttpActionResult Post([FromBody] EmployeeDto employeeDto)
        {
            if (employeeDto == null)
            {
                return BadRequest("Employee data is null.");
            }

            var employee = _mapper.Map<EmployeeInfo>(employeeDto);
            var result = _employeeService.SaveEmployee(employee);

            if (result)
            {
                return Created(new Uri(Request.RequestUri + "/" + employee.EmployeeCode), employee);
            }
            else
            {
                return BadRequest("Failed to save employee data.");
            }
        }

        // PUT api/employee/{id}
        public IHttpActionResult Put(string id, [FromBody] EmployeeDto employeeDto)
        {
            if (employeeDto == null || id != employeeDto.EmployeeCode)
            {
                return BadRequest("Invalid data.");
            }

            var employee = _mapper.Map<EmployeeInfo>(employeeDto);
            var result = _employeeService.SaveEmployee(employee);

            if (result)
            {
                return Ok(employee);
            }
            else
            {
                return BadRequest("Failed to update employee data.");
            }
        }

        // DELETE api/employee/{id}
        public IHttpActionResult Delete(string id)
        {
            var result = _employeeService.DeleteEmployee(id);

            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Failed to delete employee.");
            }
        }
    }
}
