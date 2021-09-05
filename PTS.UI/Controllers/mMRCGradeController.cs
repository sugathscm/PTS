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
    public class mMRCGradeController : Controller
    {
        private ApplicationUserManager _userManager;
        private readonly GenericService genericService = new GenericService();

        public mMRCGradeController()
        {
        }

        public mMRCGradeController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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
            mMRCGrade mmRCGrade = new mMRCGrade();
            if (id != null)
            {
                mmRCGrade = genericService.GetList<mMRCGrade>().Where(o => o.Id == id).FirstOrDefault();
            }
            return View(mmRCGrade);
        }

        public ActionResult GetList()
        {
            List<mMRCGrade> list = genericService.GetList<mMRCGrade>().OrderBy(m => m.Id).ToList();

            List<mMRCGradeViewModel> modelList = new List<mMRCGradeViewModel>();

            foreach (var item in list)
            {
                modelList.Add(new mMRCGradeViewModel() { Id = item.Id, IsActive = item.IsActive, Name = item.Name, Weight = item.Weight });
            }

            return Json(new { data = modelList.OrderBy(m => m.Id).ToList() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveOrUpdate(mMRCGrade model)
        {
            string newData = string.Empty, oldData = string.Empty;

            try
            {
                int id = model.Id;
                mMRCGrade mmRCGrade = null;
                mMRCGrade oldDiastolic = null;
                if (model.Id == 0)
                {
                    mmRCGrade = new mMRCGrade
                    {
                        Name = model.Name,
                        Weight = model.Weight,
                        IsActive = true
                    };

                    oldDiastolic = new mMRCGrade();
                    oldData = new JavaScriptSerializer().Serialize(oldDiastolic);
                    newData = new JavaScriptSerializer().Serialize(mmRCGrade);
                }
                else
                {
                    mmRCGrade = genericService.GetList<mMRCGrade>().Where(o => o.Id == model.Id).FirstOrDefault();
                    oldDiastolic = genericService.GetList<mMRCGrade>().Where(o => o.Id == model.Id).FirstOrDefault();

                    oldData = new JavaScriptSerializer().Serialize(new mMRCGrade()
                    {
                        Id = oldDiastolic.Id,
                        Name = oldDiastolic.Name,
                        Weight = oldDiastolic.Weight,
                        IsActive = oldDiastolic.IsActive
                    });

                    mmRCGrade.Name = model.Name;
                    mmRCGrade.Weight = model.Weight;
                    bool Example = Convert.ToBoolean(Request.Form["IsActive.Value"]);
                    mmRCGrade.IsActive = model.IsActive;

                    newData = new JavaScriptSerializer().Serialize(new mMRCGrade()
                    {
                        Id = mmRCGrade.Id,
                        Name = mmRCGrade.Name,
                        IsActive = mmRCGrade.IsActive
                    });
                }

                genericService.SaveOrUpdate<mMRCGrade>(mmRCGrade, mmRCGrade.Id);

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


            return RedirectToAction("Index", "mMRCGrade");
        }
    }
}