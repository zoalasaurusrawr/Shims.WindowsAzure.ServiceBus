using Azure.Messaging.ServiceBus;

namespace Microsoft.ServiceBus
{
    public class ServiceBusConnectionStringBuilder
    {
        public ServiceBusConnectionStringBuilder(string connectionString)
        {
            _properties = ServiceBusConnectionStringProperties.Parse(connectionString);
            EntityPath = _properties.EntityPath;
        }

        private readonly ServiceBusConnectionStringProperties _properties;

        public Uri Endpoint => _properties.Endpoint;
        //TODO: Understand why this was settable before and if this was really just swapping the entity name
        //in the connection string which was why code would often use patterns like
        // new ServiceBusConnectionStringBuilder(connectionString){ EntityPath = "myPath" }
        public string EntityPath { get; set; } = string.Empty;
        public TransportType TransportType { get; set; } = TransportType.Amqp;
        public string FullyQualifiedNamespace => _properties.FullyQualifiedNamespace;
        public string SharedAccessKey => _properties.SharedAccessKey;
        public string SharedAccessSignature => _properties.SharedAccessSignature;

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}