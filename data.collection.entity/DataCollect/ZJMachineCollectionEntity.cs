using System;
using Newtonsoft.Json;

namespace data.collection.entity.DataCollect
{
    public class ZJMachineCollectionEntity
    {
        public string DeviceName { get; set; }

        /// <summary>
        /// 粱型
        /// </summary>
        public string LType { get; set; }

        /// <summary>
        /// 梁号
        /// </summary>
        public string LNumber { get; set; }

        /// <summary>
        /// 孔号
        /// </summary>
        public string HoleNumber { get; set; }

        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTime CompleteTime { get; set; }

        /// <summary>
        /// 张拉类型
        /// </summary>
        public string ZLType { get; set; }

        /// <summary>
        /// 张拉模式
        /// </summary>
        public string ZLMode { get; set; }

        /// <summary>
        /// 完成结果
        /// </summary>
        public string CompleteResult { get; set; }

        public Detail A1Width { get; set; }
        public Detail A1Pressure { get; set; }
        public Detail A1Power { get; set; }

        public Detail A2Width { get; set; }
        public Detail A2Pressure { get; set; }
        public Detail A2Power { get; set; }

        public Detail B1Width { get; set; }
        public Detail B1Pressure { get; set; }
        public Detail B1Power { get; set; }

        public Detail B2Width { get; set; }
        public Detail B2Pressure { get; set; }
        public Detail B2Power { get; set; }
    }

    public class Detail
    {
        public double _10 { get; set; }
        public double _20 { get; set; }
        public double _50 { get; set; }
        public double _100 { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}