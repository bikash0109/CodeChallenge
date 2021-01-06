using challenge.Models;
using challenge.Repositories;
using Microsoft.Extensions.Logging;

namespace challenge.Services
{
    public class CompensationService : ICompensationService
    {
        private readonly ICompensationRepository _compensationRepository;

        public CompensationService(ICompensationRepository compensationRepository)
        {
            _compensationRepository = compensationRepository;
        }

        public Compensation GetCompensationById(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                return _compensationRepository.GetById(id);
            }

            return null;
        }

        public Compensation CreateCompensation(Compensation compensation)
        {
            if (compensation != null)
            {
                _compensationRepository.Add(compensation);
                _compensationRepository.SaveAsync().Wait();
            }

            return compensation;
        }
    }
}
