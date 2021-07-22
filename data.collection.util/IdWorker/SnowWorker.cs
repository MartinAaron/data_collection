using data.collection.util.DI;

namespace data.collection.util.IdWorker
{
    public class SnowWorker : ISingletonDependency
    {
        private readonly Snowflake.Core.IdWorker _worker;

        public SnowWorker()
        {
            _worker = new Snowflake.Core.IdWorker(1, 1);
        }

        public string GetId()
        {
            return _worker.NextId().ToString();
        }
    }
}