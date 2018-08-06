using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelEx.Base
{
    /// <summary>
    /// 分页类
    /// </summary>
    public class BaseQuery
    {
        /// <summary>
        /// 第几页
        /// </summary>
        public int? offset { get; set; }
        /// <summary>
        /// 每页条数
        /// </summary>
        public int? limit { get; set; }

        /// <summary>
        /// 跳过几页
        /// </summary>
        public int SkipNum { get; set; }
        /// <summary>
        /// 每页数量
        /// </summary>
        public int TakeNum { get; set; }
    }
}
