using Autofac;
using Autofac.Builder;
using Autofac.Features.AttributeFilters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace asagiv.Domain.Core.Extensions
{
    public static class ComponentBuilderExtensions
    {
        public static void AddTransient<TBase, TDerived>(this ContainerBuilder builder, object contractKey = null, params (string, object)[] metadata)
            where TDerived : TBase
        {
            builder.RegisterType<TDerived>()
                .CreateExportFrom(typeof(TBase), contractKey, GetMetadataDictionary(metadata));
        }

        public static void AddSingleton<TBase, TDerived>(this ContainerBuilder builder, object contractkey = null, params (string, object)[] metadata)
            where TDerived : TBase
        {
            builder.RegisterType<TDerived>()
                .CreateExportFrom(typeof(TBase), contractkey, GetMetadataDictionary(metadata))
                .SingleInstance();
        }

        private static IDictionary<string, object> GetMetadataDictionary((string, object)[] input)
        {
            return input.ToDictionary(x => x.Item1, x => x.Item2);
        }

        public static IRegistrationBuilder<TDerived, TReflectionActivatorData, TRegistrationStyle> CreateExportFrom<TDerived, TReflectionActivatorData, TRegistrationStyle>(this IRegistrationBuilder<TDerived, TReflectionActivatorData, TRegistrationStyle> regBuilder,
            Type baseType,
            object contractKey,
            IDictionary<string, object> metadataDictionary)
            where TReflectionActivatorData : ReflectionActivatorData
        {
            if (contractKey == null)
            {
                regBuilder.As(baseType);
            }
            else if (contractKey is string contractName)
            {
                regBuilder.Named(contractName, baseType);
            }
            else
            {
                regBuilder.Keyed(contractKey, baseType);
            }

            foreach (var metadata in metadataDictionary)
            {
                regBuilder.WithMetadata(metadata.Key, metadata.Value);
            }

            regBuilder.WithAttributeFiltering();

            return regBuilder;
        }
    }
}