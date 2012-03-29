using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SKYPE4COMLib;
using System.Text.RegularExpressions;

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
            bool callSuccessful = false;
            try
            {
                Call c;
                c = skype.PlaceCall(number);
                int waits = 1;
                const int maxWait = 50;
                // TODO: Use some kind of call property to avoid waiting for calls that fail instantly
                while (c.Status != TCallStatus.clsInProgress && waits < maxWait)
                {
                    TCallStatus curStatus = c.Status;
                    if (curStatus == TCallStatus.clsFailed | curStatus == TCallStatus.clsRefused
                        || curStatus == TCallStatus.clsCancelled || curStatus == TCallStatus.clsBusy ||
                        curStatus == TCallStatus.clsBusy)
                    {
                        break;
                    }
                    System.Threading.Thread.Sleep(500);
                }
                waits = 0;
                while (c.Status == TCallStatus.clsInProgress && waits < maxWait)
                {
                    if (waits > 5)
                    { callSuccessful = true; break; }
                    waits++;
                    System.Threading.Thread.Sleep(1000);
                }
                if (c.Status != TCallStatus.clsFinished)
                    c.Finish();
            }
            catch (Exception thrown) { Console.WriteLine(thrown.Message); }
            finally { }
            return callSuccessful;

        }
    }

}
