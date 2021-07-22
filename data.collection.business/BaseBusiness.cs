using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using data.collection.entity.Model;

namespace data.collection.business
{
    public class BaseBusiness
    {
        protected async Task<List<SelectOption>> GetEnumSelectOption(Type type)
        {
            var values = Enum.GetValues(type);
            var list = new List<SelectOption>();
            foreach (var aValue in values)
            {
                list.Add(new SelectOption
                {
                    Value = ((int) aValue).ToString(),
                    Text = aValue.ToString()
                });
            }

            await Task.CompletedTask;
            return list;
        }
    }
}