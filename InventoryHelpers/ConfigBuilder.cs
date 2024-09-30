using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryHelpers
{
    public sealed class ConfigBuilder
    {
        private static readonly object _instanceLock = new object();
        private static ConfigBuilder? _instance;
        public static ConfigBuilder Instance
        {
            get
            {
                lock (_instanceLock)
                {
                    _instance ??= new ConfigBuilder();
                }
                return _instance;
            }
        }

        private static IConfigurationRoot _configRoot;
        public static IConfigurationRoot ConfigurationRoot
        {
            get
            {
                if (_configRoot == null)
                {
                    ConfigBuilder throwAway = Instance;
                }
                return _configRoot;
            }
        }

        private ConfigBuilder()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            _configRoot = builder.Build();
        }
    }
}
