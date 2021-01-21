using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PetRescue.Data.Extensions
{
    public class ValidationExtensions
    {
        public static bool IsNotNullOrEmpty<T>(IEnumerable<T> value)
        {
            return value != null && value.Any();
        }
        public static bool IsNotNullOrEmptyOrWhiteSpace(string value)
        {
            return !(string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value));
        }
        public static bool IsNotNull(object value)
        {
            return value != null;
        }
    }
}
