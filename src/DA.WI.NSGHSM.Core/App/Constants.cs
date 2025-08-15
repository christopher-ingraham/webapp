namespace DA.WI.NSGHSM.Core.App
{
    public static class Constants
    {
        public static class Api
        {
            public const string DAWITemplateVersion = "1.0.0.0";

            public static class Policies
            {
                public const string CorsDefault = "CorsDefault";
                // Enable it on controller/actions used for upload/download content
                public const string CorsWithContentDisposition = "CorsWithContentDisposition";
                public const string WhitelistAuthorization = "WhitelistAuthorization";
            }
        }
    }
}
