using Microsoft.AspNetCore.Mvc;
using data.collection.api.Filters;
using data.collection.util.Extentions;
using data.collection.util.IdWorker;
using Microsoft.AspNetCore.Authorization;

namespace data.collection.api.Controllers
{
    [ApiController]
    [FormatResponse]
    [Authorize]
    public class BaseController : ControllerBase
    {
        public BaseController(SnowWorker snowWorker)
        {
            _snowWorker = snowWorker;
        }

        private readonly SnowWorker _snowWorker;

        protected void InitEntity(object obj)
        {
            if (obj.ContainsProperty("Id"))
                obj.SetPropertyValue("Id", _snowWorker.GetId());
        }
    }
}