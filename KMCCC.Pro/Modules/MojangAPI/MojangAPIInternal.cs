using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace KMCCC.Pro.Modules.MojangAPI
{
    public class MojangAPIInternal : IMojangAPI
    {
        #region 获取服务器状态
        public Dictionary<string, ServiceStatus> GetServiceStatus()
        {
            Dictionary<string, ServiceStatus> status = new Dictionary<string, ServiceStatus>();
            try
            {
                using (WebClient webclient = new WebClient())
                {
                    webclient.Headers.Add("Content-Type", "application/json");
                    var value = JsonConvert.DeserializeObject<JArray>(webclient.DownloadString(MojangAPIProvider.apiStatus()));
                    foreach (JObject sta in value)
                    {
                        var key = sta.Properties().Select(x => x.ToString()).First();
                        status.Add(key, (ServiceStatus)Enum.Parse(typeof(ServiceStatus), sta[key].ToString()));
                    }
                    return status;
                }
            }
            catch(Exception ex)
            {
                /*
                status.Add(MojangAPIProvider.apiStatus(), ServiceStatus.red);
                return status;
                */
                throw ex;
            }
        }
        #endregion

        #region 获取销量
        public Statistics GetStatistics()
        {
            try
            {
                using (WebClient webclient = new WebClient())
                {
                    webclient.Headers.Add("Content-Type", "application/json");
                    JObject data = new JObject
                    {
                        ["metricKeys"] = new JObject()
                    };
                    JObject metricKey_sdata = new JObject
                    {
                        MetricKeys.ITEM_SOLD_MINECRAFT,
                        MetricKeys.PREPAID_CARD_REDEEMED_MINECRAFT
                    };
                    data["metricKeys"] = metricKey_sdata;
                    var value = JsonConvert.DeserializeObject<JObject>(webclient.UploadString(MojangAPIProvider.Statistics(), data.ToString()));
                    return new Statistics(long.Parse(value[0].ToString()), long.Parse(value[1].ToString()), double.Parse(value[2].ToString()));
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public string NameToUUID(string userName)
        {
            try
            {
                using (WebClient webclient = new WebClient())
                {
                    var value = JsonConvert.DeserializeObject<JObject>(webclient.DownloadString(MojangAPIProvider.NameToUuid(userName)));
                    return value["id"].ToString();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
