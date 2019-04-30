using Dms.Product.Base.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dms.Product.General.NetWorkDataBase
{
    public class NetWorkService
    {
        HttpPostHelper _httpPostHelper = new HttpPostHelper();

        BaseURL _base = new BaseURL();

        Tuple<string, string, string, string> ConvertJContainer(JContainer jsonResult, string err)
        {
            if (jsonResult != null)
            {
                Func<object, string> func = l => l == null ? null : l.ToString();

                Tuple<string, string, string, string> result = new Tuple<string, string, string, string>(func(jsonResult["code"]),
                    func(jsonResult["msg"]), func(jsonResult["data"]), err);

                return result;
            }
            else
            {
                return null;
            }
        }

        string ConvertToDataJContainer(JContainer jsonResult, string err)
        {
            if (jsonResult != null)
            {
                return jsonResult.ToString();
            }
            else
            {
                return null;
            }
        }

        public string Post(string url, Dictionary<string, string> dic, out string err)
        {

            JContainer jContainer;

            //string url = _base.GetServiceUrl(type);

            _httpPostHelper.PostData(url, dic, out jContainer, out err);

            if (jContainer == null)
            {
                return null;
            }

            return this.ConvertToDataJContainer(jContainer, err);
        }

        public string Get(string url, Dictionary<string, string> dic, out string err)
        {
            err = string.Empty;

            JContainer jContainer;

            //string url = _base.GetServiceUrl(type);

            StringBuilder sb = new StringBuilder();

            foreach (var item in dic)
            {
                sb.Append(item.Key + "=" + item.Value + "&");
            }

            jContainer = _httpPostHelper.SendDataByGET(url, sb.ToString().TrimEnd('&'), out err);

            if (jContainer == null)
            {
                return null;
            }

            return this.ConvertToDataJContainer(jContainer, err);
        }

        public string Get(URLEnum type, Dictionary<string, string> dic, out string err)
        {

            JContainer jContainer;

            string url = _base.GetServiceUrl(type);

            return this.Get(url, dic, out err);

        }

        public string Post(URLEnum type, Dictionary<string, string> dic, out string err)
        {

            JContainer jContainer;

            string url = _base.GetServiceUrl(type);

            _httpPostHelper.PostData(url, dic, out jContainer, out err);

            if (jContainer == null)
            {
                return null;
            }

            return this.ConvertToDataJContainer(jContainer, err);
        }


        const string key = "chzhjkkj123456./";

        public async Task<HistoryRoot> GetHistDataCollection(string sn, string startTime, string endTime)
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

            keyValuePairs.Add("key", key);
            keyValuePairs.Add("sn", sn);
            keyValuePairs.Add("startTime", startTime);
            keyValuePairs.Add("endTime", endTime);

            return await Task.Run(() =>
             {
                 string err;

                 var result = this.Post(URLEnum.FindPastRecordsData, keyValuePairs, out err);

                 if (result == null) return null;

                 HistoryRoot model = JsonConvert.DeserializeObject<HistoryRoot>(result);

                 model.message = err;

                 return model;
             });
        }

        public async Task<NetWorkDataCollection<T>> GetDataBase<T>(URLEnum url)
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

            keyValuePairs.Add("key", key);

            return await Task.Run(() =>
            {
                string err;

                var result = this.Post(url, keyValuePairs, out err);

                if (result == null) return null;

                NetWorkDataCollection<T> model = JsonConvert.DeserializeObject<NetWorkDataCollection<T>>(result);

                model.message = err;
                 
                return model;
            });
        }

    }
}
