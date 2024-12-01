using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace RestService
{
    public class NumberValidator
    {
        public static bool IsValid(string number)
        {
            return !string.IsNullOrWhiteSpace(number) && Regex.IsMatch(number, @"^\d+$");
        }
    }
}