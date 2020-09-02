using PTS.BAL;
using PTS.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PTS.UI.Controllers
{
    public class PatientController : Controller
    {
        readonly GenericService genericService = new GenericService();

        // GET: Patient
        public ActionResult Index()
        {
            ViewBag.GenderList = new SelectList(genericService.GetList<Gender>(), "Id", "Name");
            ViewBag.TitleList = new SelectList(genericService.GetList<Title>(), "Id", "Name");
            ViewBag.MaritialStatuList = new SelectList(genericService.GetList<MaritialStatu>(), "Id", "Name");

            return View();
        }
    }
}