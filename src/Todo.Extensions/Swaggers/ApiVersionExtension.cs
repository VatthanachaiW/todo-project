using System;

namespace Todo.Extensions.Swaggers
{
    public static class ApiVersionExtension
    {
        public static string VersionFormatter(this string versionString)
        {
            // fieldCount format : major.minor.build.revision
            var version = versionString.Length == 1 ? new Version($"{versionString}.0") : new Version(versionString);

            if (version.Build > 0 && version.Minor > 0 && version.Major > 0 && version.Revision > 0)
                return $"v{version.ToString(4)}";
            if (version.Build > 0 && version.Minor > 0 && version.Major > 0)
                return $"v{version.ToString(3)}";
            if (version.Build > 0 && version.Minor > 0)
                return $"v{version.ToString(2)}";
            return $"v{version}";
        }
    }
}