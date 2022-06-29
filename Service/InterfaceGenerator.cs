using ClassGenerator_BETA_.Interfaces.Service;
using System.Threading.Tasks;

namespace ClassGenerator_BETA_.Service
{
    public class InterfaceGenerator : IInterfaceGenerator
    {
        public async Task<string> Generate(string className, string domain, string namespaceString, string entitie, string folder, string entitieReference)
        {
            return await Task.Run(() =>
            {
                return $"using {entitieReference};  \n"
                    + $"namespace {namespaceString}  \n"
                    + "{  \n"
                    + $"     public interface I{className}{folder} : IGeneric{folder}<{entitie}>  \n"
                    + "      {  \n"
                    + "      }  \n"
                    + "}  \n";
            });
        }
    }
}
