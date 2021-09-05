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
    public class FEV1StagingController : Controller
    {
        private ApplicationUserManager _userManager;
        private readonly GenericService genericService = new GenericService();

        public FEV1StagingController()
        {
        }

        public FEV1StagingController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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
            FEV1Staging catScore = new FEV1Staging();
            if (id != null)
            {
                catScore = genericService.GetList<FEV1Staging>().Where(o => o.Id == id).FirstOrDefault();
            }
            return View(catScore);
        }

        public ActionResult GetList()
        {
            List<FEV1Staging> list = genericService.GetList<FEV1Staging>().ToList();

            List<FEV1StagingViewModel> modelList = new List<FEV1StagingViewModel>();

            foreach (var item in list)
            {
                modelList.Add(new FEV1StagingViewModel() { Id = item.Id, IsActive = item.IsActive, Stage = item.Stage, Text = item.Text });
            }

            return Json(new { data = modelList }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveOrUpdate(FEV1Staging model)
        {
            string newData = string.Empty, oldData = string.Empty;

            try
            {
                int id = model.Id;
                FEV1Staging catScore = null;
                FEV1Staging oldDiastolic = null;
                if (model.Id == 0)
                {
                    catScore = new FEV1Staging
                    {
                        Stage = model.Stage,
                        Text = model.Text,
                        IsActive = true
                    };

                    oldDiastolic = new FEV1Staging();
                    oldData = new JavaScriptSerializer().Serialize(oldDiastolic);
                    newData = new JavaScriptSerializer().Serialize(catScore);
                }
                else
                {
                    catScore = genericService.GetList<FEV1Staging>().Where(o => o.Id == model.Id).FirstOrDefault();
                    oldDiastolic = genericService.GetList<FEV1Staging>().Where(o => o.Id == model.Id).FirstOrDefault();

                    oldData = new JavaScriptSerializer().Serialize(new FEV1Staging()
                    {
                        Id = oldDiastolic.Id,
                        Stage = oldDiastolic.Stage,
                        Text = oldDiastolic.Text,
                        IsActive = oldDiastolic.IsActive
                    });

                    catScore.Stage = model.Stage;
                    catScore.Text = model.Text;
                    bool Example = Convert.ToBoolean(Request.Form["IsActive.Value"]);
                    catScore.IsActive = model.IsActive;

                    newData = new JavaScriptSerializer().Serialize(new FEV1Staging()
                    {
                        Id = catScore.Id,
                        Stage = catScore.Stage,
                        IsActive = catScore.IsActive
                    });
                }

                genericService.SaveOrUpdate<FEV1Staging>(catScore, catScore.Id);

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


            return RedirectToAction("Index", "FEV1Staging");
        }
    }
}