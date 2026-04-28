using System;
using System.Collections.Generic;
using System.Linq;

namespace VidaCamara.CrossCuting.Utilities
{
    public static class StrinExtensions
    {
        public static TSelf TrimStringProperties<TSelf>(this TSelf input)
        {
            var stringProperties = input.GetType().GetProperties()
                .Where(p => p.PropertyType == typeof(string) && p.CanWrite);

            foreach (var stringProperty in stringProperties)
            {
                string currentValue = (string)stringProperty.GetValue(input, null);
                if (currentValue != null)
                    stringProperty.SetValue(input, currentValue.Trim(), null);
            }
            return input;
        }

        public static TSelf FormatDecimalProperties<TSelf>(this TSelf input)
        {
            var stringProperties = input.GetType().GetProperties()
               .Where(p => p.PropertyType == typeof(decimal) && p.CanWrite);

            foreach (var stringProperty in stringProperties)
            {
                decimal currentValue = (decimal)stringProperty.GetValue(input, null);
                if (currentValue != 0)
                    stringProperty.SetValue(input, currentValue.ToString("C2"), null);
            }

            return input;
        }

        public static TSelf FormatToUpperProperties<TSelf>(this TSelf input)
        {
            var stringProperties = input.GetType().GetProperties()
                .Where(p => p.PropertyType == typeof(string) && p.CanWrite);

            foreach (var stringProperty in stringProperties)
            {
                string currentValue = (string)stringProperty.GetValue(input, null);
                if (currentValue != null)
                    stringProperty.SetValue(input, currentValue.ToUpper(), null);
            }
            return input;
        }

        public static TSelf NullStringProperties<TSelf>(this TSelf input)
        {
            var stringProperties = input.GetType().GetProperties()
                .Where(p => p.PropertyType == typeof(string) && p.CanWrite);

            foreach (var stringProperty in stringProperties)
            {
                string currentValue = (string)stringProperty.GetValue(input, null);
                if (currentValue == null)
                    stringProperty.SetValue(input, "", null);
            }
            return input;
        }

        public static string ToConcatedString<TSelf>(this TSelf input)
        {
            string joined = "";
            var stringProperties = input.GetType().GetProperties()
                .Where(p => p.PropertyType == typeof(string) && p.CanWrite);

            //var camposNoValidosSucave = UtilHelper.obtainConfig("camposNoValidosSucave").Split(';');
            foreach (var stringProperty in stringProperties)
            {
                /*if (!camposNoValidosSucave.Contains(stringProperty.Name))
                {*/
                    string currentValue = (string)stringProperty.GetValue(input, null);
                    joined += currentValue;
                /*}*/
            }

            return joined;
        }
        public static bool IsAnyNullOrEmpty(object obj)
        {
            if (Object.ReferenceEquals(obj, null))
                return true;

            return obj.GetType().GetProperties()
                .Any(x => IsNullOrEmpty(x.GetValue(obj)));
        }

        private static bool IsNullOrEmpty(object value)
        {
            if (Object.ReferenceEquals(value, null))
                return true;

            var type = value.GetType();
            return type.IsValueType
                && Object.Equals(value, Activator.CreateInstance(type));
        }
    }
}
