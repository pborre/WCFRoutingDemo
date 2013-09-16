using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Routing;


namespace DynamicRouter
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a ServiceHost for the CalculatorService type.
            using (ServiceHost serviceHost =
                new ServiceHost(typeof(RoutingService)))
            {

                // Open the ServiceHost to create listeners         
                // and start listening for messages.

                serviceHost.Description.Behaviors.Add(new RoutingBehavior(new RoutingConfiguration()));
                
                ServiceDebugBehavior debugb = serviceHost.Description.Behaviors.Find<ServiceDebugBehavior>();
                debugb.IncludeExceptionDetailInFaults= true;

                serviceHost.Description.Behaviors.Add(new UpdateBehavior());

                Console.WriteLine("The Routing Service configured, opening....");
                serviceHost.Open();

                Console.WriteLine("The Routing Service is now running.");
                Console.WriteLine("Press <ENTER> to terminate router.");

                // The service can now be accessed.
                Console.ReadLine();
            }
        }
    }
}
