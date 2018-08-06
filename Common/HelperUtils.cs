using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using ModelEx;
using ModelEx.Base;

namespace Common
{
    /// <summary>
    /// 系统公共方法
    /// </summary>
    public static class HelperUtils
    {
        static Random rd = new Random();
        /// <summary>
        /// Json转实体类
        /// </summary>
        /// <typeparam name="T"><peparam>
        /// <param name="strjson"></param>
        /// <returns></returns>
        public static T JsonTo<T>(string strjson)
        {
            JavaScriptSerializer jserializer = new JavaScriptSerializer();
            var UserDetail = jserializer.Deserialize<T>(strjson);
            return UserDetail;
        }

        /// <summary>
        /// 字符串MD5+Key 加密
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ToMD5(string data)
        {
            StringBuilder sb = new StringBuilder();
            MD5 md5 = new MD5CryptoServiceProvider();
            var Key = ""; //读取配置文件
            byte[] t = md5.ComputeHash(Encoding.UTF8.GetBytes("32333" + data + "z32!"));
            foreach (var t1 in t)
            {
                sb.Append(t1.ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString().ToUpper();
        }

        /// <summary>
        /// 检查文字是否存在
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool FileExists(string url)
        {
            return File.Exists(url);
        }

        /// <summary>
        /// 检查是否为数字
        /// </summary>
        /// <param name="values"></param>
        /// <param name="type">1整数，2正小数</param>
        /// <returns></returns>
        public static bool CheckisNum(string values, int type = 1)
        {
            var regex = type == 1 ? @"^\d+$" : "^[0-9]+(.[0-9]{1,2})?$";
            System.Text.RegularExpressions.Regex rex = new System.Text.RegularExpressions.Regex(@"^\d+$");
            return rex.IsMatch(values);
        }

        /// <summary>
        /// ToJson
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string ToJson(object o)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string str = serializer.Serialize(o);
            return str;
        }
        /// <summary>
        /// ToJson
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string ToJson2(this object o)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string str = serializer.Serialize(o);
            return str;
        }

        #region 模糊姓名-例：张三 => 张*    王老五 => 王**
        /// <summary>
        /// 模糊姓名-例：张三 => 张*    王老五 => 王**
        /// </summary>
        /// <param name="str">姓名</param>
        /// <param name="defaulVal">为空时的返回值-默认""</param>
        /// <returns></returns>
        public static string FuzzyName(this string str, string defaulVal = "")
        {
            if (!string.IsNullOrEmpty(str))
            {
                var result = str.ToList().Select((x, i) => new { result = (i == 0 ? x.ToString() : "*") }).Select(x => x.result).ToJoinStr("");
                return result;
            }
            else
            {
                return defaulVal;
            }
        }
        #endregion

        #region 转成Url传参格式 a=1&b=3&c=3
        /// <summary>
        /// 转成Url传参格式 a=1&b=3&c=3
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static string ToUrlParam(this Dictionary<string, string> dic)
        {
            var list = dic.Select(x => new { k = x.Key + "=" + x.Value }).Select(x => x.k).ToList();
            var str = string.Join("&", list);
            return str;
        }
        #endregion

        #region 数组拼接成字符串
        /// <summary>
        /// 数组拼接成字符串
        /// </summary>
        /// <param name="ienumerable"></param>
        /// <param name="splitStr">分割字符串:默认为逗号</param>
        /// <returns></returns>
        public static string ToJoinStr(this IEnumerable<object> ienumerable, string splitStr = ",")
        {
            return string.Join(splitStr, ienumerable);
        }
        #endregion


        #region 模拟Get请求方法
        /// <summary>
        /// Get获取请求结果
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string SendGet(this string url)
        {
            var msg = "";
            try
            {
                HttpWebRequest request;
                HttpWebResponse response;
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                string str = "";
                using (WebResponse wr = request.GetResponse())
                {
                    response = (HttpWebResponse)request.GetResponse();
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    str = reader.ReadToEnd();
                }
                msg = str;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToStr();
            }

            return msg;
        }
        #endregion

        #region 模拟Post请求方法
        /// <summary>
        /// Post获取请求结果
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string SendPost(this string strURL, string param)
        {
            var msg = "";
            try
            {
                HttpWebRequest request = WebRequest.Create(strURL) as HttpWebRequest;
                byte[] data = System.Text.Encoding.UTF8.GetBytes(param);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                using (Stream stream = request.GetRequestStream()) { stream.Write(data, 0, data.Length); }
                var sm = (request.GetResponse() as HttpWebResponse).GetResponseStream();
                StreamReader sr = new StreamReader(sm, Encoding.UTF8);
                msg = sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToStr();
            }
            return msg;
        }
        #endregion


        /// <summary>
        /// null转空
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string CheckNullRstring(object o)
        {
            if (o == null)
                return "";
            return o.ToString();
        }

        /// <summary>
        /// 时间 是否同一天比较
        /// </summary>
        /// <param name="dt1"></param>
        /// <returns></returns>
        public static bool CompareDatetime(DateTime? dt1, DateTime dt2)
        {
            if (dt1 == null) return false;
            var newdt = dt1.ToDateTime();
            return newdt.Year == dt2.Year && newdt.Month == dt2.Month && newdt.Day == dt2.Day;
        }

        /// <summary>    
        /// 转化一个DataTable    
        /// </summary>    
        /// <typeparam name="T"></typeparam>    
        /// <param name="list"></param>    
        /// <returns></returns>    
        public static DataTable ToDataTable<T>(this IEnumerable<T> list)
        {

            //创建属性的集合    
            List<PropertyInfo> pList = new List<PropertyInfo>();
            //获得反射的入口    

            Type type = typeof(T);
            DataTable dt = new DataTable();
            //把所有的public属性加入到集合 并添加DataTable的列      // p.PropertyType
            Array.ForEach<PropertyInfo>(type.GetProperties(), p => { pList.Add(p); dt.Columns.Add(p.Name, typeof(string)); });
            foreach (var item in list)
            {
                //创建一个DataRow实例    
                DataRow row = dt.NewRow();
                //给row 赋值    
                pList.ForEach(p => row[p.Name] = p.GetValue(item, null));
                //加入到DataTable    
                dt.Rows.Add(row);
            }
            return dt;
        }


        /// <summary>    
        /// DataTable 转换为List 集合    
        /// </summary>    
        /// <typeparam name="TResult">类型</typeparam>    
        /// <param name="dt">DataTable</param>    
        /// <returns></returns>    
        public static List<T> ToList<T>(this DataTable dt) where T : class, new()
        {
            //创建一个属性的列表    
            List<PropertyInfo> prlist = new List<PropertyInfo>();
            //获取TResult的类型实例  反射的入口    

            Type t = typeof(T);

            //获得TResult 的所有的Public 属性 并找出TResult属性和DataTable的列名称相同的属性(PropertyInfo) 并加入到属性列表     
            Array.ForEach<PropertyInfo>(t.GetProperties(), p => { if (dt.Columns.IndexOf(p.Name) != -1) prlist.Add(p); });

            //创建返回的集合    

            List<T> oblist = new List<T>();

            foreach (DataRow row in dt.Rows)
            {
                //创建TResult的实例    
                T ob = new T();
                //找到对应的数据  并赋值    
                prlist.ForEach(p => { if (row[p.Name] != DBNull.Value) p.SetValue(ob, row[p.Name], null); });
                //放入到返回的集合中.    
                oblist.Add(ob);
            }
            return oblist;
        }




        /// <summary>    
        /// 将集合类转换成DataTable    
        /// </summary>    
        /// <param name="list">集合</param>    
        /// <returns></returns>    
        public static DataTable ToDataTableTow(IList list)
        {
            DataTable result = new DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();

                foreach (PropertyInfo pi in propertys)
                {
                    result.Columns.Add(pi.Name, pi.PropertyType);
                }
                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        object obj = pi.GetValue(list[i], null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }

        /// <summary>
        /// 数字转中文
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string NumToChinese(object num, int type = 1)
        {
            try
            {
                string NumberChars = "";
                if (type == 1) NumberChars = "零一二三四五六七八九 ";
                if (type == 2) NumberChars = "日一二三四五六";
                int n = (int)num;
                return " " + NumberChars[n];
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 获取Config文件配置
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetConfig(string key)
        {
            try
            {
                return ConfigurationManager.AppSettings[key].ToString().ToStrTrim();
            }
            catch (Exception ex)
            {
                return "";
            }

        }

        /// <summary>
        /// 去除中文转义符
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RemoveSymbol(string value)
        {
            value = value.Replace("\n", "");
            value = value.Replace("（", "(");
            value = value.Replace("）", ")");
            value = value.Trim();
            return value;
        }

        /// <summary>
        /// 根据枚举名称获取枚举值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        public static int GetEumnValuesByName<T>(string values)
        {
            try
            {
                values = values.Trim();
                if (values == "" || values == "-") return 0;
                var ss = (int)Enum.Parse(typeof(T), values);
                return ss;
            }
            catch (Exception e)
            {
                throw new Exception("不存在【" + values + "】这种类型");
            }
        }
        /// <summary>
        /// 根据值获取枚举文本描述
        /// </summary>
        /// <param name="v"></param>
        /// <param name="enumname"></param>
        /// <returns></returns>
        public static string EnumToValue<T>(string v, int isspit = 0)
        {
            try
            {
                if (String.IsNullOrEmpty(v)) return "";
                if (isspit == 0)
                {
                    int value = v == null ? -1 : Convert.ToInt32(v);
                    var val = Enum.GetName(typeof(T), value);
                    return val;
                }
                else
                {
                    var meg = "";
                    foreach (var item in v.Split(','))
                    {
                        if (!String.IsNullOrEmpty(item))
                        {
                            int value = Convert.ToInt32(item);
                            var val = Enum.GetName(typeof(T), value);
                            meg = meg + val + ",";
                        }
                    }
                    meg = meg.TrimEnd(',');
                    return meg;

                }
            }
            catch (Exception e)
            {
                return "";
            }
        }

        #region 枚举相关
        /// <summary>
        /// 根据枚举值获取文字
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="i"></param>
        /// <returns></returns>
        public static string EnumToName<T>(int i)
        {
            foreach (var e in Enum.GetValues(typeof(T)))
            {
                if (Convert.ToInt32(e) == i)
                    return e.ToString();
                else
                    continue;
            }
            return "";
        }

        /// <summary>
        /// 根据枚举值集合获取文字集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr">枚举值集合</param>
        /// <returns>若不存在，返回值的对应为：-1</returns>
        public static List<string> EnumToNameList<T>(int[] arr)
        {
            var list = new List<string>();
            for (int i = 0; i < arr.Length; i++)
                list.Add(EnumToName<T>(arr[i]));
            return list;
        }

        /// <summary>
        /// 根据枚举的值获取文字描述
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int EnumToVal<T>(string name)
        {
            foreach (var e in Enum.GetValues(typeof(T)))
            {
                if (e.ToString() == name)
                    return Convert.ToInt32(e);
                else
                    continue;
            }
            return -1;
        }

        /// <summary>
        /// 根据枚举文字集合获取值集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr">枚举文字集合</param>
        /// <returns>若不存在，返回值的对应为空字符串</returns>
        public static List<int> EnumToValList<T>(string[] arr)
        {
            var list = new List<int>();
            for (int i = 0; i < arr.Length; i++)
                list.Add(EnumToVal<T>(arr[i]));
            return list;
        }
        #endregion


        public static string ToGrade(double value)
        {
            double i = Math.Round(value % 8, 1);
            int j = (int)(value / 8);
            string mes = "";
            if (j != 0) mes += j;
            if (i != 0) mes += " " + i + "/8";
            if (mes == "") return "0";
            return mes;
        }

        /// <summary>  
        /// 获取指定月份指定周数的开始日期  
        /// </summary>  
        /// <param name="year">年份</param>  
        /// <param name="month">月份</param>  
        /// <param name="index">周数</param>  
        /// <returns></returns>  
        public static void GetStartDayOfWeeks(int year, int month, string[] index, out DateTime startDayOfWeeks, out DateTime endDayOfWeeks)
        {
            DateTime startMonth = new DateTime(year, month, 1);  //该月第一天  
            int dayOfWeek = 7;
            if (Convert.ToInt32(startMonth.DayOfWeek.ToString("d")) > 0)
            {
                dayOfWeek = Convert.ToInt32(startMonth.DayOfWeek.ToString("d"));  //该月第一天为星期几  

            }
            var startWeek = new DateTime();
            if (dayOfWeek <= 3)
            {     //该月第一周开始日期  
                startWeek = startMonth.AddDays(1 - dayOfWeek);
            }
            else
            {
                startWeek = startMonth.AddDays(8 - dayOfWeek);
            }

            //DateTime startDayOfWeeks = startWeek.AddDays((index - 1) * 7);  //index周的起始日期  
            startDayOfWeeks = startWeek.AddDays((Convert.ToInt32(index[0]) - 1) * 7);  //index周的起始日期  
            endDayOfWeeks = startWeek.AddDays(Convert.ToInt32(index[index.Length - 1]) * 7).AddDays(-1);

            if (endDayOfWeeks.Month == (month + 1) && endDayOfWeeks.Day > 4) //已超过当月最大周日期
            {
                endDayOfWeeks = endDayOfWeeks.AddDays(-7);
            }
            if (startDayOfWeeks.Month == (month + 1))
            {
                startDayOfWeeks = startDayOfWeeks.AddDays(-7);
            }
        }

        public static int GetMonthWeek(DateTime date)
        {
            int result = 0;
            var sdate = DateTime.Parse(date.Year + "-" + date.Month + "-01");
            var edate = sdate.AddMonths(1).AddMinutes(-1);


            var quitweek = int.Parse(sdate.DayOfWeek.ToString("d"));
            sdate = sdate.AddDays(7 - quitweek);
            result++;
            for (int i = 0; i < 5; i++)
            {
                sdate = sdate.AddDays(7);
                if (sdate > edate)
                {
                    break;
                }
                result++;
            }
            return result;
        }


        /// <summary>
        /// 实体转 Dic
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj1"></param>
        /// <returns></returns>
        public static List<FiledChange> ToDic<T>(T obj1)
        {
            // var dic = new Dictionary<string, object>();
            var list = new List<FiledChange>();
            var a = obj1.GetType();
            var sinfo = a.GetProperties();
            for (int i = 0; i < sinfo.Length; i++)
            {
                var name = sinfo[i].Name;
                if (name.ToUpper() != "ID" && name.ToUpper() != "CREATDATE" && name.ToUpper() != "STATUS")
                {
                    if (sinfo[i].PropertyType.IsValueType || sinfo[i].PropertyType.Name.StartsWith("String"))
                    {
                        var f = new FiledChange();
                        var Remark = "";
                        var fiarr1 = sinfo[i].GetValue(obj1, null);
                        var descriptarr = (DescriptionAttribute[])sinfo[i].GetCustomAttributes(typeof(DescriptionAttribute), false);
                        if (descriptarr != null && descriptarr.Length >= 1)
                        {
                            var des = descriptarr[0].Description;
                            name = descriptarr[0].Description;
                            if (des.Split('|').Length == 2)
                            {
                                name = des.Split('|')[0];
                                Remark = des.Split('|')[1];
                            }
                        }
                        f.FileName = name;
                        f.Remark = Remark;
                        f.FileVal = fiarr1;
                        //  f.Description = name;
                        list.Add(f);
                    }
                }
            }
            return list;
        }

        public static string ValdesToKey(string des, string val)
        {
            //des  1=男,2=女
            try
            {
                var dic = new Dictionary<string, string>();
                if (des.Length != 0)
                {
                    if (des.Contains(','))
                    {
                        var keyarr = des.Split(',');
                        foreach (var key in keyarr)
                        {
                            dic.Add(key.Split('=')[0], key.Split('=')[1]);
                        }
                    }
                    else
                    {
                        dic.Add(des.Split('=')[0], des.Split('=')[1]);
                    }
                }
                var result = dic[val];
                return result;
            }
            catch (Exception e)
            {
                return val;
            }
        }

        public static string RandomNum()
        {

            var v = rd.Next(1000, 9999);
            return v.ToString();

        }




        #region 时间-辅助方法
        /// <summary>
        /// 获取今日，昨日，本周 时间，返回string
        /// </summary>
        /// <param name="type">1开始   2结束</param>
        /// <param name="t">1:今日  2：昨日  3：本周</param>
        /// <returns></returns>
        public static string GetTimeStr(int type, int t)
        {
            return GetTime(type, t).ToString("yyyy/MM/dd HH:mm:ss");
        }

        /// <summary>
        /// 获取今日，昨日，本周，返回DateTime
        /// </summary>
        /// <param name="type">1开始   2结束</param>
        /// <param name="t">1:今日  2：昨日  3：本周  4：不限制时间  5：本月</param>
        /// <returns></returns>
        public static DateTime GetTime(int type, int? t = 1)
        {
            //t:默认==1  
            DateTime dt = new DateTime();
            switch (t)
            {
                case 1: dt = GetTimeForL(DateTime.Now, type); break;//今日
                case 2: dt = GetTimeForL(DateTime.Now.AddDays(-1), type); break;//昨日
                case 3: dt = GetTimeForL(type == 1 ? GetWeekFirstDay() : GetWeekLastDay(), type); break;//本周
                case 5: dt = GetTimeForL(type == 1 ? GetMonthFistDay() : GetMonthLastDay(), type); break;//本月
                default: break;
            }
            return dt;
        }

        /// <summary>
        /// 当月第一天
        /// </summary>
        /// <returns></returns>
        public static DateTime GetMonthFistDay()
        {
            return Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-01"));//实例化为1号的日期
        }
        /// <summary>
        /// 当月最后一天
        /// </summary>
        /// <returns></returns>
        public static DateTime GetMonthLastDay()
        {
            return GetMonthFistDay().AddMonths(1).AddDays(-1);// 得到1号然后 加一个月 ，再减去一天
        }

        /// <summary>
        /// 得到本周第一天
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static DateTime GetWeekFirstDay()
        {
            DateTime datetime = DateTime.Now;
            //星期一为第一天  
            int weeknow = Convert.ToInt32(datetime.DayOfWeek);
            //因为是以星期一为第一天，所以要判断weeknow等于0时，要向前推6天。  
            weeknow = (weeknow == 0 ? (7 - 1) : (weeknow - 1));
            int daydiff = (-1) * weeknow;
            //本周第一天  
            string FirstDay = datetime.AddDays(daydiff).ToString("yyyy/MM/dd");
            return Convert.ToDateTime(FirstDay);
        }
        /// <summary>  
        /// 得到本周最后一天
        /// </summary>  
        /// <param name="datetime"></param>  
        /// <returns></returns>  
        public static DateTime GetWeekLastDay()
        {
            DateTime datetime = DateTime.Now;
            //星期天为最后一天  
            int weeknow = Convert.ToInt32(datetime.DayOfWeek);
            weeknow = (weeknow == 0 ? 7 : weeknow);
            int daydiff = (7 - weeknow);
            //本周最后一天  
            string LastDay = datetime.AddDays(daydiff).ToString("yyyy/MM/dd");
            return Convert.ToDateTime(LastDay);
        }
        /// <summary>
        /// 给定时间，返回一天的最早和最晚时间
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="type">1:最早时间  2:最晚时间</param>
        /// <returns></returns>
        public static DateTime GetTimeForL(DateTime dt, int type)
        {
            return Convert.ToDateTime(type == 1 ? dt.ToString("yyyy/MM/dd 00:00:00") : dt.ToString("yyyy/MM/dd 23:59:59"));
        }
        #endregion 时间辅助方法

        //#region 加密方法
        ///// <summary>
        ///// 加密方法
        ///// </summary>
        ///// <param name="name">需要加密字符串</param>
        ///// <returns>加密后的字符串</returns>
        //public static string Encrypt(this string name)
        //{
        //    if (name != "")
        //    {
        //        DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
        //        MemoryStream ms = new MemoryStream();
        //        CryptoStream cs = new
        //            CryptoStream(ms, cryptoProvider.CreateEncryptor(KEY_64, IV_64), CryptoStreamMode.Write);
        //        StreamWriter sw = new StreamWriter(cs);
        //        sw.Write(name);
        //        sw.Flush();
        //        cs.FlushFinalBlock();
        //        ms.Flush();

        //        //再转换为一个字符串
        //        return Convert.ToBase64String(ms.GetBuffer(), 0, Int32.Parse(ms.Length.ToString()));
        //    }
        //    else
        //    {
        //        return "";
        //    }
        //}
        //#endregion

        //#region 解密方法
        ///// <summary>
        ///// 解密方法
        ///// </summary>
        ///// <param name="name">需要解密的字符串</param>
        ///// <returns>解密后的字符串</returns>
        //public static string Decrypt(this string name)//标准的DES解密
        //{
        //    //#region DES 解密算法
        //    if (name != "")
        //    {
        //        DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
        //        //从字符串转换为字节组
        //        Byte[] buffer = Convert.FromBase64String(name);
        //        MemoryStream ms = new MemoryStream(buffer);
        //        CryptoStream cs = new
        //            CryptoStream(ms, cryptoProvider.CreateDecryptor(KEY_64, IV_64), CryptoStreamMode.Read);
        //        StreamReader sr = new StreamReader(cs);
        //        return sr.ReadToEnd();
        //    }
        //    else
        //    {
        //        return "";
        //    }
        //    //#endregion
        //}
        //#endregion

        //private static Byte[] KEY_64
        //{
        //    get
        //    {
        //        return new byte[] { 42, 16, 93, 156, 78, 4, 218, 32 };
        //    }
        //}
        //private static Byte[] IV_64
        //{
        //    get
        //    {
        //        return new byte[] { 55, 103, 246, 79, 36, 99, 167, 3 };
        //    }
        //}

        /// <summary>
        /// 根据keyvalue 集合获取下拉框的option选项html文本
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string GetHtmlOnOption(List<KeyValue> list)
        {
            return string.Join("", list.Select(x => new { html = "<option value='" + x.Value + "'>" + x.Name + "</option>" }).Select(x => x.html));
        }

        public static string GetHtmlOnOptionStr(List<KeyValue> list)
        {
            return string.Join("", list.Select(x => new { html = "<option value='" + x.UDID + "'>" + x.Name + "</option>" }).Select(x => x.html));
        }


        /// <summary>
        /// 枚举转集合（KeyValue对象,存放键值对）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static List<KeyValue> EnumToList<T>()
        {
            List<KeyValue> list = new List<KeyValue>();
            var i = 0;
            foreach (var e in Enum.GetValues(typeof(T)))
            {
                i++;
                var m = new KeyValue();
                m.Name = e.ToString();
                m.Value = Convert.ToInt32(e);
                m.No = i;
                list.Add(m);
            }
            return list;
        }

        /// <summary>
        /// 新起线程执行
        /// </summary>
        /// <param name="m"></param>
        public static void ThreadAction(Action m)
        {
            try
            {
                var t = new Thread(() =>
                {
                    m();
                });
                t.Start();

            }
            catch (Exception ex) {
                //Logger.Default.Error(ex.Message);
            }
        }

        /// <summary>
        /// 新起线程执行
        /// </summary>
        /// <param name="m"></param>
        public static async Task<int> TaskAction(Action m)
        {
            try
            {
                await Task.Factory.StartNew(() =>
                {
                    m();
                });
                return 1;
            }
            catch (Exception ex)
            {
                //Logger.Default.Error(ex.Message);
                return 0;
            }
        }



        #region 转成Dictionary<string,string>原字符 a=1&b=3&c=3
        /// <summary>
        ///  转成Dictionary<string,string>字符 a=1&b=3&c=3
        /// </summary>
        /// <param name="paramStr"></param>
        /// <returns></returns>
        public static Dictionary<string, string> UrlParamToDic(string paramStr)
        {
            var dic = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(paramStr))
                return dic;
            var paramArr = paramStr.Split('&');
            for (int i = 0; i < paramArr.Length; i++)
                dic.Add(paramArr[i].Split('=')[0], paramArr[i].Split('=')[1]);
            return dic;
        }
        #endregion

        #region 将后面字符串追加到当前字符后面
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="newStr"></param>
        /// <returns></returns>
        public static string ToAppend(this string str, string newStr)
        {
            var sb = new StringBuilder();
            sb.Append(str);
            sb.Append(newStr);
            return sb.ToStr();
        }
        #endregion

        #region 日志记录
        /// <summary>
        /// 日志记录
        /// </summary>
        /// <param name="action">错误名称（文件名-不支持特殊符号）</param>
        /// <param name="strMessage">错误内容</param>
        public static void WriteTextLog(string action, string msg)
        {
            string path = @"D:\Test_Log\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string fileFullPath = path + action + DateTime.Now.ToString("_yyyyMMddhhmmss") + ".txt";
            StringBuilder str = new StringBuilder();
            str.Append("Time:    " + DateTime.Now.ToString() + "\r\n");
            str.Append("Action:  " + action + "\r\n");
            str.Append("Message: " + msg + "\r\n");
            str.Append("-----------------------------------------------------------\r\n\r\n");
            StreamWriter sw;
            if (!File.Exists(fileFullPath))
            {
                sw = File.CreateText(fileFullPath);
            }
            else
            {
                sw = File.AppendText(fileFullPath);
            }
            sw.WriteLine(str.ToString());
            sw.Close();
        }
        #endregion


        #region 枚举
        #region 获取枚举的 Text(文字内容)
        /// <summary>
        /// 获取枚举的 Text(文字内容)
        /// </summary>
        /// <param name="t">Type</param>
        /// <param name="value">Text</param>
        /// <returns></returns>
        public static string GetEnumText(Type type, int value, string defaultval = "")
        {
            try
            {
                return Enum.Parse(type, value.ToString()).ToString();
            }
            catch (Exception)
            {
                return defaultval;
            }
        }
        #endregion

        #region 获取枚举的 Value(数字内容)
        /// <summary>
        /// 获取枚举的 Value(数字内容)
        /// </summary>
        /// <param name="t">Type</param>
        /// <param name="text">Text</param>
        /// <returns></returns>
        public static int GetEnumValue(Type type, string text, int defaultval = -1)
        {
            try
            {
                return Convert.ToInt32(Enum.Parse(type, text.Trim()));
            }
            catch (Exception)
            {
                return defaultval;
            }
        }
        #endregion
        #endregion


        /// <summary>
        /// 分页参数转换
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        public static void ConvertPage(ref int? offset, ref int? limit)
        {
            if (offset != 0)
            { offset = offset / limit; }
            offset += 1;
        }

        /// <summary>
        /// 分页参数转换
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        public static void ConvertPage(this BaseQuery query)
        {
            query.offset = query.offset ?? 1;
            query.limit = query.limit ?? 10;
            if (query.offset != 0)
            { query.offset = query.offset / query.limit; }
            query.offset += 1;
            query.TakeNum = query.limit.Value;
            query.SkipNum = (query.offset.Value - 1) * query.TakeNum;
        }
    }

    public class KeyValue
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int No { get; set; }
        /// <summary>  
        /// 枚举的描述  
        /// </summary>  
        public string Name { set; get; }

        /// <summary>  
        /// String类型的Id
        /// </summary>  
        public string UDID { set; get; }
        /// <summary>  
        /// 枚举对象的值  
        /// </summary>  
        public int Value { set; get; }
    }

    public class FiledChange
    {
        public string FileName { get; set; }
        public object FileVal { get; set; }
        public string Description { get; set; }
        public string Remark { get; set; }
    }
}
