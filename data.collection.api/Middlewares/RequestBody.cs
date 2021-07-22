using data.collection.util.DI;

namespace ZR.DataHunter.Api
{
    public class RequestBody : IScopedDependency
    {
        public string Body { get; set; }
    }
}
