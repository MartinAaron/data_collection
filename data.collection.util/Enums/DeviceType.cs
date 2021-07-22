using System.ComponentModel;

namespace data.collection.util.Enums
{
    public enum DeviceType
    {
        [Description("tensioning_machine")] 张拉机 = 1,
        [Description("grouting_machine")] 压浆机 = 2,
        [Description("personnel_positioning")] 人员定位 = 3,
        [Description("bridge_machine")] 架桥机 = 4,
        [Description("gantry_crane")] 龙门吊 = 5,
        [Description("project_transport")] 工程车辆定位 = 6,
        [Description("types_working_time")] 分类及工时情况 = 7,
    }
}