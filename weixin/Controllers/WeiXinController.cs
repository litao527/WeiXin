using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace weixin.Controllers
{
    public class WeiXinController : Controller
    {
        // GET: WeiXin guPAaeyJM4yUGljNJgRfo8V4Uk0IUX3RCp477cBtPQY
        public ActionResult Index()
        {
            try
            {
                //测试静态页
                //int i = Common.CnblogsOperation.GenerationBlogToHtml(Common.CnblogsOperation.GetCnblogInfo());


                var datas = DAL.DataAcess.GetAllEntitys().ToList();
                ViewBag.Data = datas;
                return View();
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex.Message);
                throw;
            }
        }
        public JsonResult GetMsg()
        {

            try
            {
                var datas = DAL.DataAcess.GetAllEntitys().ToList();
                return Json(datas, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex.Message);
                throw;
            }
        }
    }
}