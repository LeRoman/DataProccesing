using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataProcessing
{
    class Validator
    {
        static Regex Date = new(@"\d{4}-\d{2}-\d{2}");
        static Regex AccNumber = new(@"\d+");
        static Regex Text = new(@"[a-zA-Z]+");
        static Regex Payment = new(@"\d+\.\d");

        static public bool IsValid(string[] row)
        {
            if (!Text.IsMatch(row[0]) || !Text.IsMatch(row[1])
                || !Text.IsMatch(row[2]) || !Payment.IsMatch(row[3])
                || !Date.IsMatch(row[4]) || !AccNumber.IsMatch(row[5])
                || !Text.IsMatch(row[6])) return false;
            return true;
        }

    }
}
