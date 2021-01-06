using challenge.Models;
using System;
using System.Threading.Tasks;

namespace challenge.Repositories
{
    public interface IEmployeeRepository
    {
        Employee GetById(string id);
        Employee GetTotalEmployeeDetailById(string id);
        Employee Add(Employee employee);
        Employee Remove(Employee employee);
        Task SaveAsync();
    }
}