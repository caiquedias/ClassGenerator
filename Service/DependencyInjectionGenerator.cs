using ClassGenerator_BETA_.DTO;
using ClassGenerator_BETA_.Interfaces.Service;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ClassGenerator_BETA_.Service
{
    public class DependencyInjectionGenerator : IDependencyInjectionGenerator
    {
        public async Task<string> Generate(IEnumerable<TablesBySchemaDTO> tables)
        {
            return await Task.Run(() =>
            {
                TextReader trr = new StreamReader(@"Templates\DependecyInjectionRepositoryTemplate.txt");
                string myRepositoryText = trr.ReadToEnd();
                TextReader trs = new StreamReader(@"Templates\DependecyInjectionServiceTemplate.txt");
                string myServiceText = trs.ReadToEnd();

                var DIRepositoryResultText = string.Empty;
                var DIServiceResultText = string.Empty;

                DIRepositoryResultText += " #region Repositories \n ";
                DIRepositoryResultText += " services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>)); \n ";
                foreach (var item in tables)
                {
                    DIRepositoryResultText += myRepositoryText
                    .Replace("[CLASSNAME]", item.ClassName)
                    .Replace("[REPOSITORYFOLDER]", "Repository");

                    DIRepositoryResultText += "\n";
                }
                DIRepositoryResultText += " #endregion Repositories ";

                DIServiceResultText += " #region Services \n ";
                DIServiceResultText += " services.AddTransient(typeof(IGenericService<>), typeof(GenericService<>)); \n ";
                foreach (var item in tables)
                {
                    DIServiceResultText += myServiceText
                    .Replace("[CLASSNAME]", item.ClassName)
                    .Replace("[SERVICEFOLDER]", "Service");

                    DIServiceResultText += "\n";
                }
                DIServiceResultText += " #endregion Services \n\n ";



                return DIRepositoryResultText += "\n\n" + DIServiceResultText;
            });
        }
    }
}
