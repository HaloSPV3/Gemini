using System;

namespace Registrar
{
    public class RegistryLoadException : Exception
    {
        public RegistryLoadException(string message) : base(message) { }
    }

    public class RegistryOptionException : Exception
    {
        public RegistryOptionException(string message) : base(message) { }
    }
}
