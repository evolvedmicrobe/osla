using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace SkypeAlarmServer
{

    class Program
    {
        static SkypeAlarm SA = new SkypeAlarm();
        [STAThread]
        static void Main(string[] args)
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            binding.Name = "SAbinding";

            //Uri httpAddress = new Uri("http://140.247.92.83:8001/SkypeAlarmService");
            Uri httpAddress = new Uri("http://localhost:8001/SkypeAlarmService");

            // Create a ServiceHost for the CalculatorService type and provide the base address.
            ServiceHost serviceHost = new ServiceHost(SA, httpAddress);

            // Open the ServiceHostBase to create listeners and start listening for messages.
            serviceHost.Open();

            // The service can now be accessed.
            Console.WriteLine("SkypeAlarmService is ready.");
            Console.WriteLine("Press <ENTER> to terminate service.");
            Console.WriteLine();
            Console.ReadLine();

            // Close the ServiceHostBase to shutdown the service.
            serviceHost.Close();
        }
    }
}
