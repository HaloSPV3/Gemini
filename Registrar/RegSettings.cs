using System;
using System.Collections.Generic;
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
            foreach (KeyValuePair<string, RegOption> kvp in _settings)
            {
                string subKeys = kvp.Value.GetSubKeys();
                string keyPath = _registryString;

                if (subKeys != null)
                {
                    keyPath += subKeys;
                }

                Object keyValue;
                try
                {
                    keyValue = Registry.GetValue(keyPath, kvp.Value.GetKeyName(), kvp.Value);
                    kvp.Value.OptionValue = keyValue ??
                        throw new RegistryLoadException(String.Format("The registry key {0} at node {1} was not found. " +
                        "Either the node doesn't exist, or the key doesn't exist at the node. " +
                        "if caught, call SaveSettings to repopulate the node. " +
                        "The setting will use the default value if caught as well.", kvp.Value.GetKeyName(), keyPath));
                }
                catch (FormatException)
                {
                    throw new RegistryLoadException(String.Format("Failed when loading setting {0}: " +
                        "The format for the entry in the registry was wrong. " +
                        "(EG: attempting to convert a string entry 'abc' to a float). " +
                        "The option will keep its default value if caught.", kvp.Value.GetKeyName()));
                }
            }
        }

        public void SaveSettings() // Save the settings dict values to the registry
        {
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
