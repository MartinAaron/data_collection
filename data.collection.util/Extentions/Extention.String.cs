using System;
using System.Security.Cryptography;
using System.Text;

namespace data.collection.util.Extentions
{
    public static partial class Extention
    {
        public static string CleanString(this string newStr)
        {
            return newStr.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", "");
        }
        
        public static string ToMD5String(this string str)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.UTF8.GetBytes(str);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("x2"));
            }
            md5.Dispose();

            return sb.ToString();
        }

        /// <summary>
        /// 判断是否为Null或者空
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this object obj)
        {
            if (obj == null)
                return true;
            else
            {
                string objStr = obj.ToString();
                return string.IsNullOrEmpty(objStr);
            }
        }

        public static long GetTimeStamp(this DateTime dt)
        {
            var dateStart = new DateTime(1970, 1, 1, 8, 0, 0);
            var timeStamp = (long) ((dt - dateStart).TotalMilliseconds);
            return timeStamp;
        }

        public static DateTime StampToDateTime(this string timeStamp)
        {
            var dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000"); //因为知道了位数是13位，需在后面加4个0，变成17位。
            var toNow = new TimeSpan(lTime);
            var time = dateTimeStart.Add(toNow);
            return time;
        }
    }
}