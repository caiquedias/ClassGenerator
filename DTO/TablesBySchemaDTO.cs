using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassGenerator_BETA_.DTO
{
    public class TablesBySchemaDTO
    {
        public string SchemaName { get; set; }
        public string TableName { get; set; }
        public string ClassName { get; set; }
        public string AccessKeyObject { get; set; }
    }
}
