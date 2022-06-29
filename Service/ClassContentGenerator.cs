using ClassGenerator_BETA_.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassGenerator_BETA_.Service
{
    public class ClassContentGenerator : IClassContentGenerator
    {
        public async Task<string> Generate(string className, string domain, string namespaceString, string entity, string folder, string entityReference, string repositoryReference)
        {
            switch (folder.ToUpper())
            {
                case "REPOSITORY":
                    return await Task.Run(() =>
                    {
                        return $"using {entityReference};\n "
                                  + $"  using {repositoryReference};\n "
                                  + " \n"
                                  + $"  namespace {namespaceString} \n"
                                  + "  { \n"
                                  + $"      public class {className}Repository : GenericRepository<{entity}>, I{className}Repository \n"
                                  + "      {  \n"
                                  + $"          public {className}Repository({domain}Context context) : base(context)  \n"
                                  + "          {  \n"
                                  + "          }  \n"
                                  + "      }  \n"
                                  + "  }  \n";
                    });

                case "SERVICE":
                    return await Task.Run(() =>
                    {
                        return $"using {domain}.API.Entities;  \n"
                                + $"  using {repositoryReference};  \n"
                                + $"  using {domain}.API.Interfaces.Service;  \n"
                                + $"  \n"
                                + $"  namespace {namespaceString}  \n"
                                + "  { \n "
                                + $"      public class {className}Service : GenericService<{entity}>, I{className}Service  \n"
                                + "      {  \n"
                                + $"          private readonly IGenericRepository<{entity}> _repository;  \n"
                                + $"          public {className}Service(IGenericRepository<{entity}> repository) : base(repository)  \n"
                                + "          {  \n"
                                + $"              _repository = repository;  \n"
                                + "          } \n "
                                + "      }  \n"
                                + "  } \n";
                    });
                default:
                    return null;
            }
            
        }
    }
}
