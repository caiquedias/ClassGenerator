using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassGenerator_BETA_.Interfaces.Service
{
    public interface IControllerGenerator
    {
        Task<string> Generate(string className, string domain, string folder);
    }
}
