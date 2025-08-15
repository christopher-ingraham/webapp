using System.Collections.Generic;

namespace DA.WI.NSGHSM.Core.App
{
    public class Configuration
    {
        public string EnvironmentName { get; set; }
        public string RootFolder { get; set; }
        public ApplicationConfig Application { get; set; }
        public CorsConfig Cors { get; set; }
        public IdentityServerConfig IdentityServer { get; set; }
        public DataSourcesConfig DataSources { get; set; }
        public LogConfig Log { get; set; }
        public EndpointsConfig Endpoints { get; set; }

        public DevelopmentConfig Development { get; set; }

        public class ApplicationConfig
        {
            public string Title { get; set; }
            public string Name { get; set; }

            public string Description { get; set; }
            public string Customer { get; set; }
            public string Copyright { get; set; }

            public LocalesConfig Locales { get; set; }

            public string Version { get; set; }

            public string TemplateVersion { get; set; }

            public string Environment { get; set; }

            public FooterConfig Footer { get; set; }

            public MenuItemConfig[] Menu { get; set; }

            public class FooterConfig
            {
                public string TextKey { get; set; }
                public bool IsVisible { get; set; }
            }

            public class LocalesConfig : Dictionary<string, LocaleConfig>
            {
            }

            public class LocaleConfig
            {
                public string Description { get; set; }
                public bool IsDefault { get; set; }

            }

            public class MenuItemConfig
            {
                public string Key { get; set; }
                public string Default { get; set; }
                public string Icon { get; set; }
                public string Path { get; set; }
                public string[] Roles { get; set; }
                public MenuItemConfig[] Children { get; set; }
            }
        }

        public class CorsConfig
        {
            public string[] AllowedOrigins { get; set; }
        }

        public class IdentityServerConfig
        {
            public string Authority { get; set; }
            public string Audience { get; set; }
        }

        public class DevelopmentConfig
        {
            public SqliteConfig Sqlite { get; set; }

            public class SqliteConfig
            {
                public string DbFileName { get; set; }
                public bool AlwaysRecreate { get; set; }
            }

        }

        public class DataSourcesConfig : Dictionary<string, DataSourceConfig>
        {
        }

        public class DataSourceConfig
        {
            public ProviderType Provider { get; set; }
            public string ConnectionString { get; set; }

            public string InitScriptPath { get; set; }
            public InitModeType InitMode { get; set; }

            public enum ProviderType
            {
                Sqlite = 0,
                SqlServer,
                Oracle
            }

            public enum InitModeType
            {
                Never = 0,
                IfNotExists,
                Always
            }
        }

        public class LogConfig
        {
            public bool IsLogActionStarting { get; set; }
            public bool IsLogActionFinishing { get; set; }
        }

        public class EndpointsConfig
        {
            public string[] WebUrl { get; set; }
            public string IdentityServerUrl { get; set; }
        }
    }
}
