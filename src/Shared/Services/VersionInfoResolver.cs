namespace Shared.Services
{
    using System.Diagnostics;

    public static class VersionInfoResolver
    {
        private static readonly Lazy<VersionInfo> version = new(GetVersion);

        public static string Version => version.Value.Version;

        public static string InformationalVersion => version.Value.InformationalVersion;

        private static VersionInfo GetVersion()
        {
            var assembly = typeof(VersionInfoResolver).Assembly;
            var fileVersionInfo = GetFileVersionInfo();

            return new VersionInfo(
                version: fileVersionInfo?.FileVersion ?? assembly.GetName().Version?.ToString() ?? "unknown",
                informationalVersion: fileVersionInfo?.ProductVersion ?? "unknown"
            );
        }

        private static FileVersionInfo? GetFileVersionInfo()
        {
            var location = Environment.ProcessPath;
            if (location == null)
            {
                return null;
            }

            return FileVersionInfo.GetVersionInfo(location);
        }

        private class VersionInfo
        {
            public VersionInfo(string version, string informationalVersion)
            {
                this.Version = version;
                this.InformationalVersion = informationalVersion;
            }

            public string Version { get; }

            public string InformationalVersion { get; }
        }
    }
}
