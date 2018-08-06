using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using ModelEx.Base;
using Common.SysEnum;

namespace O.AdminPro.Controllers.Base
{
    public class BaseController : Controller
    {


        #region 返回对象格式
        public ResponseData Success(string _msg)
        {
            return Success(_msg, null);
        }
        public ResponseData Success(string _msg, object _data)
        {
            return Success(_msg, (int)SysResponseCode.成功, _data);
        }
        public ResponseData Success(string _msg, int _code, object _data)
        {
            return new ResponseData() { code = _code, message = _msg, data = _data };
        }


        public ResponseData Error(string _msg)
        {
            return Error(_msg, null);
        }
        public ResponseData Error(string _msg, object _data)
        {
            return Error(_msg, (int)SysResponseCode.服务端错误, _data);
        }
        public ResponseData Error(string _msg, int _code, object _data)
        {
            return new ResponseData() { code = _code, message = _msg, data = _data };
        }
        #endregion
    }
}