using System;
using System.Collections.Generic;

namespace Registrar
{
    /// <summary>
    /// Interface which defines the behavior of a validator to be used with an option's value.
    /// Validate(object value) should take the value and run it through some checks. Returning
    /// true will make the check pass, false will fail it.
    /// Description should be a description of the criteria which makes it fail or pass.
    /// </summary>
    public interface IValidator
    {
        bool Validate(object value);
        string Description();
    }

    /// <summary>
    /// A class which when instantiated, is to contain information concerning the result of
    /// the validation.
    /// If the option successfully was validated, Successful will be true, otherwise it will
    /// be false.
    /// Information will contain information on why the check failed.
    /// </summary>
    public class ValidationResponse
    {
        public bool Successful { get; set; }
        public string Information { get; set; }
    }

    /// <summary>
    /// A helper class of converters to be used when writing implementations of the IValidator interface.
    /// Returns the converted value associated with the method name.
    /// </summary>
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
            Dictionary<string, Object> booleanConverterDict = new Dictionary<string, Object>() // This is a bit iffy.
            {
                { "true", true },
                { "false", false },
                { "1", true },
                { "0", false },
            };

            string valueStr = value.ToString().ToLower();
            bool conversionSuccessful = booleanConverterDict.TryGetValue(valueStr, out object convertedValue);

            if (conversionSuccessful)
            {
                return (bool)convertedValue;
            }
            else
            {
                return false;
            }
        }
    }
}
