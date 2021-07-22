using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using data.collection.entity.DataCollect;
using data.collection.util.ApiClient;
using data.collection.util.Extentions;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace data.collect.Spider.ZLHtmlNews
{
    public class ZLHtmlNews : IZLHtmlNews
    {
        public ZLHtmlNews(ApiClient apiClient, ILogger<ZLHtmlNews> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }

        private readonly ApiClient _apiClient;
        private readonly ILogger<ZLHtmlNews> _logger;

        public async Task<IList<ZJMachineCollectionEntity>> GetZLHtmlData()
        {
            try
            {
                var res = new List<ZJMachineCollectionEntity>();

                var str = await _apiClient.GetDatatoRemote(
                    "http://ali2.vect.com.cn:8083/web/ajax_web/MainAjax.ashx?PostType=LogOn&LogName=ljqm5&LogPwd=222",
                    "");

                var zlNewsStr = await _apiClient.GetDatatoRemote(
                    "http://ali2.vect.com.cn:8083/web/record/tension_result_list.aspx", "");

                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(zlNewsStr);
                await Task.CompletedTask;
                var node = htmlDoc.DocumentNode.SelectNodes("//table[@id='GV']/tr[@class='GV_TR']").ToList();
                var tRes = new List<HtmlNode>();
                foreach (var item in node)
                {
                    var entity = new ZJMachineCollectionEntity();
                    var childNode = item.ChildNodes.Where(x => !x.InnerText.IsNullOrEmpty()).ToList();
                    childNode.ForEach(x =>
                    {
                        if (x.InnerText.CleanString() != "")
                        {
                            tRes.Add(x);
                        }
                    });
                    var deviceString = tRes[tRes.Count - 4].InnerHtml.CleanString();
                    var device_id = deviceString.Substring(deviceString.IndexOf('(') + 1,
                        deviceString.IndexOf(')') - deviceString.IndexOf('(') - 1);
                    // 获取详细信息
                    var deviceStr = await _apiClient.GetDatatoRemote(
                        $"http://ali2.vect.com.cn:8083/web/record/tension_result.aspx?SingleTensionId={device_id}", "");

                    var deviceDoc = new HtmlDocument();
                    deviceDoc.LoadHtml(deviceStr);
                    await Task.CompletedTask;

                    #region 1、获取页面 input

                    var deviceNode = deviceDoc.DocumentNode
                        .SelectNodes(
                            "//*[@id='form1']/table[@class='myTable']//td/table[@class='myTable']/tr//td[2]/table[@class='myTable']/tr");
                    foreach (var detail in deviceNode)
                    {
                        var device = detail.ChildNodes.Where(x => !x.InnerText.IsNullOrEmpty()).ToList();
                        device.ForEach(x =>
                        {
                            var innerNodes = x.SelectNodes("//input").ToList();
                            entity.DeviceName = innerNodes[3].GetAttributeValue("value", "");
                            entity.LType = innerNodes[4].GetAttributeValue("value", "");
                            entity.LNumber = innerNodes[5].GetAttributeValue("value", "");
                            entity.HoleNumber = innerNodes[6].GetAttributeValue("value", "");
                            entity.CompleteTime = Convert.ToDateTime(innerNodes[7].GetAttributeValue("value", ""));
                            entity.ZLType = innerNodes[8].GetAttributeValue("value", "");
                            entity.ZLMode = innerNodes[9].GetAttributeValue("value", "");
                            entity.CompleteResult = innerNodes[10].GetAttributeValue("value", "");
                        });
                    }

                    #endregion

                    #region 2、获取 table

                    var deviceParamList = new List<HtmlNode>();
                    var deviceParams = deviceDoc.DocumentNode.SelectNodes("//table[@id='GV']/tr[@class='GV_TR']")
                        .ToList();
                    foreach (var param in deviceParams)
                    {
                        var cleanParam = param.ChildNodes.Where(x => !x.InnerText.IsNullOrEmpty()).ToList();
                        cleanParam.ForEach(x =>
                        {
                            if (x.InnerText.CleanString() != "")
                            {
                                deviceParamList.Add(x);
                            }
                        });
                    }

                    entity.A1Width = new Detail()
                    {
                        _10 = double.Parse(deviceParamList[1].InnerText.CleanString()),
                        _20 = double.Parse(deviceParamList[2].InnerText.CleanString()),
                        _50 = double.Parse(deviceParamList[3].InnerText.CleanString()),
                        _100 = double.Parse(deviceParamList[4].InnerText.CleanString()),
                    };
                    entity.A1Pressure = new Detail()
                    {
                        _10 = double.Parse(deviceParamList[9].InnerText.CleanString()),
                        _20 = double.Parse(deviceParamList[10].InnerText.CleanString()),
                        _50 = double.Parse(deviceParamList[11].InnerText.CleanString()),
                        _100 = double.Parse(deviceParamList[12].InnerText.CleanString()),
                    };

                    entity.A1Power = new Detail()
                    {
                        _10 = double.Parse(deviceParamList[17].InnerText.CleanString()),
                        _20 = double.Parse(deviceParamList[18].InnerText.CleanString()),
                        _50 = double.Parse(deviceParamList[19].InnerText.CleanString()),
                        _100 = double.Parse(deviceParamList[20].InnerText.CleanString()),
                    };
                    
                    entity.A2Width = new Detail()
                    {
                        _10 = double.Parse(deviceParamList[25].InnerText.CleanString()),
                        _20 = double.Parse(deviceParamList[26].InnerText.CleanString()),
                        _50 = double.Parse(deviceParamList[27].InnerText.CleanString()),
                        _100 = double.Parse(deviceParamList[28].InnerText.CleanString()),
                    };

                    entity.A2Pressure = new Detail()
                    {
                        _10 = double.Parse(deviceParamList[33].InnerText.CleanString()),
                        _20 = double.Parse(deviceParamList[34].InnerText.CleanString()),
                        _50 = double.Parse(deviceParamList[35].InnerText.CleanString()),
                        _100 = double.Parse(deviceParamList[36].InnerText.CleanString()),
                    };

                    entity.A2Power = new Detail()
                    {
                        _10 = double.Parse(deviceParamList[41].InnerText.CleanString()),
                        _20 = double.Parse(deviceParamList[42].InnerText.CleanString()),
                        _50 = double.Parse(deviceParamList[43].InnerText.CleanString()),
                        _100 = double.Parse(deviceParamList[44].InnerText.CleanString()),
                    };

                    entity.B1Width = new Detail()
                    {
                        _10 = double.Parse(deviceParamList[49].InnerText.CleanString()),
                        _20 = double.Parse(deviceParamList[50].InnerText.CleanString()),
                        _50 = double.Parse(deviceParamList[51].InnerText.CleanString()),
                        _100 = double.Parse(deviceParamList[52].InnerText.CleanString()),
                    };

                    entity.B1Pressure = new Detail()
                    {
                        _10 = double.Parse(deviceParamList[57].InnerText.CleanString()),
                        _20 = double.Parse(deviceParamList[58].InnerText.CleanString()),
                        _50 = double.Parse(deviceParamList[59].InnerText.CleanString()),
                        _100 = double.Parse(deviceParamList[60].InnerText.CleanString()),
                    };

                    entity.B1Power = new Detail()
                    {
                        _10 = double.Parse(deviceParamList[65].InnerText.CleanString()),
                        _20 = double.Parse(deviceParamList[66].InnerText.CleanString()),
                        _50 = double.Parse(deviceParamList[67].InnerText.CleanString()),
                        _100 = double.Parse(deviceParamList[68].InnerText.CleanString()),
                    };

                    entity.B2Width = new Detail()
                    {
                        _10 = double.Parse(deviceParamList[73].InnerText.CleanString()),
                        _20 = double.Parse(deviceParamList[74].InnerText.CleanString()),
                        _50 = double.Parse(deviceParamList[75].InnerText.CleanString()),
                        _100 = double.Parse(deviceParamList[76].InnerText.CleanString()),
                    };

                    entity.B2Pressure = new Detail()
                    {
                        _10 = double.Parse(deviceParamList[81].InnerText.CleanString()),
                        _20 = double.Parse(deviceParamList[82].InnerText.CleanString()),
                        _50 = double.Parse(deviceParamList[83].InnerText.CleanString()),
                        _100 = double.Parse(deviceParamList[84].InnerText.CleanString()),
                    };

                    entity.B2Power = new Detail()
                    {
                        _10 = double.Parse(deviceParamList[89].InnerText.CleanString()),
                        _20 = double.Parse(deviceParamList[90].InnerText.CleanString()),
                        _50 = double.Parse(deviceParamList[91].InnerText.CleanString()),
                        _100 = double.Parse(deviceParamList[92].InnerText.CleanString()),
                    };

                    #endregion

                    res.Add(entity);
                }

                return res;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}