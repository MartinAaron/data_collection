using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using data.collection.entity.DataCollect;
using data.collection.util.ApiClient;
using data.collection.util.Extentions;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;

namespace data.collect.Spider.YJHtmlNews
{
    public class YJHtmlNews : IYJHtmlNews
    {
        public YJHtmlNews(ILogger<YJHtmlNews> logger, ApiClient apiClient)
        {
            _logger = logger;
            _apiClient = apiClient;
        }

        private readonly ILogger<YJHtmlNews> _logger;
        private readonly ApiClient _apiClient;

        public async Task<IList<YJMachineCollectEntity>> GetYJHtmlData()
        {
            try
            {
                await _apiClient.GetDatatoRemote(
                    "http://ali2.vect.com.cn:8083/web/ajax_web/MainAjax.ashx?PostType=LogOn&LogName=lj5&LogPwd=222",
                    "");

                var yjNewsStr = await _apiClient.GetDatatoRemote(
                    "http://ali2.vect.com.cn:8083/web/record/grout_result_list.aspx", "");

                // total page 
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(yjNewsStr);

                await Task.CompletedTask;

                var node = htmlDoc.DocumentNode.SelectNodes("//table[@id='GV']/tr[@class='GV_TR']").ToList();
                
                var list = GetResByHtml(node);
                
                return list;
            }
            catch (System.Exception ex)
            {
                _logger.LogWarning(ex.Message);
                return null;
            }
        }

        private List<YJMachineCollectEntity> GetResByHtml(List<HtmlNode> node)
        {
            var res = new List<YJMachineCollectEntity>();
            foreach (var item in node)
            {
                var tRes = new List<HtmlNode>();
                var childNode = item.ChildNodes.Where(x => !x.InnerText.IsNullOrEmpty()).ToList();
                childNode.ForEach(x =>
                {
                    if (x.InnerText.CleanString() != "")
                    {
                        tRes.Add(x);
                    }
                });
                //TODO 增加材料质量
                res.Add(new YJMachineCollectEntity
                {
                    Number = tRes[0].InnerText.CleanString(),
                    DeviceType = tRes[1].InnerText.CleanString(),
                    CompleteTime = Convert.ToDateTime(tRes[2].InnerText),
                    LType = tRes[3].InnerText.CleanString(),
                    LNumber = tRes[4].InnerText.CleanString(),
                    HoleNumber = tRes[5].InnerText.CleanString(),
                    GroutingType = tRes[6].InnerText.CleanString(),
                    GroutingResult = tRes[7].InnerText.CleanString(),
                    InnerGroutingPressure = tRes[8].InnerText.CleanString(),
                    OutGroutingPressure = tRes[9].InnerText.CleanString(),
                    PressureTime = tRes[10].InnerText.CleanString()
                });
            }

            return res;
        }
    }
}