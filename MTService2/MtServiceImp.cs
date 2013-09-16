using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTWCFLibrary
{
    public class MTServiceImp : IMTService
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            foreach (Customer cust in composite.Customers)
            {
                Console.WriteLine(cust);
            }
            if (composite.BoolValue)
            {
                composite.StringValue = "Service 2";
            }
            return composite;
        }

        public CompositeType GetDataUsingDataContract2(CompositeType composite)
        {
            foreach (Customer cust in composite.Customers)
            {
                Console.WriteLine(cust);
                cust.Id += 1;
            }
            if (composite.BoolValue)
            {
                composite.StringValue = "Service 1";
            }
            return composite;
        }

        string IMTService.GetData(int value)
        {
            throw new NotImplementedException();
        }


    }
}
