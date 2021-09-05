using Newtonsoft.Json;
using PTS.BAL;
using PTS.DAL;
using PTS.UI.Models;
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
            return View();
        }

        public ActionResult GetList()
        {
            List<Patient> patientList = genericService.GetList<Patient>().ToList();

            List<BaseViewModel> modelList = new List<BaseViewModel>();

            foreach (var patient in patientList)
            {
                modelList.Add(new BaseViewModel() { Id = patient.Id, Name = patient.FirstName + " " + patient.LastName });
            }

            return Json(new { data = modelList }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Details(int? id)
        {
            Patient patient = new Patient();

            //Patient Details
            ViewBag.GenderList = new SelectList(genericService.GetList<Gender>(), "Id", "Name");
            ViewBag.TitleList = new SelectList(genericService.GetList<Title>(), "Id", "Name");
            ViewBag.MaritialStatuList = new SelectList(genericService.GetList<MaritialStatu>(), "Id", "Name");

            return View(patient);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveOrUpdate(PatientMedicalHistoryViewModel model)
        {
            string newData = string.Empty, oldData = string.Empty;
            try
            {

                TempData["Message"] = ResourceData.SaveSuccessMessage;
            }
            catch (Exception ex)
            {
                TempData["Message"] = string.Format(ResourceData.SaveErrorMessage, ex.InnerException);
            }

            return RedirectToAction("Index", "Report");
        }

        public class LogicalItem
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }
        }
    }
}