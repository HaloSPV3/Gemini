using System;
using System.Collections.Generic;
using Microsoft.Win32;

namespace Registrar
{
    public class RegSettings
    {
        string _baseKey = null;
        string _rootKey = null;
        string _registryString = null;
        private Dictionary<string, RegOption> _settings = new Dictionary<string, RegOption>();

        public RegSettings(string base_key, string root_key)
        {
            _baseKey = base_key;
            _rootKey = root_key;
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

        public bool RootKeyEntryExists()
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
                        _result += string.Format("Failed loading option {0}: Option did not exist in the registry. " +
                            "Using default.", kvp.Value.GetKeyName());
                    }
                    else
                    {
                        kvp.Value.OptionValue = keyValue;
                    }
                }
                catch (FormatException)
                {
                    _result += String.Format("Failed when loading option {0}: Option was not formatted correctly. " +
                        "This usually occurs if someone manually" + "alters the entry in the registry. Using default.", kvp.Value.GetKeyName());
                }
            }
            return _result;
        }

        public string SaveSettings() // Save the settings dict values to the registry
        {
            string _result = null;

            foreach (KeyValuePair<string, RegOption> kvp in _settings)
            {
                string subKeys = kvp.Value.GetSubKeys();

                ValidationResponse validation_result = kvp.Value.Validate();
                if (!validation_result.Successful)
                {
                    _result += String.Format("Failed when validating an option: {0} - {1}", kvp.Value.GetKeyName(), validation_result.Information);
                }

                string keyOut = _registryString;
                if (subKeys != null)
                {
                    keyOut += subKeys;
                }
                Registry.SetValue(keyOut, kvp.Value.GetKeyName(), kvp.Value.OptionValue);
            }

            return _result;
        }
    }
}
