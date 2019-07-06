using System;
using System.IO;
using System.Reflection;

namespace IdentityModel.UnitTests
{
    static class ApplicationBasePath
    {
#if NET45 || NET461
        private static readonly string _assemblyDirectory;

        static ApplicationBasePath()
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            var directoryAssembly = entryAssembly ?? Assembly.GetExecutingAssembly();
            const string schemaFile = @"file:\";
            var path = Path.GetDirectoryName(directoryAssembly.CodeBase);
            if (path.StartsWith(schemaFile))
                path = path.Remove(0, schemaFile.Length);
            _assemblyDirectory = path;
        }
#endif

        public static string GetApplicationBasePath()
        {
#if NETCOREAPP2_1 || NETCOREAPP2_2
            return System.AppContext.BaseDirectory;
#else
            return _assemblyDirectory;
#endif
        }
    }
}
