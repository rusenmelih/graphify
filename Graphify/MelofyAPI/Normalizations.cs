using System;
using System.Collections.Generic;
using System.Text;

namespace MelofyAPI
{
    public sealed class Normalizations
    {
        public static object NormalizeLowerCase(object value)
        {
            if (value == null)
                return value;

            return (value.ToString().ToLower());
        }
        public static object NormalizeTrim(object value)
        {
            if (value == null)
                return value;

            return (value.ToString().Trim());
        }
        public static object NormalizeUpperCase(object value)
        {
            if (value == null)
                return value;

            return (value.ToString().ToUpper());
        }
        public static object NormalizeCapitalizeFirstLetterWords(object value)
        {
            string input = (string)value;
            if (String.IsNullOrEmpty(input))
                return input;

            string[] array = input.Split(' ');
            for (int i = 0; i < array.Length; i++)
                if (array[i].Length > 0)
                    array[i] = char.ToUpper(array[i][0]) + array[i].Substring(1).ToLower();

            return String.Join(' ', array);
        }

        public static object NormalizeDischargeStringSpace(object value)
        {
            if (String.IsNullOrWhiteSpace((string)value))
                return null;

            return value;
        }
        public static object NormalizeReplaceWhiteSpaceEmpty(object value)
        {
            if (value == null)
                return value;

            return ((string)value).Replace(" ", string.Empty);
        }



    }
}
