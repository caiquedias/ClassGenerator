using ClassGenerator_BETA_.DTO;
using ClassGenerator_BETA_.Interfaces.Repository;
using ClassGenerator_BETA_.Interfaces.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ClassGenerator_BETA_.Service
{
    public class GenericService : IGenericService
    {
        private readonly IGenericRepository _repository;
        public GenericService(IGenericRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<TablesBySchemaDTO>> GetTablesBySchema(string schema)
        {
            var result = await _repository.GetTablesBySchema(schema);

            if (result.GetSucess())
            {
                return JsonConvert.DeserializeObject<List<TablesBySchemaDTO>>(result.GetObjectReturn().ToString());
            }

            return null;
        }
    }
}
