using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace OmnionAPI.Configuration
{
    public class DefinicoesConfiguracao
    {
        private static IConfigurationRoot _configuration;

        private static IConfiguration Instance
        {
            get
            {
                if(_configuration == null)
                {
                    _configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                        .AddJsonFile("appsettings.json", false)
                        .Build();
                }
                return _configuration;
            }
        }

        public static string ObterConexaoBancoDeDados(string nome)
        {
            return Instance.GetSection("ConnectionStrings").GetSection(nome).GetSection("ConnectionString").Value;
        }
    }
}
