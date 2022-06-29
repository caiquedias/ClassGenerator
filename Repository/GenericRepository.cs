using ClassGenerator_BETA_;
using ClassGenerator_BETA_.DTO;
using ClassGenerator_BETA_.Interfaces.Repository;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Client.API.Repository
{
    public class GenericRepository : IGenericRepository
    {
        protected readonly IConfiguration _configuration;
        private readonly string ConnectionString;
        public GenericRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            ConnectionString = Program._connectionString;
        }
        public async Task<ResultDto> GetTablesBySchema(string schema)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var dataTable = new DataTable();

                    using (SqlConnection con = new SqlConnection(ConnectionString))
                    {
                        var commandText = "SELECT "
                                        + "     SCHEMA_NAME(schema_id) as schemaName, "
                                        + "     name AS TableName "
                                        + " FROM sys.tables "
                                        + $" WHERE SCHEMA_NAME(schema_id) = '{schema}' ";

                        SqlCommand cmd = new SqlCommand(commandText, con);
                        cmd.CommandType = CommandType.Text;
                        con.Open();

                        var dataAdapter = new SqlDataAdapter { SelectCommand = cmd };

                        dataAdapter.Fill(dataTable);
                    }

                    return Task.FromResult(
                            new ResultDto(true, DataTableToJSON(dataTable))
                             );
                }
                catch
                {
                    throw;
                }
            });
        }

        private string DataTableToJSON(DataTable table)
        {
            return JsonConvert.SerializeObject(table);
        }
    }
}
