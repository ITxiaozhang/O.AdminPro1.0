using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.SysEnum;
using ModelEx.Base;

namespace O.AdminPro.Controllers.Base
{
    public class AuthorizeAttribute:ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var attribute= filterContext.ActionDescriptor.GetCustomAttributes(true).Where(x => x.GetType() == typeof(PowerAttribute)).FirstOrDefault();
            //有权限配置-则验证（未配置则不验证）
            if (attribute != null)
            {
                var power = (PowerAttribute)attribute;
                //不需要验证的权限类型
                var NoList = (new SysPower_enum[] { SysPower_enum.版本日志, SysPower_enum.登录, SysPower_enum.退出 }).ToList();
                if (!NoList.Contains(power.SysPower))
                {
                    //filterContext.Result = new JsonResult
                    //{
                    //    Data = new ResponseData { code = (int)SysResponseCode.无访问权限, message = SysResponseCode.无访问权限.ToString() },
                    //    ContentEncoding = System.Text.Encoding.UTF8,
                    //    ContentType = "application/json",
                    //    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    //};
                    //return;
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}