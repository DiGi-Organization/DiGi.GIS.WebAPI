namespace DiGi.GIS.WebAPI.Classes
{
    /// <summary>
    /// Provides functionality to watch and retrieve configuration settings for the GIS PostgreSQL Web API from a specified configuration file.
    /// </summary>
    public class GISWebAPIConfigurationFileWatcher : Core.IO.FileWatcher.Classes.ConfigurationFileWatcher
    {
        /// <summary>
        /// Initializes a new instance of the GISWebAPIConfigurationFileWatcher class.
        /// </summary>
        /// <param name="path">The path to the configuration file to be watched.</param>
        /// <param name="interval">The time interval in milliseconds between checks for changes to the configuration file.</param>
        public GISWebAPIConfigurationFileWatcher(string path, double interval = 5000)
            : base(path, interval)
        {
        }

        /// <summary>
        /// Gets a value indicating whether write authorization enforcement is enabled.
        /// <para>False denies every write request; it does not open them.</para>
        /// </summary>
        public bool Enabled
        {
            get
            {
                return ConfigurationFile.GetValue<bool>(nameof(Enabled), defaultValue: !string.IsNullOrWhiteSpace(Key));
            }
        }

        /// <summary>
        /// Gets the secret access token for authorizing write operations, or the value of the DIGI_GIS_WEBAPI_KEY environment variable if not configured in the file.
        /// </summary>
        public string? Key
        {
            get
            {
                string? key = ConfigurationFile.GetValue<string>(nameof(Key));
                if (!string.IsNullOrWhiteSpace(key))
                {
                    return key;
                }

                return System.Environment.GetEnvironmentVariable("DIGI_GIS_WEBAPI_KEY");
            }
        }

        /// <summary>
        /// Gets a value indicating whether the key check is explicitly waived, leaving write endpoints reachable without an access key.
        /// <para>Intended for local development only. This is the sole setting that grants unauthenticated access to write endpoints.</para>
        /// </summary>
        public bool Open
        {
            get
            {
                return ConfigurationFile.GetValue<bool>(nameof(Open), defaultValue: false);
            }
        }

        /// <summary>
        /// Gets a value indicating whether updates to PRG address points are permitted according to the configuration file.
        /// </summary>
        public bool AllowUpdateAddressPoint
        {
            get
            {
                return ConfigurationFile.GetValue<bool>(nameof(AllowUpdateAddressPoint));
            }
        }

        /// <summary>
        /// Gets a value indicating whether updates to administrative areal 2D data are permitted according to the configuration file.
        /// </summary>
        public bool AllowUpdateAdministrativeAreal2D
        {
            get
            {
                return ConfigurationFile.GetValue<bool>(nameof(AllowUpdateAdministrativeAreal2D));
            }
        }

        /// <summary>
        /// Gets a value indicating whether updates to 2D buildings are permitted based on the configuration file settings.
        /// </summary>
        public bool AllowUpdateBuilding2D
        {
            get
            {
                return ConfigurationFile.GetValue<bool>(nameof(AllowUpdateBuilding2D));
            }
        }

        /// <summary>
        /// Gets a value indicating whether updates to building models are permitted based on the configuration file settings.
        /// </summary>
        public bool AllowUpdateBuildingModel
        {
            get
            {
                return ConfigurationFile.GetValue<bool>(nameof(AllowUpdateBuildingModel));
            }
        }

        /// <summary>
        /// Gets a value indicating whether updating EPW file data is enabled in the configuration.
        /// </summary>
        public bool AllowUpdateEPWFile
        {
            get
            {
                return ConfigurationFile.GetValue<bool>(nameof(AllowUpdateEPWFile));
            }
        }

        /// <summary>
        /// Gets a value indicating whether updates to occupancy data are permitted according to the configuration file.
        /// </summary>
        public bool AllowUpdateOccupancyData
        {
            get
            {
                return ConfigurationFile.GetValue<bool>(nameof(AllowUpdateOccupancyData));
            }
        }

        /// <summary>
        /// Gets a value indicating whether updates to orthophoto data are permitted according to the configuration file.
        /// </summary>
        public bool AllowUpdateOrtoDatas
        {
            get
            {
                return ConfigurationFile.GetValue<bool>(nameof(AllowUpdateOrtoDatas));
            }
        }

        /// <summary>
        /// Gets a value indicating whether updating year built data is enabled in the configuration.
        /// </summary>
        public bool AllowUpdateYearBuiltData
        {
            get
            {
                return ConfigurationFile.GetValue<bool>(nameof(AllowUpdateYearBuiltData));
            }
        }

        /// <summary>
        /// Gets a value indicating whether stored year built data may be deleted - whole objects, one prediction run, or user entries withdrawn by moderation.
        /// <para>Kept apart from <see cref="AllowUpdateYearBuiltData"/> because a delete has no undo: a host can accept new predictions and user entries while refusing removals. A missing value denies.</para>
        /// </summary>
        public bool AllowDeleteYearBuiltData
        {
            get
            {
                return ConfigurationFile.GetValue<bool>(nameof(AllowDeleteYearBuiltData));
            }
        }

        /// <summary>
        /// Gets a value indicating whether updates to buildings are permitted based on the configuration file settings.
        /// </summary>
        public bool AllowUpdateBuilding
        {
            get
            {
                return ConfigurationFile.GetValue<bool>(nameof(AllowUpdateBuilding));
            }
        }

        /// <summary>
        /// Gets a value indicating whether updates to building data are permitted based on the configuration file settings.
        /// </summary>
        public bool AllowUpdateBuildingData
        {
            get
            {
                return ConfigurationFile.GetValue<bool>(nameof(AllowUpdateBuildingData));
            }
        }

        /// <summary>
        /// Gets a value indicating whether signed-in users may store the solar radiation of a building (<c>gis/buildingsolarradiation/setsolarradiation</c>).
        /// <para>The write is authorized by the user's bearer token, not by the access key, so this flag is the host's only switch for it. A missing value denies.</para>
        /// </summary>
        public bool AllowUpdateSolarRadiationData
        {
            get
            {
                return ConfigurationFile.GetValue<bool>(nameof(AllowUpdateSolarRadiationData));
            }
        }

        /// <summary>
        /// Gets a value indicating whether updates to territorial unit data are permitted according to the configuration file.
        /// </summary>
        public bool AllowUpdateUnit
        {
            get
            {
                return ConfigurationFile.GetValue<bool>(nameof(AllowUpdateUnit));
            }
        }
    }
}