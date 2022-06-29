using ClassGenerator_BETA_.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClassGenerator_BETA_.Interfaces.Service
{
    public interface IDependencyInjectionGenerator
    {
        Task<string> Generate(IEnumerable<TablesBySchemaDTO> tables);
    }
}
