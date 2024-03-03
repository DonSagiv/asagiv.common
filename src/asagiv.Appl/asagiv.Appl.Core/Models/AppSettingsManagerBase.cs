using asagiv.Appl.Core.Exceptions;
using asagiv.Appl.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace asagiv.Appl.Core.Models
{
    public abstract class AppSettingsManagerBase : IAppSettingsManager
    {
        #region Properties
        public IReadOnlyDictionary<string, object> Settings { get; private set; }
        #endregion

        #region Constructor
        protected AppSettingsManagerBase()
        {
            ApplySettings();
        }
        #endregion

        #region Methods
        private void ApplySettings()
        {
            var settingsDictionary = new Dictionary<string, object>();

            ApplySettingsOverride(settingsDictionary);

            Settings = new ReadOnlyDictionary<string, object>(settingsDictionary);
        }

        public T Get<T>(string key)
        {
            var value = Settings[key];

            if (value is T typeValue)
            {
                return typeValue;
            }

            throw new InvalidTypeArgumentException($"Invalid type for key {key}: Expected type: {typeof(T)}, actual type = {value.GetType()}");
        }

        public bool TryGet<T>(string key, out T output)
        {
            var value = Settings[key];

            if (value is T typeValue)
            {
                output = typeValue;

                return true;
            }

            output = default;

            return false;
        }

        protected abstract void ApplySettingsOverride(IDictionary<string, object> settingsDictionary);
        #endregion
    }
}
