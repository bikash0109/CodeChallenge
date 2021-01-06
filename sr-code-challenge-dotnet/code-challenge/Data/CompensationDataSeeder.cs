using challenge.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Data
{
    public class CompensationDataSeeder
    {
        private CompensationContext _compensationContext;

        public CompensationDataSeeder(CompensationContext compensationContext)
        {
            _compensationContext = compensationContext;
        }

        public async Task Seed()
        {
            if (!_compensationContext.Compensations.Any())
            {
                List<Compensation> compensations = LoadCompensations();
                _compensationContext.Compensations.AddRange(compensations);

                await _compensationContext.SaveChangesAsync();
            }
        }

        private List<Compensation> LoadCompensations()
        {
            using (FileStream fs = new FileStream("resources/CompensationSeedData.json", FileMode.Open))
            using (StreamReader sr = new StreamReader(fs))
            using (JsonReader jr = new JsonTextReader(sr))
            {
                JsonSerializer serializer = new JsonSerializer();

                List<Compensation> compensations = serializer.Deserialize<List<Compensation>>(jr);

                return compensations;
            }
        }
    }
}
