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
            Uri httpAddress = new Uri("http://140.247.92.83:8001/AlarmNotifier");
            //Uri httpAddress = new Uri("http://localhost:8000/AlarmNotifier");
            ServiceHost selfHost = new ServiceHost(typeof(AlarmNotifier), httpAddress);
            //WSHttpBinding myBinding = new WSHttpBinding();
            BasicHttpBinding myBinding = new BasicHttpBinding();
            //giant value to allow large transfers
            int LargeValue = (int)Math.Pow(2.0, 26);
            myBinding.ReaderQuotas.MaxArrayLength = LargeValue;
            myBinding.MaxBufferSize = LargeValue;
            myBinding.MaxBufferPoolSize = LargeValue;
            myBinding.MaxReceivedMessageSize = LargeValue;
            myBinding.ReaderQuotas.MaxBytesPerRead = LargeValue;
            myBinding.ReaderQuotas.MaxStringContentLength = LargeValue;
            //myBinding.ReaderQuotas.MaxDepth = LargeValue;
            //myBinding.ReaderQuotas.MaxNameTableCharCount = LargeValue;
            
            myBinding.ReceiveTimeout = new TimeSpan(0, 5, 0);
            myBinding.SendTimeout = new TimeSpan(0, 5, 0);
            myBinding.CloseTimeout = new TimeSpan(0, 10, 0);

            //myBinding.Security = WSHttpSecurity();
            //NetTcpBinding myBinding = new NetTcpBinding();
            //myBinding.Security.Mode = SecurityMode.Transport;
            //myBinding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            try
            {
                selfHost.AddServiceEndpoint(typeof(IAlarm),
                                myBinding,
                                "AlarmNotifier");
                ServiceMetadataBehavior smb=new ServiceMetadataBehavior();
                smb.HttpGetEnabled=true;
               
                selfHost.Description.Behaviors.Add(smb);
                 
                //step 5 of hosting procedure: start (and then stop) service
                selfHost.Open();
                Console.WriteLine("The service is ready");
                Console.WriteLine(httpAddress.OriginalString);
                Console.WriteLine();
                //now to 
                AlarmNotifier AN = new AlarmNotifier();
                //AN.TurnOnAlarm();
                Console.WriteLine(AN.GetAlarmStatus().AlarmOn.ToString());
                while (true)
                {
                    string entry=Console.ReadLine();
                    if (entry == "y")
                    {
                        AN.TurnOnAlarm();
                        AN.UpdateStatus("Alarm is on");
                    }
                    else if (entry == "n")
                    {
                        AN.TurnOffAlarm();
                        AN.UpdateStatus("Alarm is off");
                    }
                    else if (entry == "q")
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine(entry);
                    }
                }


               
            }
            catch(CommunicationException ce)
            {
                Console.WriteLine("An exception ocurred: {0}",ce.Message);
                selfHost.Abort();
            }
           //TcpChannel channel = new TcpChannel(8085);           
           //ChannelServices.RegisterChannel(channel,true);
           //AlarmNotifier InstStatus = new AlarmNotifier();
           //RemotingServices.Marshal(InstStatus, "AlarmNotifier");
           //Console.WriteLine(channel.IsSecured.ToString());

           //Application.Run();
           //Console.WriteLine("The server is up and Running");
           //InstStatus.UpdateStatus("First round wait");
           //Thread.Sleep(7000);
           //InstStatus.TurnOnAlarm();
           //Thread.Sleep(6000);
           //InstStatus.UpdateStatus("New Status");
           

           Console.ReadLine();
           selfHost.Close();
        }
    }
}
