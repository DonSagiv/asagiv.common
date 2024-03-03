using Autofac;
using MediatR;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace asagiv.Domain.Core.DependencyInjection
{
    internal class MediatrModule : Autofac.Module
    {
        #region Fields
        private readonly Assembly[] _assemblies;
        #endregion

        #region Constructor
        public MediatrModule(IEnumerable<Assembly> assemblies)
        {
            _assemblies = assemblies.ToArray();
        }
        #endregion

        #region Methods
        protected override void Load(ContainerBuilder builder)
        {
            var configuration = MediatRConfigurationBuilder
                .Create(_assemblies)
                .WithAllOpenGenericHandlerTypesRegistered()
                .Build();

            builder.RegisterMediatR(configuration);
        }
        #endregion
    }
}
