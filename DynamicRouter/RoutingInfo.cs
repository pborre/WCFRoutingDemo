using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicRouter
{
    class RoutingInfo
    {
        public RoutingInfo(string value, List<string> epNames)
        {
            routingValue = value;
            endpointNames = epNames;
        }

        public string routingValue;
        public List<string> endpointNames;
    }
}
