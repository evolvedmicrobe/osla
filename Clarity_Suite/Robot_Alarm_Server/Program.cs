using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
//using System.Runtime.Remoting;
//using System.Runtime.Remoting.Channels;
//using System.Runtime.Remoting.Channels.Tcp;
using Robot_Alarm;
using System.Net;
using System.Net.Mail;
using System.ServiceModel;
using System.Threading;
using System.ServiceModel.Description;
using System.IO;

namespace Robot_Alarm
{
    /// <summary>
    /// Run a WCF service 
    /// </summary>

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static int EXTRATIME = 5; //In Minutes
        public static AlarmNotifier ThisAlarm;
        [STAThread]
        static void Main()
        {
            #region Set up selfHost
            // To get this to work you need to:
            //     netsh http add urlacl url=`URL` user=`DOMAIN\user`
            //Uri httpAddress = new Uri("http://localhost:8001/AlarmNotifier");
            StreamReader SR = new StreamReader(System.Environment.CurrentDirectory + "\\" + "HostAddress.clarity");
            SR.ReadLine();
            string strhttpAddress = SR.ReadLine().Trim();
            SR.Close();
            //Uri httpAddress = new Uri("http://140.247.90.36:8001/AlarmNotifier");
            Uri httpAddress = new Uri(strhttpAddress);
            
            ThisAlarm = new AlarmNotifier();
            //Going to pass an instance instead of a type
            //ServiceHost selfHost = new ServiceHost(typeof(AlarmNotifier), httpAddress);
            ServiceHost selfHost = new ServiceHost(ThisAlarm, httpAddress);
            BasicHttpBinding myBinding = new BasicHttpBinding();
            
            //giant value to allow large transfers
            int LargeValue = (int)Math.Pow(2.0, 26);
            myBinding.ReaderQuotas.MaxArrayLength = LargeValue;
            myBinding.MaxBufferSize = LargeValue;
            myBinding.MaxBufferPoolSize = LargeValue;
            myBinding.MaxReceivedMessageSize = LargeValue;
            myBinding.ReaderQuotas.MaxBytesPerRead = LargeValue;
            myBinding.ReaderQuotas.MaxStringContentLength = LargeValue;

            myBinding.ReceiveTimeout = new TimeSpan(0, 5, 0);
            myBinding.SendTimeout = new TimeSpan(0, 5, 0);
            myBinding.CloseTimeout = new TimeSpan(0, 10, 0);
            #endregion
            try
            {
                selfHost.AddServiceEndpoint(typeof(IAlarm),
                                myBinding,
                                "AlarmNotifier");
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                selfHost.Description.Behaviors.Add(smb);

                selfHost.Open();
                Console.WriteLine("The service is ready");
                Console.WriteLine(httpAddress.OriginalString);
                Console.WriteLine();


                
                Console.WriteLine("Alarm Status: " + ThisAlarm.GetAlarmStatus().AlarmOn.ToString());
                Thread test = new Thread(Test);
                test.Start();
                Thread monitor = new Thread(Monitor);
                monitor.Start();
                Application.Run();
            }
            catch (CommunicationException ce)
            {
                Console.WriteLine("An exception ocurred: {0}", ce.Message);
                selfHost.Abort();
            }
            selfHost.Close();
        }
        static void Test()
        {
            Console.WriteLine(DateTime.Now.ToString() + " Testing... ");
            Thread.Sleep(1000);
            if (ThisAlarm.CallConnects("9712228263"))
            {
                Console.WriteLine(DateTime.Now.ToString() + " Test succeeded");
            }
            else { Console.WriteLine(DateTime.Now.ToString() + " Test failed"); }
        }
        static void Monitor()
        {
            int CHECK_INTERVAL = 60000; // 1 minutes
            while (true)
            {
                if (AlarmNotifier.CurrentlyLoadedProtocolData.Count > 0)
                {
                    int MinimumDelay = (from p in AlarmNotifier.CurrentlyLoadedProtocolData select p.maxdelay).Min();
                    //foreach (ProtocolData p in AlarmNotifier.CurrentlyLoadedProtocolData)
                    //{
                    try
                    {
                        TimeSpan maxdelay = new TimeSpan(0, MinimumDelay + EXTRATIME, 0);
                        DateTime lastcheck = ThisAlarm.GetInstrumentStatus().TimeCreated;
                        if (maxdelay.CompareTo(DateTime.Now.Subtract(lastcheck)) < 0)
                        {
                            ReportToAllUsers("The robot software has not reported anything for a time longer than "
                                + (MinimumDelay + EXTRATIME) + " minutes");
                            if (AlarmNotifier.ShouldCall()) { ThisAlarm.CallAllUsers(); }
                            break;
                        }
                    }
                    catch (Exception thrown)
                    {
                        Console.WriteLine(thrown.Message);
                    }
                    //}
                }
                Thread.Sleep(CHECK_INTERVAL);
            }
        }
        #region Alarm Stuff
        static SmtpClient createSmtpClient()
        {
            //IF THIS FAILS, IT IS LIKELY DUE TO THE MCAFEE VIRUS SCANNER
            //CHANGE THE ACCESS PROTECTION TO ALLOW AN EXCEPTION FOR THE PROGRAM
            SmtpClient ToSend = new SmtpClient("smtp.gmail.com");//need to fill this in
            ToSend.UseDefaultCredentials = false;
            ToSend.Port = 587;
            ToSend.EnableSsl = true;
            ToSend.Credentials = new NetworkCredential("clarity.lab.automation@gmail.com", "icontrolrobots");
            return ToSend;

        }
        static void ReportToAllUsers(string message = "The robot has an error, and has stopped working")
        {
            Console.WriteLine(DateTime.Now.ToString() + " Reported to all users: " + message);
            EmailErrorMessageToAllUsers(message);
            if (AlarmNotifier.ShouldCall()) { ThisAlarm.CallAllUsers(); }
        }
        static void EmailErrorMessageToAllUsers(string ErrorMessage = "The robot has an error, and has stopped working")
        {
            foreach (ProtocolData p in AlarmNotifier.CurrentlyLoadedProtocolData)
            {
                string[] emails = p.emails.Split(';');
                foreach (string emailaddress in emails)
                {
                    //IF THIS FAILS, IT IS LIKELY DUE TO THE MCAFEE VIRUS SCANNER
                    //CHANGE THE ACCESS PROTECTION TO ALLOW AN EXCEPTION FOR THE PROGRAM
                    String senderAddress = "clarity.lab.automation@gmail.com";
                    try
                    {
                        MailMessage email = new MailMessage(senderAddress, emailaddress, "Robot Alert", ErrorMessage);
                        SmtpClient ToSend = createSmtpClient();
                        ToSend.Send(email);
                    }
                    catch { }
                }
            }
        }

 
        #endregion
    }
}
