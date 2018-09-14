namespace Registrar
{
    public class OptionRegistrationException : System.Exception
    {
        public OptionRegistrationException() { }
        public OptionRegistrationException(string message) : base(message) { }
        public OptionRegistrationException(string message, System.Exception inner) : base(message, inner) { }
        protected OptionRegistrationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    public class OptionRetrievalException : System.Exception
    {
        public OptionRetrievalException() { }
        public OptionRetrievalException(string message) : base(message) { }
        public OptionRetrievalException(string message, System.Exception inner) : base(message, inner) { }
        protected OptionRetrievalException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    public class OptionAssignmentException : System.Exception
    {
        public OptionAssignmentException() { }
        public OptionAssignmentException(string message) : base(message) { }
        public OptionAssignmentException(string message, System.Exception inner) : base(message, inner) { }
        protected OptionAssignmentException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    public class RegLoadException : System.Exception
    {
        public RegLoadException() { }
        public RegLoadException(string message) : base(message) { }
        public RegLoadException(string message, System.Exception inner) : base(message, inner) { }
        protected RegLoadException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    public class RegSaveException : System.Exception
    {
        public RegSaveException() { }
        public RegSaveException(string message) : base(message) { }
        public RegSaveException(string message, System.Exception inner) : base(message, inner) { }
        protected RegSaveException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
