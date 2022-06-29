using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassGenerator_BETA_.Interfaces.Service
{
    public interface IClassContentGenerator
    {
        Task<string> Generate(string className, string domain, string namespaceString, string entity, string folder, string entityReference, string repositoryReference);
    }
}
