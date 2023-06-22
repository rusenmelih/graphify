using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace MelofyAPI
{
    public sealed class Melofies
    {
        public static bool Required(AbstractFormControl control)
        {
            if (control.Value == null)
                return false;

            return true;
        }
        public static bool MaxLength(AbstractFormControl control, int maxlength)
        {
            if (control.Value == null)
                return true;

            if (control.Value.ToString().Length > maxlength)
                return false;

            return true;
        }
        public static bool MinLength(AbstractFormControl control, int minlength)
        {
            if (control.Value == null)
                return true;

            if (control.Value != null && control.Value.ToString().Length >= minlength)
                return true;

            return false;
        }
        public static bool Email(AbstractFormControl control)
        {
            if (control.Value == null)
                return true;

            return Regex.IsMatch(control.Value.ToString(), @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
        }
        public static bool Password(AbstractFormControl control)
        {
            if (control.Value == null)
                return true;

            return Regex.IsMatch(control.Value.ToString(), @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$");
        }
        public static bool PasswordRepeat(AbstractFormControl control, string password)
        {
            if (control.Value == null)
                return true;

            return (control.Value.ToString() == password);
        }
        public static bool Pattern(AbstractFormControl control, Regex regex)
        {
            if (control.Value == null)
                return true;

            return regex.IsMatch(control.Value.ToString());
        }
        public static bool Pattern(AbstractFormControl control, string regex)
        {
            if (control.Value == null)
                return true;

            return Regex.IsMatch(control.Value.ToString(), regex);
        }
        public static bool Min(AbstractFormControl control, int min)
        {
            if (control.Value == null)
                return true;

            StreamWriter dom = new StreamWriter("dom.txt");
            dom.Write((control.Value == null) ? ("null") : ("anenen"));
            dom.Close();
            dom.Dispose();

            Type type = control.Value.GetType();
            if (type == typeof(Int32))
                return ((int)control.Value >= min);

            throw new FieldAccessException("Value is not in int type.");
        }
        public static bool Min(AbstractFormControl control, double min)
        {
            if (control.Value == null)
                return true;

            Type type = control.Value.GetType();
            if (type == typeof(Double))
                return ((double)control.Value >= min);

            throw new FieldAccessException("Value is not in double type.");
        }
        public static bool Min(AbstractFormControl control, float min)
        {
            if (control.Value == null)
                return true;

            Type type = control.Value.GetType();
            if (type == typeof(Single))
                return ((float)control.Value >= min);

            throw new FieldAccessException("Value is not in float type.");
        }
        public static bool Min(AbstractFormControl control, long min)
        {
            if (control.Value == null)
                return true;

            Type type = control.Value.GetType();
            if (type == typeof(Int64))
                return ((long)control.Value >= min);

            throw new FieldAccessException("Value is not in long type.");
        }
        public static bool Min(AbstractFormControl control, decimal min)
        {
            if (control.Value == null)
                return true;

            Type type = control.Value.GetType();
            if (type == typeof(Decimal))
                return ((decimal)control.Value >= min);

            throw new FieldAccessException("Value is not in decimal type.");
        }
        public static bool Max(AbstractFormControl control, int max)
        {

            if (control.Value == null)
                return true;

            Type type = control.Value.GetType();
            if (type == typeof(Int32))
                return ((int)control.Value <= max);

            throw new FieldAccessException("Value is not in int type.");
        }
        public static bool Max(AbstractFormControl control, double max)
        {

            if (control.Value == null)
                return true;

            Type type = control.Value.GetType();
            if (type == typeof(Double))
                return ((double)control.Value <= max);

            throw new FieldAccessException("Value is not in double type.");
        }
        public static bool Max(AbstractFormControl control, float max)
        {

            if (control.Value == null)
                return true;

            Type type = control.Value.GetType();
            if (type == typeof(Single))
                return ((float)control.Value <= max);

            throw new FieldAccessException("Value is not in int type.");
        }
        public static bool Max(AbstractFormControl control, long max)
        {

            if (control.Value == null)
                return true;

            Type type = control.Value.GetType();
            if (type == typeof(Int64))
                return ((long)control.Value <= max);

            throw new FieldAccessException("Value is not in int type.");
        }
        public static bool Max(AbstractFormControl control, decimal max)
        {

            if (control.Value == null)
                return true;

            Type type = control.Value.GetType();
            if (type == typeof(Decimal))
                return ((decimal)control.Value <= max);

            throw new FieldAccessException("Value is not in decimal type.");
        }
        public static bool Length(AbstractFormControl control, int length)
        {
            if (control.Value == null)
                return true;

            return (((string)control.Value).Length == length);
        }
        public static bool Identity(AbstractFormControl control)
        {
            if (control.Value == null)
                return true;

            string identity = control.Value.ToString();

            if (!Regex.IsMatch(identity, RegexPattern.Identity))
                return false;

            int oddSum = Convert.ToInt32(identity[0].ToString()) + Convert.ToInt32(identity[2].ToString()) + Convert.ToInt32(identity[4].ToString()) + Convert.ToInt32(identity[6].ToString()) + Convert.ToInt32(identity[8].ToString());
            int evenSum = Convert.ToInt32(identity[1].ToString()) + Convert.ToInt32(identity[3].ToString()) + Convert.ToInt32(identity[5].ToString()) + Convert.ToInt32(identity[7].ToString());

            if ((((oddSum * 7) - evenSum) % 10) != Convert.ToInt32(identity[9].ToString()))
                return false;

            int sum = 0;
            for (int i = 0; i < 10; i++)
                sum += Convert.ToInt32(identity[i].ToString());

            if ((sum % 10) != Convert.ToInt32(identity[10].ToString()))
                return false;

            return true;
        }
        public static bool IBAN(AbstractFormControl control)
        {
            if (control.Value == null)
                return true;

            string iban = control.Value.ToString().ToUpper();

            if (!Regex.IsMatch(iban, @"^[A-Z0-9]+$"))
                return false;

            string bank = iban.Substring(4) + iban.Substring(0, 4);

            int asciiShift = 55;
            StringBuilder sb = new StringBuilder();

            foreach (char c in bank)
            {
                int v;
                if (char.IsLetter(c))
                    v = c - asciiShift;
                else
                    v = int.Parse(c.ToString());

                sb.Append(v);
            }

            string checkSumString = sb.ToString();

            int checkSum = int.Parse(checkSumString.Substring(0, 1));

            for (int i = 1; i < checkSumString.Length; i++)
            {
                int v = int.Parse(checkSumString.Substring(i, 1));
                checkSum *= 10;
                checkSum += v;
                checkSum %= 97;

            }
            return (checkSum == 1);
        }
        public static bool OnlyDigit(AbstractFormControl control)
        {
            if (control.Value == null)
                return true;

            return Regex.IsMatch(control.Value.ToString(), @"^[0-9]+$");
        }
        public static bool OnlyLetter(AbstractFormControl control)
        {
            if (control.Value == null)
                return true;
            
            return Regex.IsMatch((string)control.Value, "^[a-zA-Z]+$");
        }
        public static bool FloatingDigit(AbstractFormControl control)
        {

            if (control.Value == null)
                return true;

            return Regex.IsMatch(control.Value.ToString(), @"^-?\d+(?:\.\d+)?$");
        }
        public static bool Int32CountingNumber(AbstractFormControl control)
        {
            if (control.Value == null)
                return true;

            if (control.Value.GetType() == typeof(Int32))
                return (Convert.ToInt32(control.Value) > 0);

            throw new AbstractFormFieldError("Value is not int type");
        }
        public static bool Contains(AbstractFormControl control, List<string> list)
        {
            if (control.Value == null)
                return true;

            return list.Contains(control.Value.ToString());
        }
        public static bool ContainsExcept(AbstractFormControl control, List<string> list, int there)
        {
            if (control.Value == null)
                return true;

            for (int i = 0; i < list.Count; i++)
            {
                if ((string)control.Value == list[i] && i != there)
                    return false;
            }

            return true;
        }
        public static bool ArrayWithDictionaryKeys<T, U>(AbstractFormControl control, Dictionary<T, U> keyValues)
        {
            if (control.Value == null)
                return true;

            if (control.Value.GetType() == typeof(List<string>))
            {
                foreach (T item in (List<T>)control.Value)
                {
                    if (!keyValues.ContainsKey(item))
                        return false;
                }

                return true;
            }

            throw new Exception("Value is not in T-list type.");
        }
        public static bool UniqueList<T>(AbstractFormControl control)
        {
            if (control.Value == null)
                return true;

            if (control.Value.GetType() == typeof(List<T>))
            {
                HashSet<T> hashSet = new HashSet<T>();
                foreach (T item in (List<T>)control.Value)
                    hashSet.Add(item);

                if (hashSet.Count == ((List<T>)control.Value).Count)
                    return true;

                return false;
            }

            throw new Exception("Value is not in T-list type.");
        }
        public static bool EqualiaventString(AbstractFormControl control, string target)
        {
            if (control.Value == null)
                return true;

            if (control.Value.GetType() == typeof(Component<>))
            {
                if (((Component<string>)control.Value).Content == target)
                    return true;
            }
            else
            {
                if (control.Value.ToString() == target)
                    return true;
            }

            return false;
        }
    }
}
