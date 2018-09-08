using System;

namespace Registrar
{
    public class RegOption
    {
        private string _keyName = null;
        private string _subKeys = null;
        private IValidator _validator = null;
        private object _optionValue = null;
        private object _optionDefault = null;

        public RegOption(string key_name, IValidator validator, Object value, string sub_keys = null)
        {
            _keyName = key_name;

            string keyOut;
            if (sub_keys != null)
            {
                if (sub_keys[0] != '/')
                {
                    sub_keys = '/' + sub_keys;

                }
                keyOut = sub_keys.Replace(@"/", @"\\");
                _subKeys = keyOut;
            }

            _validator = validator;
            _optionValue = value;
        }

        public ValidationResponse Validate(Object value = null)
        {
            ValidationResponse response = new ValidationResponse();
            if (value == null)
            {
                value = _optionValue;
            }

            if (_validator != null)
            {
                bool option_valid = _validator.Validate(value);
                if (!option_valid)
                {
                    response.Successful = false;
                    response.Information = _validator.Description();
                    //return _validator.Description();
                }
                else
                {
                    response.Successful = true;
                    response.Information = "Successfully processed option.";
                }
            }
            _optionValue = value;
            return response;
        }

        public Object OptionValue
        {
            get { return _optionValue; }
            set { Validate(value); }
        }

        public Object OptionDefault
        {
            get { return _optionDefault; }
        }

        public string GetSubKeys()
        {
            return _subKeys;
        }

        public string GetKeyName()
        {
            return _keyName;
        }
    }

    public interface IValidator
    {
        bool Validate(object value);
        string Description();
    }

    public class ValidationResponse
    {
        public bool Successful { get; set; }
        public string Information { get; set; }
    }
}
