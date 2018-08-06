using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelEx.Base
{
    /// <summary>
    /// 默认返回对象数据对象
    /// </summary>
    public class ResponseData
    {
        public ResponseData() { }
        public int code { get; set; }
        public string message { get; set; }
        public object data { get; set; }
    }
    public class ResponsePage {
        public ResponsePage(int _total, object _rows)
        {
            this.total = _total;
            this.rows = _rows;
        }
        public int total { get; set; }
        public object rows { get; set; }
    }
}
