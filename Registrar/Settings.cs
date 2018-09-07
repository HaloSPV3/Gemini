using System;
using System.Security.Permissions;
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

        public Object[] SettingsArray = new Object[] { };

        public Settings(RegistryKey root_key, string sub_key)
        {
            _rootKey = root_key;
            _registry = _rootKey.OpenSubKey(sub_key);
        }

    }
}
