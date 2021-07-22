using System;
using System.Collections.Generic;
using System.Reflection;

namespace data.collection.util.Extentions
{
    public static partial class Extention
    {
        private static BindingFlags _bindingFlags { get; }
            = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static;

        public static Dictionary<string, object> ObjectToDictionary(this object source)
        {
            try
            {
                var res = new Dictionary<string, object>();
                foreach (var type in source.GetType().GetProperties())
                {
                    if (DateTime.TryParse(type.GetValue(source).ToString(), out var dateTime))
                    {
                        res.Add(type.Name, dateTime.GetTimeStamp());
                    }
                    else if (type.GetType().IsPrimitive)
                    {
                        var _className = type.GetType().Name;
                        foreach (var prop in type.GetType().GetProperties())
                        {
                            res.Add($"{_className}{prop.Name}", prop.GetValue(type));
                        }
                    }
                    else
                    {
                        res.Add(type.Name, type.GetValue(source).ToString());
                    }
                }

                return res;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        /// <summary>
        /// 是否拥有某属性
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="propertyName">属性名</param>
        /// <returns></returns>
        public static bool ContainsProperty(this object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName, _bindingFlags) != null;
        }

        /// <summary>
        /// 获取某属性值
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="propertyName">属性名</param>
        /// <returns></returns>
        public static object GetPropertyValue(this object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName, _bindingFlags).GetValue(obj);
        }

        /// <summary>
        /// 设置某属性值
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static void SetPropertyValue(this object obj, string propertyName, object value)
        {
            obj.GetType().GetProperty(propertyName, _bindingFlags).SetValue(obj, value);
        }
    }
}