using challenge.Models;
using challenge.Repositories;
using Microsoft.Extensions.Logging;
using System;

namespace challenge.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(ILogger<EmployeeService> logger, IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public Employee Create(Employee employee)
        {
            if (employee != null)
            {
                _employeeRepository.Add(employee);
                _employeeRepository.SaveAsync().Wait();
            }

            return employee;
        }

        public Employee GetById(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                return _employeeRepository.GetById(id);
            }

            return null;
        }

        /// <summary>
        /// A new method to enumerate the DirectReport array from the Json, similar to GetById, it gets the data as 'AsEnumerable'
        /// before converting it to a single object.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Employee GetTotalEmployeeDetailById(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                return _employeeRepository.GetTotalEmployeeDetailById(id);
            }

            return null;
        }

        public Employee Replace(Employee originalEmployee, Employee newEmployee)
        {
            if (originalEmployee != null)
            {
                _employeeRepository.Remove(originalEmployee);
                if (newEmployee != null)
                {
                    // ensure the original has been removed, otherwise EF will complain another entity w/ same id already exists
                    _employeeRepository.SaveAsync().Wait();

                    _employeeRepository.Add(newEmployee);
                    // overwrite the new id with previous employee id
                    newEmployee.EmployeeId = originalEmployee.EmployeeId;
                }
                _employeeRepository.SaveAsync().Wait();
            }

            return newEmployee;
        }

        public ReportingStructure GetReportingStructure(string id)
        {
            var employee = GetTotalEmployeeDetailById(id);
            if (employee != null)
            {
                var count = EmployeeReporteeCounter(employee);
                ReportingStructure rVal = new ReportingStructure
                {
                    Employee = employee,
                    NumberOfReports = count
                };
                return rVal;
            }
            return default;
        }


        // recursive binary tree postorder - can be done iteratively using a queue
        int EmployeeReporteeCounter(Employee employee)
        {
            int count = 0;
            if (employee.DirectReports != null)
            {
                foreach (var reportee in employee.DirectReports)
                {
                    count++;
                    count += EmployeeReporteeCounter(reportee);
                }
            }
            return count;
        }
    }
}
