using System;
using System.Collections.Generic;
using Microsoft.Win32;

namespace Registrar
{
    /// <summary>
    /// Represents the settings in the registry.
    /// Contains an internal dictionary which maps option names to option objects.
    /// </summary>
    public class RegSettings
    {
        string _baseKey = null;
        string _rootKey = null;
        string _registryString = null;

        private Dictionary<string, RegOption> _settings = new Dictionary<string, RegOption>();

        /// <summary>
        /// Constructor for the settings instance.
        /// </summary>
        /// <param name="base_key">The base key in the registry the root key will go under. EG: HKEY_CURRENT_USERS.</param>
        /// <param name="root_key">The root key which is where all the keys the options use will fall under. EG: passing 'RootKey' -> HKEY_CURRENT_USERS/RootKey in the registry.</param>
        public RegSettings(string base_key, string root_key)
        {
            _baseKey = base_key;
            _rootKey = root_key;
            _registryString = String.Format("{0}\\{1}", base_key, root_key);
        }

        /// <summary>
        /// Adds the option instance to the internal mapping of options.
        /// </summary>
        /// <param name="option_name">The name of the option to use in the registry. Can be different than the keyname.</param>
        /// <param name="option">The option instance.</param>
        public void RegisterSetting(string option_name, RegOption option)
        {
            _settings.Add(option_name, option);
        }

        /// <summary>
        /// Retrieves the value associated with the option in the settings mapping (not the option object).
        /// </summary>
        /// <param name="option_name">The name of the option to get the value of.</param>
        /// <returns>The option value. Raises an exception of type KeyNotFound if the option name was not found.</returns>
        public Object GetSetting(string option_name)
        {
            return _settings[option_name].OptionValue;
        }

        /// <summary>
        /// Check if the root key of the setting currently exists in the registry.
        /// </summary>
        /// <returns>True if the key does exist, false if it doesn't. Raises an exception of type InvalidOperationException if the base key was wrong.</returns>
        public bool RootKeyExists()
        {
            RegistryKey _registryRoot;
            switch (_baseKey)
            {
                case BaseKeys.HKEY_CURRENT_USER:
                    _registryRoot = Registry.CurrentUser.OpenSubKey(_rootKey, false);
                    break;
                case BaseKeys.HKEY_CLASSES_ROOT:
                    _registryRoot = Registry.ClassesRoot.OpenSubKey(_rootKey, false);
                    break;
                case BaseKeys.HKEY_CURRENT_CONFIG:
                    _registryRoot = Registry.CurrentConfig.OpenSubKey(_rootKey, false);
                    break;
                case BaseKeys.HKEY_LOCAL_MACHINE:
                    _registryRoot = Registry.LocalMachine.OpenSubKey(_rootKey, false);
                    break;
                case BaseKeys.HKEY_USERS:
                    _registryRoot = Registry.Users.OpenSubKey(_rootKey, false);
                    break;
                default:
                    throw new InvalidOperationException("Invalid Base Key given. Use the BaseKeys helper class.");
            }
            return _registryRoot != null;
        }

        /// <summary>
        /// Attempts to load values out of the registry and set the option instance's values with the loaded values.
        /// </summary>
        /// <returns>Null if successful, or a string detailing which options failed and why.</returns>
        public string LoadSettings() // Load settings from the registry instance
        {
            string _result = null;
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
                    keyValue = Registry.GetValue(keyPath, kvp.Value.GetKeyName(), kvp.Value.OptionValue);
                    if (keyValue == null)
                    {
                        _result += string.Format("\r\nFailed loading option {0}: Option did not exist in the registry. " +
                            "Using default.", kvp.Value.GetKeyName());
                    }
                    else
                    {
                        ValidationResponse validation_result = kvp.Value.SetOptionValue(keyValue);
                        if (!validation_result.Successful)
                        {
                            _result += String.Format("\r\nFailed when validating an option while loading: {0} - {1}", kvp.Value.GetKeyName(), validation_result.Information);
                        }
                    }
                }
                catch (FormatException)
                {
                    _result += String.Format("\r\nFailed when loading option {0}: Option was not formatted correctly. " +
                        "This usually occurs if someone manually" + "alters the entry in the registry. Using default.", kvp.Value.GetKeyName());
                }
            }
            return _result;
        }

        /// <summary>
        /// Attempts to save the current values in the settings dictionary to the registry.
        /// </summary>
        /// <returns>Null if successful, or a string detailing which options failed and why.</returns>
        public string SaveSettings() // Save the settings dict values to the registry
        {
            string _result = null;

            foreach (KeyValuePair<string, RegOption> kvp in _settings)
            {
                string subKeys = kvp.Value.GetSubKeys();
                string keyOut = _registryString;
                if (subKeys != null)
                {
                    keyOut += subKeys;
                }
                ValidationResponse validation_result = kvp.Value.Validate();
                if (!validation_result.Successful)
                {
                    _result += String.Format("\r\nFailed when validating an option during saving: {0} - {1}, this occurs if someone manually edits the registry" +
                        "to use an invalid value. Using default.", kvp.Value.GetKeyName(), validation_result.Information);
                }
                else
                {
                    Registry.SetValue(keyOut, kvp.Value.GetKeyName(), kvp.Value.OptionValue);
                }
            }
            return _result;
        }
    }
}
