using System;
using DotnetSpider.DataFlow.Parser;
using DotnetSpider.DataFlow.Storage;
using DotnetSpider.Selector;

namespace data.collection.entity.DataCollect
{
    [EntitySelector(Expression = ".//tr[@class='GV_TR']", Type = SelectorType.XPath)]
    public class YJMachineCollectEntity : EntityBase<YJMachineCollectEntity>
    {
        public string Number { get; set; }
        public string DeviceType { get; set; }
        public DateTime CompleteTime { get; set; }

        /// <summary>
        /// 梁型
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
        /// 注浆类型
        /// </summary>
        public string GroutingType { get; set; }

        /// <summary>
        /// 注浆结果
        /// </summary>
        public string GroutingResult { get; set; }

        /// <summary>
        /// 进浆压力
        /// </summary>
        public string InnerGroutingPressure { get; set; }

        /// <summary>
        /// 返浆压力
        /// </summary>
        public string OutGroutingPressure { get; set; }

        /// <summary>
        /// 保压时长
        /// </summary>
        public string PressureTime { get; set; }
    }
}