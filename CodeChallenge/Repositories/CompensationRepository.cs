using CodeChallenge.Data;
using CodeChallenge.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CodeChallenge.Repositories
{
    public class CompensationRepository : ICompensationRepository
    {
        private readonly EmployeeContext _employeeContext;
        private readonly ILogger<IEmployeeRepository> _logger;

        public CompensationRepository(EmployeeContext employeeContext, ILogger<IEmployeeRepository> logger)
        {
            _employeeContext = employeeContext;
            _logger = logger;
        }

        public Compensation Add(Compensation compensation)
        {
            compensation.Id = Guid.NewGuid().ToString();
            _employeeContext.Compensation.Add(compensation);
            return compensation;
        }

        public Compensation GetByEmployeeId(string employeeId)
        {
            var compensation = _employeeContext.Compensation
                // This Include may be necessary depending on the requirements of the endpoint. We already have an Employee Controller
                // so it probably isn't necessary to return both of these things at the same time. I suppose it will
                // depend on the context of why the endpoint is being built/what it's being used for
                .Include(c => c.Employee)
                .ThenInclude(e => e.DirectReports)
                .SingleOrDefault(c => c.EmployeeId == employeeId);

            return compensation;
        }

        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }
    }
}
