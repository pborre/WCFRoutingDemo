using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MTServiceClient.Ref_Svc;
using System.ServiceModel;
using System.ServiceModel.Discovery;
using System.ServiceModel.Description;


namespace MTServiceClient
{
    class Client
    {
        static int Received;
        static int Send;
        static void Main(string[] args)
        {
            try
            {
                Console.Title = "Client";               
                System.Diagnostics.Debug.WriteLine("Client started");
                Console.WriteLine("Enter 1234 to send to Service 1234");
                Console.WriteLine("Enter 5678 to send to Service 5678");
                Console.WriteLine("Enter 9999 to send to discovery");
                string input = "";
                input = Console.ReadLine();

               

                while (input.ToUpper() != "X")
                {
                    for (int i = 0; i < 1; i++)
                    {
                        System.Threading.Thread thr = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(CallService20Times));
                        thr.Start(input);

                    }
                    input = Console.ReadLine();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.ReadLine();
            }


        }

        private static void CallService20Times(Object obj)
        {
            try
            {
                string input = obj.ToString();
                using (MTServiceClient.Ref_Svc.MTServiceClient proxy = new MTServiceClient.Ref_Svc.MTServiceClient())
                {
                    for (int i = 0; i < 1; i++)
                    {
                        //Console.WriteLine(proxy.GetData(10));
                        MTServiceClient.Ref_Svc.CompositeType cType = new MTServiceClient.Ref_Svc.CompositeType();
                        cType.BoolValue = true;
                        cType.StringValue = input;
                        List<Customer> customers = new List<Customer>();
                        customers.Add(new Customer() { Name = "Danone", Id = 100, Country = "BE", Status = "" });
                        customers.Add(new Customer() { Name = "Mohawk", Id = 101, Country = "US", Status = "" });
                        customers.Add(new Customer() { Name = "Arcelor", Id = 102, Country = "FR", Status = "" });
                        customers.Add(new Customer() { Name = "Unilin", Id = 103, Country = "BE", Status = "" });
                        cType.Customers = customers.ToArray<Customer>();
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("Sending Message:");
                        foreach (Customer cust in cType.Customers)
                        {
                            Console.WriteLine(string.Format("Id: {0} \t Name: {1} \t Status: {2}", cust.Id, cust.Name, cust.Status));
                        }



                        Send++;
                        Ref_Svc.CompositeType rType = proxy.GetDataUsingDataContract(cType);



                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(rType.StringValue);
                        Console.WriteLine();
                        Console.WriteLine("Message Received from Service: " + rType.StringValue);
                        foreach (Customer cust in rType.Customers)
                        {

                            Console.WriteLine(string.Format("Id: {0} \t Name: {1} \t Status: {2}", cust.Id, cust.Name, cust.Status));
                        }
                        Console.WriteLine("--------------------------------------------------------------");
                        //}
                        Received++;
                        Console.WriteLine("Send: " + Send.ToString() + " , Received: " + Received);
                    }
                }
            }
            catch (FaultException exc)
            { 
                Console.WriteLine(exc.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }



    }
}
