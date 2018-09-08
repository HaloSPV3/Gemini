using System;

namespace Registrar
{
    public class RegistryOptionException : Exception
    {
        public RegistryOptionException(string message) : base(message) { }
    }

    public class RegistryKeyNotFoundException : Exception
    {
        public RegistryKeyNotFoundException(string message) : base(message) { }
    }

    public class RegistryKeyFormatException : Exception
    {
        public RegistryKeyFormatException(string message) : base(message) { }
    }
}
