using O.AdminPro.Controllers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.SysEnum;

namespace O.AdminPro.Controllers
{
    [Base.Authorize]
    public class HomeController : BaseController
    {
        public JsonResult GetResult(string id)
        {
            return Json(Success(id), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}