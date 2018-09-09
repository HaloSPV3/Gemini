namespace Registrar
{
    /// <summary>
    /// Defines constants used for the base keys in the registry, intended
    /// to make creating settings instances easier.
    /// </summary>
    public static class BaseKeys // This can definitely be made to be better, but it'll do for now.
    {
        public const string HKEY_CURRENT_USER=     "HKEY_CURRENT_USER";
        public const string HKEY_CLASSES_ROOT=     "HKEY_CLASSES_ROOT";
        public const string HKEY_LOCAL_MACHINE=    "HKEY_LOCAL_MACHINE";
        public const string HKEY_USERS=            "HKEY_USERS";
        public const string HKEY_CURRENT_CONFIG=   "HKEY_CURRENT_CONFIG";
    }
}
