using System.Linq;
using System.Reflection;
using System;
using System.IO;

namespace asagiv.Domain.Core.Extensions
{
    public static class AssemblyExtensions
    {
        public static Assembly[] GetAssembliesForCurrentDomain(params string[] importAssemblyDirectories)
        {
            var domainAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            var externalFiles = importAssemblyDirectories
                .SelectMany(x => Directory.GetFiles(x))
                .Where(x => x.EndsWith(".dll") || x.EndsWith(".exe"))
                .ToArray();

            var assemblies = externalFiles
                .Select(Assembly.LoadFrom)
                .Concat(domainAssemblies)
                .ToArray();

            return assemblies;
        }
    }
}
