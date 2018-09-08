using System;
using System.Collections.Generic;
using Microsoft.Win32;

namespace Registrar
{
    public class RegSettings
    {
        // Registry Tree
        // Node in the tree is a Key
        // Keys have Subkeys and Values
        // Subkeys have Values

        // So I need the Keyname, Subkeyname (optional), to create the registry instance.
        string _rootKey = null;
        string _registryString = null;
        
        public Dictionary<string, RegOption> SettingsDict = new Dictionary<string, RegOption>();

        public RegSettings(string root_key, string sub_key)
        {
            _rootKey = root_key;
            _registryString = root_key + "\\" + sub_key;
        }

        public void LoadSettings() // Load settings from the registry instance
        {
            if (_registryString == null)
            {
                throw new RegistryNotSetException("The registry string is null. Did you instantiate the settings object correctly?");
            }

        }

        public void SaveSettings() // Save the settings array values to the registry
        {
            if (_registryString == null)
            {
                throw new RegistryNotSetException("The registry string is null. Did you instantiate the settings object correctly?");
            }

            foreach (KeyValuePair<string, RegOption> kvp in SettingsDict)
            {
                ValidationResponse validation_result = kvp.Value.Validate();
                if (!validation_result.Successful)
                {
                    throw new RegistryOptionException("Criteria was not met for option: " + kvp.Value.GetKeyName() + " Reason: " + validation_result.Information);
                }
                Registry.SetValue(_registryString, kvp.Value.GetKeyName(), kvp.Value.OptionValue);
            }
        }
    }
}
