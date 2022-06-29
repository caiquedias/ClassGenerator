using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassGenerator_BETA_.DTO
{
    public class TranslateResultDTO
    {
        public Data data { get; set; }
    }
    public class Data
    {
        public List<Translation> translations { get; set; }
    }
    public class Translation
    {
        public string translatedText { get; set; }
    }

}
