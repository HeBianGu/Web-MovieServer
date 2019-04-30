using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Dms.Product.General.NetWorkDataBase
{
    /// <summary>
    /// 通过post获取数据帮助类
    /// </summary>
    public class HttpPostHelper
    {
        private CookieContainer cookieContainer;

        private Dictionary<string, Cookie> cacheCookies = new Dictionary<string, Cookie>();

        /// <summary>
        /// 通过post获取数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="paras"></param>
        /// <param name="jsonresult"></param>
        /// <param name="errorInfor">如果发生错误，则返回错误信息</param>
        /// <returns></returns>
        public bool PostData(string url, IDictionary<string, string> paras, out JContainer jsonResult, out string errorInfor, bool isJson = false)
        {
            bool re = false;
            jsonResult = null;
            HttpWebRequest request = null;
            string result = "";
            errorInfor = "";
            try
            {
                request = HttpWebRequest.Create(url) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; .NET CLR 1.0.3705;)";
                request.Accept = "*/*";
                request.KeepAlive = true;
                request.ProtocolVersion = HttpVersion.Version10;
                request.CookieContainer = cookieContainer;
                request.Timeout = 30000;

                if (paras != null && paras.Count > 0)
                {
                    if (isJson == true)
                    {
                        string jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(paras);
                        byte[] data = Encoding.UTF8.GetBytes(jsonStr.ToString());
                        using (System.IO.Stream stream = request.GetRequestStream())
                        {
                            stream.Write(data, 0, data.Length);
                        }
                    }
                    else
                    {
                        StringBuilder buffer = new StringBuilder();
                        foreach (string key in paras.Keys)
                        {
                            if (buffer.Length == 0)
                            {
                                buffer.AppendFormat("{0}={1}", key, paras[key]);
                            }
                            else
                            {
                                buffer.AppendFormat("&{0}={1}", key, paras[key]);
                            }
                        }
                        byte[] data = Encoding.UTF8.GetBytes(buffer.ToString());

                        using (System.IO.Stream stream = request.GetRequestStream())
                        {
                            stream.Write(data, 0, data.Length);
                        }
                    }
                }

                using (HttpWebResponse wr = request.GetResponse() as HttpWebResponse)
                {
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(wr.GetResponseStream()))
                    {
                        foreach (Cookie c in wr.Cookies)
                        {
                            if (c.Expired) cacheCookies.Remove(c.Name);
                            else cacheCookies[c.Name] = c;
                        }
                        result = sr.ReadToEnd();
                        re = true;
                        sr.Close();
                    }
                    wr.Close();
                }

                try
                {
                    jsonResult = Newtonsoft.Json.JsonConvert.DeserializeObject(result) as Newtonsoft.Json.Linq.JContainer;

                    System.Diagnostics.Debug.WriteLine(jsonResult);
                }
                catch (Exception ex)
                {
                    re = false;
                    errorInfor = string.Format("url:{0}返回无效json字符串:{1}", url, result);
                }
            }
            catch (Exception ex)
            {
                errorInfor = ex.Message.ToString();
            }
            finally
            {
                if (request != null) request.Abort();
            }
            return re;
        }


        /// <summary>
        /// 通过post上传
        /// </summary>
        public JContainer Post(string Url, string jsonParas)
        {
            try
            {
                string strURL = Url;
                HttpWebRequest request;
                request = (HttpWebRequest)WebRequest.Create(strURL);
                request.Method = "POST";
                request.ContentType = "application/json;charset=UTF-8";
                request.CookieContainer = cookieContainer;
                request.Timeout = 30000;
                string paraUrlCoded = jsonParas;
                byte[] payload;
                payload = System.Text.Encoding.UTF8.GetBytes(paraUrlCoded);
                request.ContentLength = payload.Length;
                Stream writer = request.GetRequestStream();
                writer.Write(payload, 0, payload.Length);
                writer.Close();
                System.Net.HttpWebResponse response;
                response = (System.Net.HttpWebResponse)request.GetResponse();
                System.IO.Stream s;
                s = response.GetResponseStream();
                string StrDate = "";
                string strValue = "";
                StreamReader Reader = new StreamReader(s, Encoding.UTF8);
                while ((StrDate = Reader.ReadLine()) != null)
                {
                    strValue += StrDate + "\r\n";
                }
                return Newtonsoft.Json.JsonConvert.DeserializeObject(strValue) as Newtonsoft.Json.Linq.JContainer;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public string postSend(string url, string param)
        {
            Encoding myEncode = Encoding.GetEncoding("UTF-8");
            byte[] postBytes = Encoding.UTF8.GetBytes(param);

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/soap+xml; charset=utf-8";
            req.ContentLength = postBytes.Length;

            try
            {
                using (Stream reqStream = req.GetRequestStream())
                {
                    reqStream.Write(postBytes, 0, postBytes.Length);
                }
                using (WebResponse res = req.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(res.GetResponseStream(), myEncode))
                    {
                        string strResult = sr.ReadToEnd();
                        return strResult;
                    }
                }
            }
            catch (WebException ex)
            {
                return "无法连接到服务器\r\n错误信息：" + ex.Message;
            }
        }


        #region 同步通过GET方式发送数据
        /// <summary>
        /// 通过GET方式发送数据
        /// </summary>
        /// <param name="Url">url</param>
        /// <param name="postDataStr">GET数据</param>
        /// <param name="cookie">GET容器</param>
        /// <returns></returns>
        public JContainer SendDataByGET(string Url, string postDataStr, out string err)
        {
            err = string.Empty;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);

                //if (cookie.Count == 0)
                //{
                //    request.CookieContainer = new CookieContainer();
                //    cookie = request.CookieContainer;
                //}
                //else
                //{
                //    request.CookieContainer = cookie;
                //}

                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();

                return Newtonsoft.Json.JsonConvert.DeserializeObject(retString) as JContainer;


            }
            catch (Exception ex)
            {
                err = ex.Message.ToString();
                return null;
            }
        }
        #endregion
    }
}
