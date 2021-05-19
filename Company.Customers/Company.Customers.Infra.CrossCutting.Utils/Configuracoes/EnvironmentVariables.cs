using DotNetEnv;
using System;
using System.IO;
using System.Linq;

namespace Company.Customers.Infra.CrossCutting.Utils.Configuracoes
{
    public static class EnvironmentVariables
    {
        public static void CarregarVariaveis()
        {
            const string fileName = ".env";

            ConfigureEnvironmentVariables(fileName);
            var diretorio = AppDomain.CurrentDomain.BaseDirectory;
            var arquivo = Path.Combine(diretorio, fileName);
            if (File.Exists(arquivo))
            {
                Env.Load(arquivo);
            }
        }

        private static void ConfigureEnvironmentVariables(in string fileName)
        {
            var caminho = GetDirectory(fileName)?.FullName ?? string.Empty;
            var arquivo = Path.Combine(caminho, fileName);

            var directoryReference = AppDomain.CurrentDomain.BaseDirectory;
            var fileDirectory = Path.Combine(directoryReference, fileName);

            if (!File.Exists(arquivo)) 
                return;

            var streamReader = File.OpenText(arquivo);
            var fileString = streamReader.ReadToEnd();
            streamReader.Close();


            var fileInfo = new FileInfo(fileDirectory);
            var streamWriter = fileInfo.CreateText();
            streamWriter.Write(fileString);
            streamWriter.Close();
        }

        public static DirectoryInfo GetDirectory(in string fileName)
        {
            var directory = Directory.GetParent(Directory.GetCurrentDirectory());
            while (directory != null && !directory.GetFiles(fileName).Any())
            {
                directory = directory.Parent;
            }

            return directory;
        }
    }
}
