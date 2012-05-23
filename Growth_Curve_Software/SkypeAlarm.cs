using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SKYPE4COMLib;
using System.Text.RegularExpressions;

namespace Growth_Curve_Software
{
    public class SkypeAlarm
    {
        private Skype skype;
        public SkypeAlarm()
        {
            skype = new Skype();
            skype.Client.Start(true, true);
        }

        // This set simply remembers what numbers we've verified and so that we don't
        // waste precious cents re-verifying
        private HashSet<string> verified = new HashSet<string>();
        private static string number_re = @"[0-9]{10}";
        public bool TestNumber(string number)
        {
            if (verified.Contains(number)) { return true; }
            Match m = Regex.Match(number, number_re);
            if (m.Success)
            {
                if (CallConnects(number))
                {
                    verified.Add(number);
                    return true;
                }
            }
            return false;
        }

        private bool CallConnects(string number)
        {
            Call c;
            try
            {
                c = skype.PlaceCall(number);
            }
            catch { return false; }
            int waits = 1;
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
