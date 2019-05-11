using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace 百度翻译接口的调用.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            //sign=f89f9594663708c1605f3d736d01d2d4
            return View();
        }


        // 将英文转换为中文
        public ActionResult TranslateToZH(string inputText)
        {
           
            string url = "http://api.fanyi.baidu.com/api/trans/vip/translate?appid=xxx&from=en&to=zh&";

            if (!string.IsNullOrEmpty(inputText))
            {
                url += "q=" + inputText;
            }
            else
            {
                return Json(new { msg = "no", data = "" }, JsonRequestBehavior.AllowGet);
                url += "q=sorry";
            }

            Random r = new Random();
            string salt = "";
            for (int i = 0; i < 9; i++)
            {
                salt += r.Next(1, 11);
            }
            url += "&salt=" + salt;

            string sign = "xxx" + inputText + salt + "you salt";
            //sign = sign.ToLower();
            Console.WriteLine(sign);
            sign = GetMd5Hash(sign).ToLower();

            url += "&sign=" + sign;

            HttpClient client = new HttpClient();
            string result = client.GetStringAsync(url).Result;

            var dataJson = JsonConvert.DeserializeObject<Model>(result);
            return Json(new {msg="ok", data=dataJson }, JsonRequestBehavior.AllowGet);

        }
        // 将英文转换为中文
        public ActionResult TranslateToEN(string inputText)
        {
            string url = "http://api.fanyi.baidu.com/api/trans/vip/translate?appid=xxx&from=zh&to=en&";

            if (!string.IsNullOrEmpty(inputText))
            {
                url += "q=" + inputText;
            }
            else
            {
                return Json(new { msg = "no", data = "" }, JsonRequestBehavior.AllowGet);
                url += "q=sorry";
            }

            Random r = new Random();
            string salt = "";
            for (int i = 0; i < 9; i++)
            {
                salt += r.Next(1, 11);
            }
            url += "&salt=" + salt;

            string sign = "xxx" + inputText + salt + "you salt";
            //sign = sign.ToLower();
            Console.WriteLine(sign);
            sign = GetMd5Hash(sign).ToLower();

            url += "&sign=" + sign;

            HttpClient client = new HttpClient();
            string result = client.GetStringAsync(url).Result;

            var dataJson = JsonConvert.DeserializeObject<Model>(result);
            return Json(new { msg = "ok", data = dataJson }, JsonRequestBehavior.AllowGet);
        }

        public static string GetMd5Hash(String input)
        {
            if (input == null)
            {
                return null;
            }

            MD5 md5Hash = MD5.Create();

            // 将输入字符串转换为字节数组并计算哈希数据
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // 创建一个 Stringbuilder 来收集字节并创建字符串
            StringBuilder sBuilder = new StringBuilder();

            // 循环遍历哈希数据的每一个字节并格式化为十六进制字符串
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // 返回十六进制字符串
            return sBuilder.ToString();
        }
    }

    public class Model
    {
        public string from { get; set; }
        public string to { get; set; }
        public List<ResultModel> trans_result { get; set; }
    }

    public class ResultModel
    {
        public string src { get; set; }
        public string dst { get; set; }
    }
}
