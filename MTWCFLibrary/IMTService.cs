using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Data;
using System.ServiceModel;
using System.Text;

namespace MTWCFLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IMTService
    {

        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        [OperationContract]
        CompositeType GetDataUsingDataContract2(CompositeType composite);
        
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";
        private List<Customer> customers;

        [DataMember]
        public List<Customer> Customers
        {
            get { return customers; }
            set { customers = value; }
        }

        [DataMember(Name="BoolValue")]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember(Name = "StringValue")]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }

        
    }

    [DataContract]
    public class Customer
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Country { get; set; }
        [DataMember]
        public string Status { get; set; }

        public override string ToString()
        {
            return string.Format("Id: {0} \t Name: {1} \t Status: {2}", Id, Name, Status);
        }
    }
}
