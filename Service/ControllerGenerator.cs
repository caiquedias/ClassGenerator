using ClassGenerator_BETA_.Interfaces.Service;
using System.IO;
using System.Threading.Tasks;

namespace ClassGenerator_BETA_.Service
{
    public class ControllerGenerator : IControllerGenerator
    {
        public async Task<string> Generate(string className, string domain, string folder)
        {
            return await Task.Run(() =>
            {
                TextReader tr = new StreamReader(@"Templates\ControllerTemplate.txt");
                string myText = tr.ReadToEnd();

                myText = myText
                    .Replace("[DOMAIN]", domain)
                    .Replace("[CLASSNAME]", className)
                    .Replace("[CLASSNAMELOWERCASE]", className.ToLower())
                    .Replace("[FOLDER]", folder);

                return myText;
            });
        }
    }
}
