using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace Registrar
{
    public class RegSettings
    {
        string _baseKey = null;
        string _registryString = null;
        private Dictionary<string, RegOption> _settings = new Dictionary<string, RegOption>();

        public RegSettings(string base_key, string root_key)
        {
            _baseKey = base_key;
            _registryString = String.Format("{0}\\{1}", base_key, root_key);
            
        }

        public void RegisterSetting(string key_name, RegOption option)
        {
            _settings.Add(key_name, option);
        }

        public Object GetSetting(string key_name)
        {
            return _settings[key_name].OptionValue;
        }

        public void LoadSettings() // Load settings from the registry instance
        {
            if (_registryString == null)
            {
                throw new RegistryNotSetException("The registry string is null. Did you instantiate the settings object correctly?");
            }
            
            foreach (KeyValuePair<string, RegOption> kvp in _settings)
            {
                string subKeys = kvp.Value.GetSubKeys();
                string keyPath = _registryString;

                if (subKeys != null)
                {
                    keyPath += subKeys;
                }

                Object keyValue = Registry.GetValue(keyPath, kvp.Value.GetKeyName(), kvp.Value);
                kvp.Value.OptionValue = keyValue;
            }
        }

        public void SaveSettings() // Save the settings dict values to the registry
        {
            if (_registryString == null)
            {
                throw new RegistryNotSetException("The registry string is null. Did you instantiate the settings object correctly?");
            }

            foreach (KeyValuePair<string, RegOption> kvp in _settings)
            {
                string subKeys = kvp.Value.GetSubKeys();

                ValidationResponse validation_result = kvp.Value.Validate();
                if (!validation_result.Successful)
                {
                    throw new RegistryOptionException(String.Format("Criteria was not met for option: {0} - Reason: {1}", kvp.Value.GetKeyName(), validation_result.Information));
                }

                string keyOut = _registryString;
                if (subKeys != null)
                {
                    keyOut += subKeys;
                }
                Registry.SetValue(keyOut, kvp.Value.GetKeyName(), kvp.Value.OptionValue);
            }
        }
    }
}
