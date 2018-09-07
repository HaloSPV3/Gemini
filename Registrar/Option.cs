using System;

namespace Registrar
{
    public class Option
    {
        private string _keyName = null;
        private string _subKey = null;
        private Type _optionType = null;
        private Validator[] _validatorList = null;
        private object _optionValue = null;

        public Option(string key_name, string sub_key, Type type, Validator[] validators, Object value)
        {
            _keyName = key_name;
            _subKey = sub_key;
            _optionType = type;
            _validatorList = validators;
            _optionValue = value;
        }

        public bool RunValidators()
        {
            foreach (Validator validator in _validatorList)
            {
                if (!validator.Validate(_optionValue))
                {
                    return false;
                }
            }
            return true;
        }
    }

    public abstract class Validator
    {
        public abstract bool Validate(object value);
    }
}
