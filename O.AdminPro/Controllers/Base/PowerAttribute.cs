using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common.SysEnum;

namespace O.AdminPro.Controllers.Base
{
    /// <summary>
    /// 权限特性-仅能用于方法之上
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class PowerAttribute : Attribute
    {
        public PowerAttribute(SysPower_enum code = SysPower_enum.视图)
        {
            this.PowerId = Convert.ToInt32(Enum.Parse(typeof(SysPower_enum), code.ToString()));
            this.SysPower = code;
        }
        public int PowerId { get; set; }
        public SysPower_enum SysPower { get; set; }
    }
}