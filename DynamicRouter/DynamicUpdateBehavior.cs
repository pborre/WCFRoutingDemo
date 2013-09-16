//  Copyright (c) Microsoft Corporation.  All Rights Reserved.
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Routing;
using System.ServiceModel.Routing.Configuration;
using System.ServiceModel.Configuration;
using System.ServiceModel.Discovery;
using System.Threading;


namespace DynamicRouter
{

    public class UpdateBehavior : BehaviorExtensionElement, IServiceBehavior
    {
        void IServiceBehavior.AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
        }

        void IServiceBehavior.ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            RulesUpdateExtension rulesUpdateExtension = new RulesUpdateExtension();
            serviceHostBase.Extensions.Add(rulesUpdateExtension);
        }
        void IServiceBehavior.Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        class RulesUpdateExtension : IExtension<ServiceHostBase>, IDisposable
        {
            bool primary = false;
            ServiceHostBase owner;
            Timer timer;

            void IExtension<ServiceHostBase>.Attach(ServiceHostBase owner)
            {
                this.owner = owner;
                //Call immediately, then every 5 seconds after that.
                //add your logic here to TRIGGER the rebuild of the filtertable!!!!
                // e.g. file watcher, sql notification that is catched....
                this.timer = new Timer(this.UpdateRules, this, TimeSpan.Zero, TimeSpan.FromSeconds(5));
                
            }

            void IExtension<ServiceHostBase>.Detach(ServiceHostBase owner)
            {
                this.Dispose();
            }

            public void Dispose()
            {
                if (this.timer != null)
                {
                    this.timer.Dispose();
                    this.timer = null;
                }
            }

            //static void UpdateRules(object state)
            void UpdateRules(object state)
            {

                RoutingConfiguration rc = new RoutingConfiguration();
                rc.RouteOnHeadersOnly = false;

                #region Create routing info
                //Get the routing data from a configuration database!!
                List<RoutingInfo> routingInfo = new List<RoutingInfo>();
                //routingInfo.Add(new RoutingInfo("Mohawk", new List<string>() { "ep1","ep2"}));
                routingInfo.Add(new RoutingInfo("1234", new List<string>() {"ep1" }));
                routingInfo.Add(new RoutingInfo("5678", new List<string>() {"ep2" }));
                routingInfo.Add(new RoutingInfo("9999", new List<string>() {"dynamic" }));
                #endregion

                #region Create endpoints
                //read endpoint config from a database ;)
                //endpoints can be configured dynamicly!!! changing the endpoint in the DB should trigger the UpdateRules.
                System.Collections.Generic.Dictionary<string, ServiceEndpoint> endpoints = new Dictionary<string,ServiceEndpoint>();
                ServiceEndpoint ep1 = new ServiceEndpoint(ContractDescription.GetContract(typeof(IRequestReplyRouter)),
                    new BasicHttpBinding(), new EndpointAddress("http://localhost:5814/MTService/EndPoint1"));
                endpoints.Add("ep1", ep1);
                ServiceEndpoint ep2 = new ServiceEndpoint(ContractDescription.GetContract(typeof(IRequestReplyRouter)),
                    new BasicHttpBinding(), new EndpointAddress("http://localhost:5813/MTService/EndPoint2"));
                endpoints.Add("ep2", ep2);
                
                DynamicEndpoint dynamic = new DynamicEndpoint(
                ContractDescription.GetContract(typeof(MTWCFLibrary.IMTService)),
                new BasicHttpBinding());
                endpoints.Add("dynamic", dynamic);
                #endregion

                #region Build filtertable
                //Build the filtertable at runtime
                foreach (RoutingInfo ri in routingInfo) 
                {
                    List<ServiceEndpoint> eps = new List<ServiceEndpoint>();
                    foreach(string epName in ri.endpointNames)
                    {
                        eps.Add(endpoints[epName]);
                    }
                    
                    XPathMessageContext xpmc = new XPathMessageContext();
                    xpmc.AddNamespace("a", "http://schemas.datacontract.org/2004/07/MTWCFLibrary");

                    XPathMessageFilter valueFilter = new XPathMessageFilter("//a:StringValue = '" + ri.routingValue + "'", xpmc);
                    rc.FilterTable.Add(valueFilter, eps);

                   // rc.FilterTable.Add(new XPathMessageFilter("//[*[local-name()='StringValue'] = \"" + ri.routingValue + "\"]") , eps);
                    //rc.FilterTable.Add(new MatchAllMessageFilter(), eps);
                }
                #endregion

                this.owner.Extensions.Find<RoutingExtension>().ApplyConfiguration(rc);

                this.primary = !this.primary;
            }
        }

        public override Type BehaviorType
        {
            get { return typeof(UpdateBehavior); }
        }

        protected override object CreateBehavior()
        {
            return new UpdateBehavior();
        }
    }



}
