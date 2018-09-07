using System;
using System.Collections.Generic;
using Microsoft.Win32;

namespace Registrar
{
    public class Settings
    {
        // Registry Tree
        // Node in the tree is a Key
        // Keys have Subkeys and Values
        // Subkeys have Values

        // So I need the Keyname, Subkeyname (optional), to create the registry instance.
        string _rootKey = null;
        string _registryString = null;

        public List<Option> SettingsArray = new List<Option>();

        public Settings(string root_key, string sub_key)
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

            foreach (Option option in SettingsArray)
            {
                bool option_valid = option.RunValidators();
                if (!option_valid)
                {
                    throw new RegistryOptionException("Criteria was not met for option " + option.GetKeyName()); // Reason is needed
                }
                Registry.SetValue(_registryString, option.GetKeyName(), option.Value);
            }
        }
    }
}
