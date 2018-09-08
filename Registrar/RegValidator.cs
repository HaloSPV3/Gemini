using System;
using System.Collections.Generic;

namespace Registrar
{
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

    public static class ValidatorConverters
    {
        public static int ValidatorIntConverter(Object value)
        {
            return Convert.ToInt32(value);
        }

        public static string ValidatorStringConverter(Object value)
        {
            return value.ToString();
        }

        public static bool ValidatorBooleanConverter(Object value)
        {
            Dictionary<string, Object> _booleanConversion = new Dictionary<string, Object>()
            {
                { "true", true },
                { "false", false },
                { "1", true },
                { "0", false },
            };

            string _value_str = value.ToString().ToLower();
            bool _out = _booleanConversion.TryGetValue(_value_str, out object _result);

            if (_out)
            {
                return (bool)_result;
            }
            else
            {
                return false;
            }
        }
    }
}
