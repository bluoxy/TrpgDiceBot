using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;

namespace TrpgDiceBot
{
    public static class StringExtensions
    {
        public static T Calc<T>(this string s, params object[] args)
        {
            using var dt = new DataTable();
            s = string.Format(s, args);
            var converter = TypeDescriptor.GetConverter(typeof(T));
            object result = dt.Compute(s, "");
            return (T)converter.ConvertFromString(result.ToString());
        }
    }
}
