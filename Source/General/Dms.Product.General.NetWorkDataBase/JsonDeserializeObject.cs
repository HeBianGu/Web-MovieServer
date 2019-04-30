using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dms.Product.General.NetWorkDataBase
{
    /// <summary>
    /// JContainer解析为相应的数据Model,即序列化json
    /// </summary>
    /// <typeparam name="T">数据Model</typeparam>
    public class JsonDeserializeObject<T>
    {
        /// <summary>
        /// 解析json为相应的ListModel
        /// </summary>
        /// <param name="jsonResult">待解析的Json</param>
        /// <param name="errorInfor">若发生错误，则返回错误信息</param>
        /// <returns>解析json为相应的ListModel</returns>
        public static List<T> getListT(JContainer jsonResult, ref string errorInfor)
        {
            if (jsonResult != null)
            {
                if (jsonResult["code"].ToString().Equals("0"))
                {
                    JObject jsonObj = JObject.Parse(jsonResult.ToString());

                    if (jsonObj.Property("data") != null)
                    {
                        JArray userInfos = (JArray)jsonResult["data"];

                        var items = JsonConvert.DeserializeObject<List<T>>(userInfos.ToString());

                        if (items.Count == 0) return null;

                        return items;
                    }
                }
                else errorInfor += jsonResult["message"].ToString();
            }
            else errorInfor += "   原因：网络异常，请检查网络连接";
            return null;
        }
        /// <summary>
        /// 解析json为相应的ListModel
        /// </summary>
        /// <param name="jsonResult">待解析的Json</param>
        ///  <param name="total">总数</param>
        /// <param name="errorInfor">若发生错误，则返回错误信息</param>
        /// <returns>解析json为相应的ListModel</returns>
        public static List<T> getListT(JContainer jsonResult, out uint total, ref string errorInfor)
        {
            total = 0;
            if (jsonResult != null)
            {
                if (jsonResult["code"].ToString().Equals("0"))
                {
                    JObject jsonObj = JObject.Parse(jsonResult.ToString());

                    if (jsonObj.Property("data") != null)
                    {
                        JObject data = (JObject)jsonObj["data"];

                        total = uint.Parse(data["total"].ToString());

                        if (data.Property("data") != null)
                        {
                            JArray userInfos = (JArray)data["data"];

                            var items = JsonConvert.DeserializeObject<List<T>>(userInfos.ToString());

                            if (items.Count == 0) return null;

                            return items;
                        }
                    }
                }
                else errorInfor += jsonResult["message"].ToString();
            }
            else errorInfor += "   原因：网络异常，请检查网络连接";
            return null;
        }
        /// <summary>
        /// 解析json为相应的Model
        /// </summary>
        /// <param name="jsonResult">待解析的Json</param>
        /// <param name="errorInfor">若发生错误，则返回错误信息</param>
        /// <returns>解析json为相应的Model</returns>
        public static T getT(JContainer jsonResult, ref string errorInfor)
        {
            if (jsonResult == null)
            {
                errorInfor += "   原因：网络异常，请检查网络连接";
                return default(T);
            }
            string code = jsonResult["code"].ToString();//返回码
            if (!code.Equals("0"))
            {
                errorInfor += jsonResult["message"].ToString();
                return default(T);
            }
            JObject jsonObj = JObject.Parse(jsonResult.ToString());
            if (jsonObj.Property("data") == null) return default(T);
            JObject jvalue = JObject.Parse(jsonObj["data"].ToString());
            if (jvalue == null) return default(T);
            return JsonConvert.DeserializeObject<T>(jvalue.ToString());
        }
    }
}