using System;
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
        RegistryKey _rootKey = null;
        RegistryKey _registry = null;

        public Option[] SettingsArray = new Option[] { };

        public Settings(RegistryKey root_key, string sub_key)
        {
            _rootKey = root_key;
            _registry = _rootKey.OpenSubKey(sub_key);
        }

        public void LoadSettings() // Load settings from the registry instance
        {
            if (_registry == null)
            {
                throw new RegistryNotSetException("The registry instance is null. Did you instantiate the settings object correctly?");
            }

        }

        public void SaveSettings() // Save the settings array values to the registry
        {
            if (_registry == null)
            {
                throw new RegistryNotSetException("The registry instance is null. Did you instantiate the settings object correctly?");
            }

            foreach (Option option in SettingsArray)
            {
                bool option_valid = option.RunValidators();
                if (!option_valid)
                {
                    throw new RegistryOptionException("Criteria was not met for option " + option.GetKeyName()); // Reason is needed
                }
                _registry.SetValue(option.GetKeyName(), option.Value);
            }
        }
    }
}
