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
    public class BODEIndexController : Controller
    {
        private ApplicationUserManager _userManager;
        private readonly GenericService genericService = new GenericService();

        public BODEIndexController()
        {
        }

        public BODEIndexController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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
            BODEIndex goldFEV1Staging = new BODEIndex();
            if (id != null)
            {
                goldFEV1Staging = genericService.GetList<BODEIndex>().Where(o => o.Id == id).FirstOrDefault();
            }
            return View(goldFEV1Staging);
        }

        public ActionResult GetList()
        {
            List<BODEIndex> list = genericService.GetList<BODEIndex>().ToList();

            List<BODEIndexViewModel> modelList = new List<BODEIndexViewModel>();

            foreach (var item in list)
            {
                modelList.Add(new BODEIndexViewModel() {
                    Id = item.Id,
                    IsActive = item.IsActive,
                    VAriable = item.VAriable,
                    Point1 = item.Point1,
                    Point2 = item.Point2,
                    Point3 = item.Point3,
                    Point4 = item.Point4
                });
            }

            return Json(new { data = modelList }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveOrUpdate(BODEIndex model)
        {
            string newData = string.Empty, oldData = string.Empty;

            try
            {
                int id = model.Id;
                BODEIndex goldFEV1Staging = null;
                BODEIndex oldBODEIndex = null;
                if (model.Id == 0)
                {
                    goldFEV1Staging = new BODEIndex
                    {
                        VAriable = model.VAriable,
                        Point1 = model.Point1,
                        Point2 = model.Point2,
                        Point3 = model.Point3,
                        Point4 = model.Point4,
                        IsActive = true
                    };

                    oldBODEIndex = new BODEIndex();
                    oldData = new JavaScriptSerializer().Serialize(oldBODEIndex);
                    newData = new JavaScriptSerializer().Serialize(goldFEV1Staging);
                }
                else
                {
                    goldFEV1Staging = genericService.GetList<BODEIndex>().Where(o => o.Id == model.Id).FirstOrDefault();
                    oldBODEIndex = genericService.GetList<BODEIndex>().Where(o => o.Id == model.Id).FirstOrDefault();

                    oldData = new JavaScriptSerializer().Serialize(new BODEIndex()
                    {
                        Id = oldBODEIndex.Id,
                        VAriable = oldBODEIndex.VAriable,
                        Point1 = oldBODEIndex.Point1,
                        Point2 = oldBODEIndex.Point2,
                        Point3 = oldBODEIndex.Point3,
                        Point4 = oldBODEIndex.Point4,
                        IsActive = oldBODEIndex.IsActive
                    });

                    goldFEV1Staging.VAriable = model.VAriable;
                    goldFEV1Staging.Point1 = model.Point1;
                    goldFEV1Staging.Point2 = model.Point2;
                    goldFEV1Staging.Point3 = model.Point3;
                    goldFEV1Staging.Point4 = model.Point4;
                    bool Example = Convert.ToBoolean(Request.Form["IsActive.Value"]);
                    goldFEV1Staging.IsActive = model.IsActive;

                    newData = new JavaScriptSerializer().Serialize(new BODEIndex()
                    {
                        Id = goldFEV1Staging.Id,
                        VAriable = oldBODEIndex.VAriable,
                        Point1 = oldBODEIndex.Point1,
                        Point2 = oldBODEIndex.Point2,
                        Point3 = oldBODEIndex.Point3,
                        Point4 = oldBODEIndex.Point4,
                        IsActive = goldFEV1Staging.IsActive
                    });
                }

                genericService.SaveOrUpdate<BODEIndex>(goldFEV1Staging, goldFEV1Staging.Id);

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


            return RedirectToAction("Index", "BODEIndex");
        }
    }
}