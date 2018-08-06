using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Configuration;
using System.Linq.Expressions;

namespace Common
{
    public static class ConvertHelper
    {
        public static string ToStr(this object value, string defaultValue = "")
        {
            if (value == null) return defaultValue;
            else
            {
                return value.ToString();
            }
        }
        public static string ToStrTrim(this object value, string defaultValue = "")
        {
            if (value == null) return defaultValue;
            else
            {
                return value.ToString().Trim();
            }
        }
        /// <summary>
        ///去除所有的空格
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue">为空时的值，默认为""</param>
        /// <returns></returns>
        public static string ToAllTrim(this object value, string defaultValue = "")
        {
            if (value == null) return "";
            else
            {
                return value.ToString().Replace(" ", "");
            }
        }
        public static int ToInt(this object value, int defaultValue = 0)
        {
            if (value == null) return defaultValue;
            try
            {
                return Convert.ToInt32(value);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 去尾法-取小数几位
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static decimal ToFloor(this decimal value, int n = 1)
        {
            var i = value.ToString().IndexOf(".");
            if (i < 0)
                return value;
            else
                return value.ToString().Substring(0, i + 2).ToDecimal();
        }
        /// <summary>
        /// 去尾法-取整
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static decimal Floor(this decimal value)
        {
            return Decimal.Floor(value).ToInt();
        }

        /// <summary>
        /// 将Decimal 保留n位小数,n默认为1
        /// </summary>
        /// <param name="value"></param>
        /// <param name="n">几位小数，默认为1</param>
        /// <returns></returns>
        public static decimal DecimalRound(this decimal value, int n = 1)
        {
            return Decimal.Round(value, n);
        }

        public static decimal ToDecimal(this object value, decimal defaultValue = 0)
        {
            decimal rst;
            if (value == null) return defaultValue;
            if (decimal.TryParse(value.ToString(), out rst))
            {
                return rst;
            }
            else
            {
                return defaultValue;
            }
        }

        public static double ToDouble(this object value, double defaultValue = 0)
        {
            double rst;
            if (value == null) return defaultValue;
            if (double.TryParse(value.ToString(), out rst))
            {
                return rst;
            }
            else
            {
                return defaultValue;
            }
        }
        public static float ToFloat(this object value, float defaultValue = 0)
        {
            float rst;
            if (value == null) return defaultValue;
            if (float.TryParse(value.ToString(), out rst))
            {
                return rst;
            }
            else
            {
                return defaultValue;
            }
        }


        public static bool ToBool(this object value, bool defaultValue = false)
        {
            bool rst;
            if (value == null) return defaultValue;
            if (bool.TryParse(value.ToString(), out rst))
            {
                return rst;
            }
            else
            {
                return defaultValue;
            }
        }

        public static DateTime ToDateTime(this object value, DateTime defaultValue)
        {
            DateTime rst;
            if (value == null) return defaultValue;
            if (DateTime.TryParse(value.ToString(), out rst))
            {
                return rst;
            }
            else
            {
                return defaultValue;
            }
        }

        public static DateTime? ToDateTime(this object value, DateTime? defaultValue)
        {
            DateTime rst;
            if (value == null) return defaultValue;
            if (DateTime.TryParse(value.ToString(), out rst))
            {
                return rst;
            }
            else
            {
                return defaultValue;
            }
        }

        public static DateTime ToDateTime(this object value)
        {
            var defaultValue = DateTime.Now;
            DateTime rst;
            if (value == null) return defaultValue;
            if (DateTime.TryParse(value.ToString(), out rst))
            {
                return rst;
            }
            else
            {
                return defaultValue;
            }
        }
        public static string DateToStr(this object value, string fomattr)
        {
            if (value == null)
                return "";
            else
                return Convert.ToDateTime(value).ToString(fomattr);
        }


        public static bool IsTime(this object value)
        {
            DateTime rst;
            if (value == null) return false;
            if (DateTime.TryParse(value.ToString(), out rst))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 是否为初始值
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static bool IsMinValue(this DateTime time)
        {
            return time == DateTime.MinValue;
        }
        /// <summary>
        /// 是否为null或初始值
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static bool IsNullOrMinValue(this DateTime? time)
        {
            return time == null || time.ToString().Trim() == "" || time == DateTime.MinValue;
        }

        /// <summary>
        /// 转换为统一时间格式yyyy-MM-dd HH:mm
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string ToDateTimeFormatString(this DateTime time)
        {
            return time.ToString("yyyy-MM-dd HH:mm");
        }


        /// <summary>
        /// 转换为统一时间格式yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string ToDateTimeFormatString(this string time)
        {
            DateTime rst;
            if (DateTime.TryParse(time, out rst))
                return rst.ToString("yyyy-MM-dd HH:mm");
            else
            {
                return DateTime.Now.ToDateTimeFormatString();
            }
        }

        /// <summary>
        /// 固定字符替换指定部分的值
        /// </summary>
        /// <param name="str"></param>
        /// <param name="starNum">保留前几位正常显示</param>
        /// <param name="endNum">保留后几位正常显示</param>
        /// <param name="chr">加密符号</param>
        /// <returns></returns>
        public static string ReplaceChar(this string str, int starNum, int endNum, char chr = '*')
        {
            string rst = string.Empty;
            if (str == null) return "";
            int chrNum = str.Length - starNum - endNum > 0 ? str.Length - starNum - endNum : 0;
            if (str.Length > starNum + endNum)
                rst = str.Substring(0, starNum)
                            + "".PadLeft(chrNum, chr)
                            + str.Substring(str.Length - endNum);
            else
            {
                rst = str;
            }
            return rst;
        }

        public static Random Rdom = new Random();
        /// <summary>
        /// 对list集合进行随机排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ls"></param>
        /// <returns></returns>
        public static List<T> RandomSort<T>(this List<T> ls)
        {
            ls = ls.Select(a => new { a, newID = Rdom.Next(-1, ls.Count) }).OrderBy(b => b.newID).Select(c => c.a).ToList();
            return ls;
        }




        /// <summary>
        ///     DataSetToList
        /// </summary>
        /// <typeparam name="T">转换类型</typeparam>
        /// <param name="dataSet">数据源</param>
        /// <returns></returns>
        public static List<T> DataSetToList<T>(DataSet dataSet)
        {
            //确认参数有效
            if (dataSet == null || dataSet.Tables.Count <= 0)
                return null;

            DataTable dt = dataSet.Tables[0];

            var list = new List<T>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //创建泛型对象
                var _t = Activator.CreateInstance<T>();
                //获取对象所有属性
                PropertyInfo[] propertyInfo = _t.GetType().GetProperties();
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    foreach (PropertyInfo info in propertyInfo)
                    {
                        //属性名称和列名相同时赋值
                        if (dt.Columns[j].ColumnName.ToUpper().Equals(info.Name.ToUpper()))
                        {
                            if (dt.Rows[i][j] != DBNull.Value)
                            {
                                info.SetValue(_t,
                                    Convert.ChangeType(dt.Rows[i][j],
                                        Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType), null);
                            }
                            else
                            {
                                info.SetValue(_t, null, null);
                            }
                            break;
                        }
                    }
                }
                list.Add(_t);
            }
            return list;
        }

        #region 实体复制-新实体
        /// <summary>
        /// 实体重新赋值--避免使用同一内存地址的问题
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static T EntityToEntity<T>(this T self, T result)
        {
            if (self == null)
                return result;
            try
            {
                foreach (var mItem in typeof(T).GetProperties())
                {
                    var obj = mItem.GetType().IsValueType ? Activator.CreateInstance(mItem.GetType()) : null;
                    if (obj != mItem.GetValue(self, null))
                        mItem.SetValue(result, mItem.GetValue(self, null), null);
                }
            }
            catch (NullReferenceException NullEx)
            {
                throw NullEx;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return result;
        }

        public static T EntityToEntity_v2<T>(this T self)
        {
            var result = (T)Activator.CreateInstance(typeof(T));
            if (self == null)
                return result;
            try
            {
                foreach (var mItem in typeof(T).GetProperties())
                {
                    var obj = mItem.GetType().IsValueType ? Activator.CreateInstance(mItem.GetType()) : null;
                    if (obj != mItem.GetValue(self, null))
                        mItem.SetValue(result, mItem.GetValue(self, null), null);
                }
            }
            catch (NullReferenceException NullEx)
            {
                throw NullEx;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return result;
        }

        /// <summary>
        /// 父类转成子类-将值付给子类
        /// </summary>
        /// <typeparam name="T">父类</typeparam>
        /// <typeparam name="T1">子类(需要得到的结果)</typeparam>
        /// <param name="parent">父类对象</param>
        /// <returns>返回赋值以后子类的结果</returns>
        public static T1 ParentToSonEnttiy<T, T1>(this T parent)
        {
            var result = (T1)Activator.CreateInstance(typeof(T1));
            if (parent == null)
                return result;
            try
            {
                foreach (var mItem in typeof(T).GetProperties())
                {
                    var obj = mItem.GetType().IsValueType ? Activator.CreateInstance(mItem.GetType()) : null;
                    if (obj != mItem.GetValue(parent, null))
                        mItem.SetValue(result, mItem.GetValue(parent, null), null);
                }
            }
            catch (NullReferenceException NullEx)
            {
                throw NullEx;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return result;
        }
        #endregion

        #region 实体--有Null的string的成员变量改为""
        /// <summary>
        /// 实体--有Null的string的成员变量改为""
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static T EntityNullToStr<T>(this T self)
        {
            try
            {
                foreach (PropertyInfo mItem in typeof(T).GetProperties())
                {
                    var mItemVal = mItem.GetValue(self, new object[] { });
                    if (mItem.PropertyType == typeof(string))
                    {
                        mItem.SetValue(self, mItemVal.ToStr(), null);
                    }
                }
            }
            catch (NullReferenceException NullEx)
            {
                throw NullEx;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return self;
        }
        #endregion

        /// <summary>
        /// 查找指定字符出现过的次数
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int SubStrCount(this string a, string b)
        {
            if (a.Contains(b))
                return (a.Length - a.Replace(b, "").Length) / b.Length;
            return 0;
        }

        #region 获取配置的内容
        /// <summary>
        /// 获取配置中的内容-为空择返回""
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetAppSettingsStr(this string str)
        {
            return ConfigurationManager.AppSettings[str].ToStr();
        }
        #endregion


        /// <summary>
        ///  从指定IQueryable[T]集合 中查询指定分页条件的子数据集
        /// </summary>
        /// <typeparam name="TEntity">动态实体类型</typeparam>
        /// <typeparam name="TKey">实体主键类型</typeparam>
        /// <param name="source">要查询的数据集</param>
        /// <param name="predicate">查询条件谓语表达式</param>
        /// <param name="pageIndex">分页索引</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="total">输出符合条件的总记录数</param>
        /// <returns></returns>
        public static IQueryable<TEntity> ToPage<TEntity>(this IQueryable<TEntity> source, List<Expression<Func<TEntity, bool>>> predicate, int pageIndex, int pageSize,
            out int total) where TEntity : class
        {
            if (predicate != null)
            {
                foreach (var item in predicate)
                {
                    source = source.Where(item);
                }
            }
            total = source.Count();
            // source = source.OrderBy(m => m.CreateDate);
            return source != null ? source.Skip((pageIndex - 1) * pageSize).Take(pageSize) : Enumerable.Empty<TEntity>().AsQueryable();
        }
    }

}
