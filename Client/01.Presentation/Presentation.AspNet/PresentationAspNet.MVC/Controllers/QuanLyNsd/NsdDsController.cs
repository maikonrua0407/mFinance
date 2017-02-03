using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PresentationAspNet.MVC.Controllers.QuanLyNsd
{
    public class NsdDsController : Controller
    {
        //
        // GET: /NsdDs/

        public ActionResult Index()
        {
            //ViewBag.CurrentPage = bo.CurrentPage;
            //ViewBag.PageSize = bo.PageSize;
            //ViewBag.TotalRow = bo.TotalItem;
            return View();
        }

    }
}
