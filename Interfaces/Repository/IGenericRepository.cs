using ClassGenerator_BETA_.DTO;
using System.Threading.Tasks;

namespace ClassGenerator_BETA_.Interfaces.Repository
{
    public interface IGenericRepository
    {
        public Task<ResultDto> GetTablesBySchema(string schema);
    }
}
