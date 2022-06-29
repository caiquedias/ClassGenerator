using ClassGenerator_BETA_.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ClassGenerator_BETA_.Interfaces.Service
{
    public interface IGenericService
    {
        Task<List<TablesBySchemaDTO>> GetTablesBySchema(string schema);
    }
}
