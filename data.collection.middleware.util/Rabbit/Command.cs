namespace data.collection.middleware.util.Rabbit
{
    public class Command
    {
        public virtual string ExchangeName { get; set; }
        public virtual string Message { get; set; }
        public virtual string RoutingKey { get; set; }
    }
}