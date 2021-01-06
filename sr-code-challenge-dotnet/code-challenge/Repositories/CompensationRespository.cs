using challenge.Data;
using challenge.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Repositories
{
    public class CompensationRespository : ICompensationRepository
    {
        private readonly CompensationContext _compensationContext;
        
        public CompensationRespository(CompensationContext compensationContext)
        {
            _compensationContext = compensationContext;
        }

        public Compensation Add(Compensation compensation)
        {
            _compensationContext.Compensations.Add(compensation);
            return compensation;
        }

        public Compensation GetById(string id)
        {
            return _compensationContext.Compensations.AsEnumerable().Where(c => c.EmployeeId == id).SingleOrDefault();
        }

        public Task SaveAsync()
        {
            return _compensationContext.SaveChangesAsync();
        }
    }
}
