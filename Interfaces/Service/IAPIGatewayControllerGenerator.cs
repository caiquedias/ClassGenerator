using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassGenerator_BETA_.Interfaces.Service
{
    public interface IAPIGatewayControllerGenerator
    {
        Task<string> Generate(string domain, string methods);
    }
}
