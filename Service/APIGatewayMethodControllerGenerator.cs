using ClassGenerator_BETA_.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassGenerator_BETA_.Service
{
    public class APIGatewayMethodControllerGenerator : IAPIGatewayMethodControllerGenerator
    {
        public async Task<string> Generate(string className, string domain)
        {
            return await Task.Run(() =>
            {
                TextReader tr = new StreamReader(@"Templates\APIGatewayMethodControllerTemplate.txt");
                string myText = tr.ReadToEnd();

                myText = myText
                    .Replace("[DOMAIN]", domain)
                    .Replace("[CLASSNAME]", className);

                return myText;
            });
        }
    }
}
