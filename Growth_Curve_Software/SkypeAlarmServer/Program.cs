using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SKYPE4COMLib;
using System.Text.RegularExpressions;

namespace SkypeAlarmServer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    namespace Growth_Curve_Software
    {
        public static class SkypeAlarm
        {
            private static Skype skype = new Skype();

            // This set simply remembers what numbers we've verified and so that we don't
            // waste precious cents re-verifying
            private static HashSet<string> verified = new HashSet<string>();
            private static string number_re = @"^([0-9]{10};? *)+$";
            public static bool TestNumbers(string numbers)
            {
                Match m = Regex.Match(numbers, number_re);
                if (!m.Success) { return false; }
                foreach (string n in numbers.Split(';'))
                {
                    string number = n.Trim();
                    if (verified.Contains(number)) { continue; }
                    if (CallConnects(number)) { verified.Add(number); }
                    else { return false; }
                }
                return true;
            }
            public static bool CallConnects(string number)
            {
                if (!skype.Client.IsRunning)
                {
                    skype.Client.Start(true, true);
                }
                Call c;
                try
                {
                    c = skype.PlaceCall(number);
                }
                catch { return false; }
                int waits = 1;
                // TODO: Use some kind of call property to avoid waiting for calls that fail instantly
                while (c.Duration == 0)
                {
                    System.Threading.Thread.Sleep(500);
                    if (waits++ > 50) { return false; }
                }
                try { c.Finish(); }
                catch { return false; }
                return true;

            }
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
