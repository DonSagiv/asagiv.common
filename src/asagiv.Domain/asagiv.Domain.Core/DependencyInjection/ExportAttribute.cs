using System;

namespace asagiv.Domain.Core.DependencyInjection
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class ExportAttribute : Attribute
    {
        #region Properties
        /// <summary>
        /// Type to decorate the attributed class as.
        /// </summary>
        public Type ExportType { get; }
        /// <summary>
        /// A key to define the contract of the export. Default is null.
        /// </summary>
        public object ContractKey { get; }
        /// <summary>
        /// Transient (a.k.a. Factory): New instance is created each time it's called.
        /// Singleton: Same instance is called every time.
        /// </summary>
        public CreationPolicy CreationPolicy { get; }
        #endregion

        public ExportAttribute(Type exportType, object contractKey = null, CreationPolicy creationPolicy = CreationPolicy.Transient)
        {
            ExportType = exportType;
            ContractKey = contractKey;
            this.CreationPolicy = creationPolicy;
        }
    }
}
