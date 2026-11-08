using DiGi.GIS.PostgreSQL.Classes;
using DiGi.GIS.WebAPI.Classes;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiGi.GIS.WebAPI
{
    public static partial class Modify
    {
        /// <summary>
        /// Initializes the GIS PostgreSQL Web API services, including the configuration file watcher and converter manager.
        /// </summary>
        /// <param name="serviceCollection">The <see cref="Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add services to.</param>
        /// <returns>A <see cref="System.Threading.Tasks.Task" /> representing the asynchronous operation.</returns>
        public static async Task InitializeAsync(this IServiceCollection serviceCollection)
        {
            if (serviceCollection is null)
            {
                return;
            }

            GISWebAPIConfigurationFileWatcher GISWebAPIConfigurationFileWatcher = Create.GISWebAPIConfigurationFileWatcher();
            if (GISWebAPIConfigurationFileWatcher is not null)
            {
                serviceCollection.AddSingleton(GISWebAPIConfigurationFileWatcher);
            }

            GISPostgreSQLConverterManager? gISPostgreSQLConverterManager = PostgreSQL.Create.GISPostgreSQLConverterManager();
            if (gISPostgreSQLConverterManager is not null)
            {
                List<PostgreSQL.Interfaces.IGISPostgreSQLConverter> gISPostgreSQLConverters = gISPostgreSQLConverterManager.GetPostgreSQLConverters<PostgreSQL.Interfaces.IGISPostgreSQLConverter>();
                if (gISPostgreSQLConverters is not null)
                {
                    foreach (PostgreSQL.Interfaces.IGISPostgreSQLConverter gISPostgreSQLConverter in gISPostgreSQLConverters)
                    {
                        serviceCollection.AddSingleton(gISPostgreSQLConverter.GetType(), gISPostgreSQLConverter);
                    }
                }
            }
        }
    }
}