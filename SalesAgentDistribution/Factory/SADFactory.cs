using System;
using SalesAgentDistribution.Service;

namespace SalesAgentDistribution.Factory
{
    class SADFactory
    {
        internal static IServiceQueue GetServiceQueue()
        {
            return new ServiceQueue();
        }

        internal static IDBHandler<T> GetDBHandler<T>() where T : new()
        {
            return new DBHandler<T>();
        }

        internal static IDeliveryService GetDeliveryService()
        {
            return new DeliveryServiceRemote();
        }
    }
}