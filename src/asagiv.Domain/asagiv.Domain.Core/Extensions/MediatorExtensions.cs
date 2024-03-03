using asagiv.Domain.Core.DependencyInjection;
using Autofac;
using System.Collections.Generic;
using System.Reflection;

namespace asagiv.Domain.Core.Extensions
{
    public static class MediatorExtensions
    {
        public static void AddMediatR(this ContainerBuilder containerBuilder, IEnumerable<Assembly> assemblies = null)
        {
            if(assemblies == null)
            {
                assemblies = AssemblyExtensions.GetAssembliesForCurrentDomain();
            }

            containerBuilder.RegisterModule(new MediatrModule(assemblies));
        }
    }
}
