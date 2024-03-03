using Autofac;
using Autofac.Features.Metadata;
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using asagiv.Domain.Core.Extensions;

namespace asagiv.Domain.Core.DependencyInjection
{
    public class ComponentContainer
    {
        #region Statics
        private static readonly Lazy<ComponentContainer> _lazyInstance = new Lazy<ComponentContainer>(() => new ComponentContainer());
        public static ComponentContainer Container => _lazyInstance.Value;
        #endregion

        #region Fields
        private readonly object _syncObject = new object();
        private IContainer _container;
        #endregion

        #region Constructor
        private ComponentContainer() { }
        #endregion

        #region Methods
        public void Initialize(Action<ContainerBuilder> containerBuilderAction = null, params string[] importAssemblyDirectories)
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilderAction?.Invoke(containerBuilder);

            var assemblies = AssemblyExtensions.GetAssembliesForCurrentDomain(importAssemblyDirectories)
                .Where(x => x.FullName.Contains("asagiv"))
                .ToArray();

            var assemblyTypes = GetExportedAssemblyTypes(assemblies);

            foreach (var (type, attribute) in assemblyTypes)
            {
                ImportFromExportAttributedType(containerBuilder, type, attribute);
            }

            lock (_syncObject)
            {
                _container = containerBuilder.Build();
            }
        }

        private (Type type, ExportAttribute attribute)[] GetExportedAssemblyTypes(IEnumerable<Assembly> assemblies)
        {
            return assemblies
                .SelectMany(GetExportsFromAssembly)
                .ToArray();
        }

        private IEnumerable<(Type type, ExportAttribute attribute)> GetExportsFromAssembly(Assembly assembly)
        {
            try
            {
                var allTypes = assembly
                    .GetTypes();

                var types = allTypes.SelectMany(x => GetExports(x), (x, a) => (x, a))
                    .Where(x => x.a != null)
                    .ToArray();

                return types;
            }
            catch
            {
                return Enumerable.Empty<(Type, ExportAttribute)>();
            }
        }

        private static IEnumerable<ExportAttribute> GetExports(Type x)
        {
            IEnumerable<ExportAttribute> attributesToReturn;

            try
            {
                attributesToReturn = x.GetCustomAttributes<ExportAttribute>();
            }
            catch (Exception ex)
            {
                yield break;
            }

            foreach (var attribute in attributesToReturn)
            {
                yield return attribute;
            }
        }

        private void ImportFromExportAttributedType(ContainerBuilder builder, Type derivedType, ExportAttribute exportAttribute)
        {
            var metadataDictionary = derivedType
                .GetCustomAttributes<ExportMetadataAttribute>()
                .ToDictionary(x => x.Key, x => x.Value);

            var regBuilder = builder.RegisterType(derivedType)
                .CreateExportFrom(exportAttribute.ExportType, exportAttribute.ContractKey, metadataDictionary);

            if (exportAttribute.CreationPolicy == CreationPolicy.Singleton)
            {
                regBuilder.SingleInstance();
            }
        }

        public T Build<T>()
        {
            lock (_syncObject)
            {
                return _container.Resolve<T>();
            }
        }

        public T Build<T>(object contractKey)
        {
            if (contractKey is string contractName)
            {
                lock (_syncObject)
                {
                    return _container.ResolveNamed<T>(contractName);
                }
            }
            else
            {
                lock (_syncObject)
                {
                    return _container.ResolveKeyed<T>(contractKey);
                }
            }
        }

        public T Build<T>(string key, object value)
        {
            lock (_syncObject)
            {
                return _container
                    .Resolve<IEnumerable<Meta<T>>>()
                    .First(x => FilterByMetadataValue(x, key, value))
                    .Value;
            }
        }

        public IEnumerable<T> BuildMany<T>()
        {
            lock (_syncObject)
            {
                return _container.Resolve<IEnumerable<T>>();
            }
        }

        public IEnumerable<T> BuildMany<T>(object contractKey)
        {
            if (contractKey is string contractName)
            {
                lock (_syncObject)
                {
                    return _container.ResolveNamed<IEnumerable<T>>(contractName);
                }
            }
            else
            {
                lock (_syncObject)
                {
                    return _container.ResolveKeyed<IEnumerable<T>>(contractKey);
                }
            }
        }

        public IEnumerable<T> BuildMany<T>(string key, object value)
        {
            lock (_syncObject)
            {
                return _container.Resolve<IEnumerable<Meta<T>>>()
                    .Where(x => FilterByMetadataValue(x, key, value))
                    .Select(x => x.Value);
            }
        }

        private static bool FilterByMetadataValue<T>(Meta<T> exportMeta, string key, object value)
        {
            if (value is string valueString)
            {
                return exportMeta.Metadata[key].ToString() == valueString;
            }
            else
            {
                return exportMeta.Metadata[key] == value;
            }
        }
        #endregion
    }
}