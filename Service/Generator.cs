using ClassGenerator_BETA_.DTO;
using ClassGenerator_BETA_.Interfaces.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ClassGenerator_BETA_.Service
{
    public class Generator : IGenerator
    {
        private readonly IGenericService _service;
        private readonly ITranslateTableNames _translate;
        private readonly IInterfaceGenerator _interfaceGenerator;
        private readonly IClassContentGenerator _classContentGenerator;
        private readonly IControllerGenerator _controllerGenerator;
        private readonly IDependencyInjectionGenerator _dependencyInjectionGenerator;
        private readonly IGenericClassGenerator _genericClassGenerator;
        private readonly IAPIGatewayControllerGenerator _apiGatewayControllerGenerator;
        private readonly IAPIGatewayMethodControllerGenerator _apiGatewayMethodControllerGenerator;

        public Generator(
            IGenericService service,
            ITranslateTableNames translate,
            IInterfaceGenerator interfaceGenerator,
            IClassContentGenerator classContentGenerator,
            IControllerGenerator controllerGenerator,
            IDependencyInjectionGenerator dependencyInjectionGenerator,
            IGenericClassGenerator genericClassGenerator,
            IAPIGatewayControllerGenerator apiGatewayControllerGenerator,
            IAPIGatewayMethodControllerGenerator apiGatewayMethodControllerGenerator)
        {
            _service = service;
            _translate = translate;
            _interfaceGenerator = interfaceGenerator;
            _classContentGenerator = classContentGenerator;
            _controllerGenerator = controllerGenerator;
            _dependencyInjectionGenerator = dependencyInjectionGenerator;
            _genericClassGenerator = genericClassGenerator;
            _apiGatewayControllerGenerator = apiGatewayControllerGenerator;
            _apiGatewayMethodControllerGenerator = apiGatewayMethodControllerGenerator;
        }

        public async Task CreateClass()
        {
            var data = new Dictionary<string, string>()
            {
                {"DominioBeneficiarios", "Beneficiary"},
                {"DominioClientes", "Client"},
                {"DominioContratual", "Contractual"},
                {"DominioPessoas", "Person"},
                {"DominioPlanos", "Plain"},
                {"DominioSubFaturas", "SubInvoice"}
            };

            foreach (var item in data)
            {
                var resultSuccess = await ProcessData(item.Value, item.Key);
                if (resultSuccess)
                {
                    Console.WriteLine($"Classes referente ao domínio '{item.Key}' criado com sucesso!");
                }
            }

            Console.WriteLine($"Processamento finalizado!");
        }

        private async Task<bool> ProcessData(string domain, string schema)
        {
            try
            {
                var repositoryFolder = "Repository";
                var serviceFolder = "Service";
                var controllerFolder = "Controller";

                var tables = await _service.GetTablesBySchema(schema);

                var validateModel = tables.Select(x => new TablesBySchemaDTO
                {
                    ClassName = AddSpacesToSentence(x.TableName.Replace("Dim", "")),
                    SchemaName = domain,
                    TableName = x.TableName,
                    AccessKeyObject = CreateHash(x.TableName)
                });

                var ToTranslate = tables.Select(x => new TablesBySchemaDTO
                {
                    ClassName = AddSpacesToSentence(x.TableName.Replace("Dim", "")),
                    SchemaName = null,
                    TableName = null,
                    AccessKeyObject = CreateHash(x.TableName)
                });

                var textToTranslate = JsonConvert.SerializeObject(ToTranslate);

                TextReader tr = new StreamReader($@"TranslationFiles\{domain}TranslatedDeepLResult.json");
                string myText = tr.ReadToEnd();

                if (string.IsNullOrEmpty(myText))
                {
                    textToTranslate = textToTranslate
                        .Replace("SchemaName", "NomeSchema")
                        .Replace("TableName", "NomeTabela")
                        .Replace("ClassName", "NomeClasse")
                        .Replace("AccessKeyObject", "ChaveAcessoObjeto");
                }

                else
                {
                    myText = myText
                        .Replace("NomeSchema", "SchemaName")
                        .Replace("NomeTabela", "TableName")
                        .Replace("NomeClasse", "ClassName")
                        .Replace("ChaveAcessoObjeto", "AccessKeyObject");

                    ToTranslate = JsonConvert.DeserializeObject<List<TablesBySchemaDTO>>(myText);
                }

                var classList = ToTranslate.Select(x => new TablesBySchemaDTO
                {
                    ClassName = x.ClassName.Replace(" ", ""),
                    SchemaName = validateModel.Where(item => item.AccessKeyObject == x.AccessKeyObject).FirstOrDefault().SchemaName,
                    TableName = validateModel.Where(item => item.AccessKeyObject == x.AccessKeyObject).FirstOrDefault().TableName,
                    AccessKeyObject = x.AccessKeyObject
                });

                //Generate Repository Interfaces
                var newGenericRepositoryInterface = await _genericClassGenerator.Generate(domain, "GenericRepositoryInterfaceTemplate");
                CreateFile(domain, @"Interfaces\Repository", newGenericRepositoryInterface, false, $"IGeneric{repositoryFolder}");

                //Generate Service Interfaces
                var newGenericServiceInterface = await _genericClassGenerator.Generate(domain, "GenericServiceInterfaceTemplate");
                CreateFile(domain, @"Interfaces\Service", newGenericServiceInterface, false, $"IGeneric{serviceFolder}");

                //Generate Repository Interfaces
                foreach (var item in classList)
                {
                    var newInterface = await _interfaceGenerator.Generate(
                        item.ClassName,
                        domain,
                        $"{domain}.API.Interfaces.{repositoryFolder}",
                        item.TableName,
                        repositoryFolder,
                        $"{domain}.API.Entities");

                    CreateFile(domain, @"Interfaces\Repository", newInterface, false, $"I{item.ClassName}{repositoryFolder}");
                }
                //Generate Repository Interfaces
                foreach (var item in classList)
                {
                    var newInterface = await _interfaceGenerator.Generate(
                        item.ClassName,
                        domain,
                        $"{domain}.API.Interfaces.{serviceFolder}",
                        item.TableName,
                        serviceFolder,
                        $"{domain}.API.Entities");

                    CreateFile(domain, @"Interfaces\Service", newInterface, false, $"I{item.ClassName}{serviceFolder}");
                }

                //Generate Repository Interfaces
                var newGenericRepository = await _genericClassGenerator.Generate(domain, "GenericRepositoryTemplate");
                CreateFile(domain, @"Repository", newGenericRepository, false, $"Generic{repositoryFolder}");

                //Generate Service Interfaces
                var newGenericService = await _genericClassGenerator.Generate(domain, "GenericServiceTemplate");
                CreateFile(domain, @"Service", newGenericService, false, $"Generic{serviceFolder}");

                //Generate Repository Class
                foreach (var item in classList)
                {
                    var newRepositoryClass = await _classContentGenerator.Generate(
                        item.ClassName,
                        domain,
                        $"{domain}.API.{repositoryFolder}",
                        item.TableName,
                        repositoryFolder,
                        $"{domain}.API.Entities",
                        $"{domain}.API.Interfaces.{repositoryFolder}");

                    CreateFile(domain, @"Repository", newRepositoryClass, false, $"{item.ClassName}{repositoryFolder}");
                }

                //Generate Service Class
                foreach (var item in classList)
                {
                    var newServiceClass = await _classContentGenerator.Generate(
                        item.ClassName,
                        domain,
                        $"{domain}.API.{serviceFolder}",
                        item.TableName,
                        serviceFolder,
                        $"{domain}.API.Entities",
                        $"{domain}.API.Interfaces.{repositoryFolder}");

                    CreateFile(domain, @"Service", newServiceClass, false, $"{item.ClassName}{serviceFolder}");
                }

                //Generate Controller Class
                foreach (var item in classList)
                {
                    var newControllerClass = await _controllerGenerator.Generate(
                        item.ClassName,
                        domain,
                        serviceFolder);

                    CreateFile(domain, controllerFolder, newControllerClass, false, $"{item.ClassName}{controllerFolder}");
                }

                //Generate Dependency Injection
                var newDependencyInjectionClass = await _dependencyInjectionGenerator.Generate(classList);
                CreateFile(domain, "DependencyInject", newDependencyInjectionClass, false, $"{domain}DependencyInject");

                //Generate APIGateway Method Controller Class
                var domainMethods = string.Empty;
                foreach (var item in classList)
                {
                     domainMethods += 
                        await _apiGatewayMethodControllerGenerator.Generate(
                        item.ClassName,
                        domain);
                }

                //Generate APIGateway Controller Class
                var newAPIGatewayControllerClass = await _apiGatewayControllerGenerator.Generate(domain, domainMethods);
                CreateFile(domain, "APIController", newAPIGatewayControllerClass, false, $"{domain}Controller");

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private string AddSpacesToSentence(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "";
            StringBuilder newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]) && text[i - 1] != ' ')
                    newText.Append(' ');
                newText.Append(text[i]);
            }
            return newText.ToString();
        }

        private void CreateFile(string domain, string directoryFolder, string content, bool isError = false, string fileName = "")
        {
            if (string.IsNullOrEmpty(fileName))
                fileName = $"ClassGenerator_{DateTime.Now.ToString("yyyy-MM-dd HH mm ss")}";
            else
                fileName = $"{fileName}";

            string path = string.Empty;

            if (isError)
                path = @$"C:\Users\caique.araujo\Downloads\log\ClassGenerator\temp\{DateTime.Now.ToString("yyyyMMdd")}\error";
            else
                path = @$"C:\Users\caique.araujo\Downloads\log\ClassGenerator\temp\{DateTime.Now.ToString("yyyyMMdd")}\{domain}\{directoryFolder}";

            bool exists = System.IO.Directory.Exists(path);

            if (!exists)
                System.IO.Directory.CreateDirectory(path);

            path = @$"{path}\{fileName}.cs";

            try
            {
                // Create the file, or overwrite if the file exists.
                using (FileStream fs = File.Create(path))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(content);
                    fs.Write(info, 0, info.Length);

                    fs.Dispose();
                }
            }

            catch (Exception ex)
            {
                throw new Exception(JsonConvert.SerializeObject(ex), ex);
            }
        }

        private string CreateHash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hash = md5.ComputeHash(Encoding.Default.GetBytes(input));
                return new Guid(hash).ToString();
            }
        }
    }
}
