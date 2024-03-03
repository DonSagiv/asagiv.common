using System;

namespace asagiv.Domain.Core.DependencyInjection
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class ExportMetadataAttribute : Attribute
    {
        #region Properties
        public string Key { get; }
        public object Value { get; }
        #endregion

        #region Constructor
        public ExportMetadataAttribute(string key, object value)
        {
            Key = key;
            Value = value;
        }
        #endregion
    }
}
