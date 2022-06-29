using ClassGenerator_BETA_.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassGenerator_BETA_.Service
{
    public class GenericClassGenerator : IGenericClassGenerator
    {
        public async Task<string> Generate(string domain, string template)
        {
            return await Task.Run(() =>
            {
                TextReader tr = new StreamReader(@$"Templates\{template}.txt");
                string myText = tr.ReadToEnd();

                myText = myText
                    .Replace("[DOMAIN]", domain);

                return myText;
            });
        }
    }
}
