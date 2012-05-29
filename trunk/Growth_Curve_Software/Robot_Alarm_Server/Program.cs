using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
//using System.Runtime.Remoting;
//using System.Runtime.Remoting.Channels;
//using System.Runtime.Remoting.Channels.Tcp;
using Robot_Alarm;
using System.ServiceModel;
using System.Threading;
using System.ServiceModel.Description;

namespace Robot_Alarm
{
 /// <summary>
 /// Run a WCF service 
 /// </summary>
   
    static class Program
    {
        
        public static bool ShouldExit
        {
            get
            {
                Console.WriteLine("What");
                Console.ReadLine();
                return false;
            }
            set
            {
                if (value == true) { Application.Exit(); }                
            }
        }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static AlarmNotifier ThisAlarm;
        static void TurnOnNotifierAlarm()
        {
        }
        [STAThread]
        static void Main()
        {
            #region Set up selfHost
            // To get this to work you need to:
            //     netsh http add urlacl url=`URL` user=`DOMAIN\user`
            Uri httpAddress = new Uri("http://localhost:8001/AlarmNotifier");
            //Uri httpAddress = new Uri("http://140.247.90.36:8001/AlarmNotifier");
            ServiceHost selfHost = new ServiceHost(typeof(AlarmNotifier), httpAddress);
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
                ServiceMetadataBehavior smb=new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                selfHost.Description.Behaviors.Add(smb);
                
                selfHost.Open();
                Console.WriteLine("The service is ready");
                Console.WriteLine(httpAddress.OriginalString);
                Console.WriteLine();


                ThisAlarm = new AlarmNotifier();
                Console.WriteLine("Alarm Status:");
                Console.WriteLine(ThisAlarm.GetAlarmStatus().AlarmOn.ToString());
                Thread test = new Thread(Test);
                test.Start();
                Thread poll = new Thread(Poll);
                Application.Run();
            }
            catch(CommunicationException ce)
            {
                Console.WriteLine("An exception ocurred: {0}",ce.Message);
                selfHost.Abort();
            }
            selfHost.Close();
        }
        static void Test()
        {
            Thread.Sleep(1000);
            if (ThisAlarm.CallConnects("9712228263"))
            {
                Console.WriteLine("Test succeeded");
            }
            else { Console.WriteLine("Test failed"); }
        }
        static void Poll()
        {
            Console.WriteLine("Last poll at {0}", DateTime.Now.ToString());
        }
    }
}
