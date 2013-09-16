using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Discovery;
using MTWCFLibrary;

namespace MTService
{
    class MTService
    {
        static void Main(string[] args)
        {
            Console.Title = "Service 1234";
            using (ServiceHost serviceHost = new ServiceHost(typeof(MTServiceImp)))
            {
                try
                {
                    // Make the service discoverable over UDP multicast
                    serviceHost.Description.Behaviors.Add(new ServiceDiscoveryBehavior());
                    serviceHost.AddServiceEndpoint(new UdpDiscoveryEndpoint());

                    serviceHost.Open();
                    
                    Console.WriteLine("The Sample Service is now running.");
                    Console.WriteLine("EndPoints: ");
                    foreach (ServiceEndpoint ep in serviceHost.Description.Endpoints)
                        Console.WriteLine(" " + ep.Address.Uri.ToString());
                    Console.WriteLine("Press <ENTER> to terminate Service.");
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    // The service can now be accessed.
                    Console.ReadLine();
                    serviceHost.Close();
                }
                catch (CommunicationException)
                {
                    serviceHost.Abort();
                }
            }
        }
    }
}
