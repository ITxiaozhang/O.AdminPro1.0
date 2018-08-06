using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.SysEnum
{
    /// <summary>
    /// 系统可操作类型
    /// </summary>
    public enum SysPower_enum
    {
        版本日志 = -1,
        登录 = -2,
        退出 = -3,
        视图 = 0,//默认为视图配置
        添加 = 1,
        编辑 = 2,
        删除 = 3,
        导入 = 4,
        导出 = 5,
        查询 = 6,
    }
    /// <summary>
    /// 
    /// </summary>
    public enum SysResponseCode
    {
        登录过期 = -100,
        成功 = 200,
        无访问权限 = 403,
        服务端错误 = 500,
        服务器请求超时 = 503,
    }
}
