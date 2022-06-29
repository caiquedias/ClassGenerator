using ClassGenerator_BETA_.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassGenerator_BETA_.Service
{
    public class APIGatewayControllerGenerator : IAPIGatewayControllerGenerator
    {
        public async Task<string> Generate(string domain, string methods)
        {
            return await Task.Run(() =>
            {
                TextReader tr = new StreamReader(@"Templates\APIGatewayControllerTemplate.txt");
                string myText = tr.ReadToEnd();

                myText = myText
                    .Replace("[DOMAIN]", domain)
                    .Replace("[METHODS]", methods);

                return myText;
            });
        }
    }
}
