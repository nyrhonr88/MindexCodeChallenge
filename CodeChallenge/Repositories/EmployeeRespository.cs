using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using CodeChallenge.Data;

namespace CodeChallenge.Repositories
{
    public class EmployeeRespository : IEmployeeRepository
    {
        private readonly EmployeeContext _employeeContext;
        private readonly ILogger<IEmployeeRepository> _logger;

        public EmployeeRespository(ILogger<IEmployeeRepository> logger, EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
            _logger = logger;
        }

        public Employee Add(Employee employee)
        {
            employee.EmployeeId = Guid.NewGuid().ToString();
            _employeeContext.Employees.Add(employee);
            return employee;
        }

        public Employee GetById(string id)
        {
            // I was consistently encountering one of the weirdest bugs I've ever seen
            // For some reason, the Employee.directReports list wouldn't materialize into the employee variable unless I breakpointed on that line
            // and then expanded the enumerable in Visual Studio...truly bizarre
            // Of course, I would never do _employeeContext.Employees.ToList() in an actual codebase (as it would materialize that entire table into memory), but was necessary to work around that odd bug
            // This was something that I tested on the master branch as well, and was a problem with the way the code was originally written
            var employees = _employeeContext.Employees.ToList();

            var employee = employees.SingleOrDefault(e => e.EmployeeId == id);
            return employee;
        }

        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }

        public Employee Remove(Employee employee)
        {
            return _employeeContext.Remove(employee).Entity;
        }
    }
}
