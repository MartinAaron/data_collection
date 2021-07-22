using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using data.collection.util.Primitives;
using ZR.DataHunter.Util;

namespace data.collection.api.Filters
{
    /// <summary>
    /// 若Action返回对象为自定义对象,则将其转为JSON
    /// </summary>
    public class FormatResponseAttribute : BaseActionFilterAsync
    {
        public override async Task OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is EmptyResult)
                context.Result = Success();
            else if (context.Result is ObjectResult res)
            {
                if (res.Value is AjaxResult)
                    context.Result = JsonContent(res.Value.ToJson());
                else
                    context.Result = Success(res.Value);
            }

            await Task.CompletedTask;
        }
    }
}