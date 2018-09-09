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

        private Dictionary<string, RegOption> _settingsMapping = new Dictionary<string, RegOption>();

        /// <summary>
        /// Constructor for the settings instance.
        /// </summary>
        /// <param name="baseKey">The base key in the registry the root key will go under. EG: HKEY_CURRENT_USERS.</param>
        /// <param name="rootKey">The root key which is where all the keys the options use will fall under. EG: passing 'RootKey' -> HKEY_CURRENT_USERS/RootKey in the registry.</param>
        public RegSettings(string baseKey, string rootKey)
        {
            _baseKey = baseKey;

            while (rootKey[0] == '/' || rootKey[0] == '\\')
            {
                rootKey = rootKey.Substring(1);
            }
            _rootKey = rootKey.Replace(@"/", @"\\");

            _registryString = String.Format("{0}\\{1}", _baseKey, _rootKey);
        }

        /// <summary>
        /// Adds the option instance to the internal mapping of options.
        /// </summary>
        /// <param name="optionName">The name of the option to use in the registry. Can be different than the keyname.</param>
        /// <param name="option">The option instance.</param>
        public void RegisterSetting(string optionName, RegOption option)
        {
            _settingsMapping.Add(optionName, option);
        }

        /// <summary>
        /// Retrieves the value associated with the option in the settings mapping (not the option object).
        /// </summary>
        /// <param name="optionName">The name of the option to get the value of.</param>
        /// <returns>The option value. Raises an exception of type KeyNotFound if the option name was not found.</returns>
        public T GetOption<T>(string optionName)
        {
            return (T) _settingsMapping[optionName].OptionValue;
        }

        /// <summary>
        /// Attempts to set the value of an option in the settings mapping to the supplied value.
        /// If the option fails to get set, it will keep its default value.
        /// </summary>
        /// <param name="optionName">The name of the option in the mapping to be changed.</param>
        /// <param name="value">The value to attempt to set the option to.</param>
        /// <returns>Returns an error message detailing why it failed to be set, or null if it was sucessfully set.</returns>
        public string SetOption(string optionName, Object value)
        {
            ValidationResponse validationResult = _settingsMapping[optionName].SetOptionValue(value);
            if (!validationResult.Successful)
            {
                return String.Format("Failed to set option '{0}', reason: '{1}'. Option will keep its current value.", optionName, validationResult.Information);
            }
            return null;
        }

        /// <summary>
        /// Check if the root key of the setting currently exists in the registry.
        /// </summary>
        /// <returns>True if the key does exist, false if it doesn't. Raises an exception of type InvalidOperationException if the base key was wrong.</returns>
        public bool RootKeyExists()
        {
            RegistryKey registryRoot;
            switch (_baseKey)
            {
                case BaseKeys.HKEY_CURRENT_USER:
                    registryRoot = Registry.CurrentUser.OpenSubKey(_rootKey, false);
                    break;
                case BaseKeys.HKEY_CLASSES_ROOT:
                    registryRoot = Registry.ClassesRoot.OpenSubKey(_rootKey, false);
                    break;
                case BaseKeys.HKEY_CURRENT_CONFIG:
                    registryRoot = Registry.CurrentConfig.OpenSubKey(_rootKey, false);
                    break;
                case BaseKeys.HKEY_LOCAL_MACHINE:
                    registryRoot = Registry.LocalMachine.OpenSubKey(_rootKey, false);
                    break;
                case BaseKeys.HKEY_USERS:
                    registryRoot = Registry.Users.OpenSubKey(_rootKey, false);
                    break;
                default:
                    throw new InvalidOperationException("Invalid Base Key given. Use the BaseKeys helper class.");
            }
            return registryRoot != null;
        }

        /// <summary>
        /// Attempts to load values out of the registry and set the option instance's values with the loaded values.
        /// </summary>
        /// <returns>Null if successful, or a string detailing which options failed and why.</returns>
        public string LoadSettings() // Load settings from the registry instance
        {
            string loadResult = null;
            foreach (KeyValuePair<string, RegOption> kvp in _settingsMapping)
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
                        loadResult += string.Format("\r\nFailed loading option '{0}': Option did not exist in the registry. " +
                            "The value will use its default.", kvp.Value.GetKeyName());
                    }
                    else
                    {
                        ValidationResponse validation_result = kvp.Value.SetOptionValue(keyValue);
                        if (!validation_result.Successful)
                        {
                            loadResult += String.Format("\r\nFailed when validating an option while loading: '{0}' - '{1}'. The value will use its default.", kvp.Value.GetKeyName(), validation_result.Information);
                        }
                    }
                }
                catch (FormatException)
                {
                    loadResult += String.Format("\r\nFailed when loading option '{0}': Option was not formatted correctly. " +
                        "This usually occurs if someone manually" + "alters the entry in the registry. The value will use its default.", kvp.Value.GetKeyName());
                }
            }
            return loadResult;
        }

        /// <summary>
        /// Attempts to save the current values in the settings dictionary to the registry.
        /// </summary>
        /// <returns>Null if successful, or a string detailing which options failed and why.</returns>
        public string SaveSettings() // Save the settings dict values to the registry
        {
            string saveResult = null;

            foreach (KeyValuePair<string, RegOption> kvp in _settingsMapping)
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
                    saveResult += String.Format("\r\nFailed when validating an option during saving: '{0}' - '{1}', this occurs if someone manually edits the registry" +
                        "to use an invalid value. The value will use its default.", kvp.Value.GetKeyName(), validation_result.Information);
                }
                else
                {
                    Registry.SetValue(keyOut, kvp.Value.GetKeyName(), kvp.Value.OptionValue);
                }
            }
            return saveResult;
        }
    }
}
