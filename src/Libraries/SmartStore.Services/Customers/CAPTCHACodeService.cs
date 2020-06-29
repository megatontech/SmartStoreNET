using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using SmartStore.Core;
using SmartStore.Core.Data;
using SmartStore.Core.Domain.Customers;
using SmartStore.Core.Events;
using SmartStore.Core.Localization;
using SmartStore.Services.Messages;
using SmartStore.Services.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
namespace SmartStore.Services.Customers
{
    public partial class CAPTCHACodeService : ICAPTCHACodeService
    {
        #region Private Fields

        private readonly IRepository<CAPTCHACode> _checkRepository;

        public CAPTCHACodeService(IRepository<CAPTCHACode> checkRepository)
        {
            _checkRepository = checkRepository;
        }

        public void Insert(CAPTCHACode model)
        {
            _checkRepository.Insert(model);
        }
        public bool Send(string mobile)
        {
            bool isSuccess = false;
            if (Get(mobile).Count >= 5)
            {
                isSuccess = false;
            }
            else
            {
                var item = new CAPTCHACode();
                item.Mobile = mobile;
                item.IsUsed = false;
                item.SendDate = DateTime.Now;
                string vc = "";
                Random r = new Random();
                int num1 = r.Next(0, 9);
                int num2 = r.Next(0, 9);
                int num3 = r.Next(0, 9);
                int num4 = r.Next(0, 9);
                int num5 = r.Next(0, 9);
                int num6 = r.Next(0, 9);

                int[] numbers = new int[6] { num1, num2, num3, num4, num5, num6 };
                for (int i = 0; i < numbers.Length; i++)
                {
                    vc += numbers[i].ToString();
                }
                item.Code = vc;
                item.Content = SendSMS(mobile, vc);
                Insert(item);
                isSuccess = true;
            }
            return isSuccess;

        }
        public bool ValidCode(string mobile, string code)
        {
            bool isSuccess = false;
            var codes = Get(mobile);
            if (codes != null && codes.Any(x => x.Code == code))
            {
                isSuccess = true;
                foreach (var item in codes)
                {
                    item.IsUsed = true;
                    Update(item);
                }
            }
            return isSuccess;

        }
        public void Update(CAPTCHACode model)
        {
            _checkRepository.Update(model);
        }
        public List<CAPTCHACode> Get(string mobile)
        {
            var sdate = DateTime.Now.AddMinutes(-30);
            var edate = DateTime.Now;
            return _checkRepository.Table.Where(x => x.Mobile == mobile && x.SendDate >= sdate && x.SendDate <= edate).ToList();
        }

        #endregion Private Fields




        private string SendSMS(string mobile, string code)
        {
            string result = "";
            string appkey = "5fe160ac16a78d2e77b3f68d6963a761"; //配置您申请的appkey
            string url2 = "http://v.juhe.cn/vercodesms/send.php";
            var parameters2 = new Dictionary<string, string>();
            parameters2.Add("mobile", mobile); //接收短信的手机号码
            parameters2.Add("tplId", "66178"); //短信模板ID，请参考个人中心短信模板设置
            parameters2.Add("tplValue", "#code#=" + code + "&#m#=30"); //变量名和变量值对。如果你的变量名或者变量值中带有#&=中的任意一个特殊符号，请先分别进行urlencode编码后再传递，<a href="http://www.juhe.cn/news/index/id/50" target="_blank">详细说明></a>
            parameters2.Add("key", appkey);//你申请的key
            parameters2.Add("dtype", ""); //返回数据的格式,xml或json，默认json
            result = sendPost(url2, parameters2, "get");
            return result;
        }

        /// <summary>
        /// Http (GET/POST)
        /// </summary>
        /// <param name="url">请求URL</param>
        /// <param name="parameters">请求参数</param>
        /// <param name="method">请求方法</param>
        /// <returns>响应内容</returns>
        static string sendPost(string url, IDictionary<string, string> parameters, string method)
        {
            if (method.ToLower() == "post")
            {
                HttpWebRequest req = null;
                HttpWebResponse rsp = null;
                System.IO.Stream reqStream = null;
                try
                {
                    req = (HttpWebRequest)WebRequest.Create(url);
                    req.Method = method;
                    req.KeepAlive = false;
                    req.ProtocolVersion = HttpVersion.Version10;
                    req.Timeout = 5000;
                    req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                    byte[] postData = Encoding.UTF8.GetBytes(BuildQuery(parameters, "utf8"));
                    reqStream = req.GetRequestStream();
                    reqStream.Write(postData, 0, postData.Length);
                    rsp = (HttpWebResponse)req.GetResponse();
                    Encoding encoding = Encoding.GetEncoding(rsp.CharacterSet);
                    return GetResponseAsString(rsp, encoding);
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
                finally
                {
                    if (reqStream != null) reqStream.Close();
                    if (rsp != null) rsp.Close();
                }
            }
            else
            {
                //创建请求
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + "?" + BuildQuery(parameters, "utf8"));

                //GET请求
                request.Method = "GET";
                request.ReadWriteTimeout = 5000;
                request.ContentType = "text/html;charset=UTF-8";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));

                //返回内容
                string retString = myStreamReader.ReadToEnd();
                return retString;
            }
        }

        /// <summary>
        /// 组装普通文本请求参数。
        /// </summary>
        /// <param name="parameters">Key-Value形式请求参数字典</param>
        /// <returns>URL编码后的请求数据</returns>
        static string BuildQuery(IDictionary<string, string> parameters, string encode)
        {
            StringBuilder postData = new StringBuilder();
            bool hasParam = false;
            IEnumerator<KeyValuePair<string, string>> dem = parameters.GetEnumerator();
            while (dem.MoveNext())
            {
                string name = dem.Current.Key;
                string value = dem.Current.Value;
                // 忽略参数名或参数值为空的参数
                if (!string.IsNullOrEmpty(name))//&& !string.IsNullOrEmpty(value)
                {
                    if (hasParam)
                    {
                        postData.Append("&");
                    }
                    postData.Append(name);
                    postData.Append("=");
                    if (encode == "gb2312")
                    {
                        postData.Append(HttpUtility.UrlEncode(value, Encoding.GetEncoding("gb2312")));
                    }
                    else if (encode == "utf8")
                    {
                        postData.Append(HttpUtility.UrlEncode(value, Encoding.UTF8));
                    }
                    else
                    {
                        postData.Append(value);
                    }
                    hasParam = true;
                }
            }
            return postData.ToString();
        }

        /// <summary>
        /// 把响应流转换为文本。
        /// </summary>
        /// <param name="rsp">响应流对象</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>响应文本</returns>
        static string GetResponseAsString(HttpWebResponse rsp, Encoding encoding)
        {
            System.IO.Stream stream = null;
            StreamReader reader = null;
            try
            {
                // 以字符流的方式读取HTTP响应
                stream = rsp.GetResponseStream();
                reader = new StreamReader(stream, encoding);
                return reader.ReadToEnd();
            }
            finally
            {
                // 释放资源
                if (reader != null) reader.Close();
                if (stream != null) stream.Close();
                if (rsp != null) rsp.Close();
            }
        }


    }
}