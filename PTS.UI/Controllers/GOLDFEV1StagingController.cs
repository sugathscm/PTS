using PTS.BAL;
using PTS.DAL;
using PTS.UI;
using PTS.UI.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace WFM.UI.Controllers
{
    //[Authorize]
    public class GOLDFEV1StagingController : Controller
    {
        private ApplicationUserManager _userManager;
        private readonly GenericService genericService = new GenericService();

        public GOLDFEV1StagingController()
        {
        }

        public GOLDFEV1StagingController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Diastolics
        public ActionResult Index(int? id)
        {
            GOLDFEV1Staging goldFEV1Staging = new GOLDFEV1Staging();
            if (id != null)
            {
                goldFEV1Staging = genericService.GetList<GOLDFEV1Staging>().Where(o => o.Id == id).FirstOrDefault();
            }
            return View(goldFEV1Staging);
        }

        public ActionResult GetList()
        {
            List<GOLDFEV1Staging> list = genericService.GetList<GOLDFEV1Staging>().ToList();

            List<GOLDFEV1StagingViewModel> modelList = new List<GOLDFEV1StagingViewModel>();

            foreach (var item in list)
            {
                modelList.Add(new GOLDFEV1StagingViewModel() { Id = item.Id, IsActive = item.IsActive, Stage = item.Stage, Text = item.Text });
            }

            return Json(new { data = modelList }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveOrUpdate(GOLDFEV1Staging model)
        {
            string newData = string.Empty, oldData = string.Empty;

            try
            {
                int id = model.Id;
                GOLDFEV1Staging goldFEV1Staging = null;
                GOLDFEV1Staging oldGOLDFEV1Staging = null;
                if (model.Id == 0)
                {
                    goldFEV1Staging = new GOLDFEV1Staging
                    {
                        Stage = model.Stage,
                        Text = model.Text,
                        IsActive = true
                    };

                    oldGOLDFEV1Staging = new GOLDFEV1Staging();
                    oldData = new JavaScriptSerializer().Serialize(oldGOLDFEV1Staging);
                    newData = new JavaScriptSerializer().Serialize(goldFEV1Staging);
                }
                else
                {
                    goldFEV1Staging = genericService.GetList<GOLDFEV1Staging>().Where(o => o.Id == model.Id).FirstOrDefault();
                    oldGOLDFEV1Staging = genericService.GetList<GOLDFEV1Staging>().Where(o => o.Id == model.Id).FirstOrDefault();

                    oldData = new JavaScriptSerializer().Serialize(new GOLDFEV1Staging()
                    {
                        Id = oldGOLDFEV1Staging.Id,
                        Stage = oldGOLDFEV1Staging.Stage,
                        Text = oldGOLDFEV1Staging.Text,
                        IsActive = oldGOLDFEV1Staging.IsActive
                    });

                    goldFEV1Staging.Stage = model.Stage;
                    goldFEV1Staging.Text = model.Text;
                    bool Example = Convert.ToBoolean(Request.Form["IsActive.Value"]);
                    goldFEV1Staging.IsActive = model.IsActive;

                    newData = new JavaScriptSerializer().Serialize(new GOLDFEV1Staging()
                    {
                        Id = goldFEV1Staging.Id,
                        Stage = goldFEV1Staging.Stage,
                        IsActive = goldFEV1Staging.IsActive
                    });
                }

                genericService.SaveOrUpdate<GOLDFEV1Staging>(goldFEV1Staging, goldFEV1Staging.Id);

                //CommonService.SaveDataAudit(new DataAudit()
                //{
                //    Entity = "Diastolics",
                //    NewData = newData,
                //    OldData = oldData,
                //    UpdatedOn = DateTime.Now,
                //    UserId = User.Identity.GetUserId()
                //});

                TempData["Message"] = ResourceData.SaveSuccessMessage;
            }
            catch (Exception ex)
            {
                TempData["Message"] = string.Format(ResourceData.SaveErrorMessage, ex.InnerException);
            }


            return RedirectToAction("Index", "GOLDFEV1Staging");
        }
    }
}