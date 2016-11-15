using Contracts;
using System.Configuration;

namespace Infrastructure
{
    public class ConfigurationService : IConfigurationService
    {
        public string InstanceStore => ConfigurationManager.ConnectionStrings["PersistanceDatabase"].ConnectionString;
    }
}
