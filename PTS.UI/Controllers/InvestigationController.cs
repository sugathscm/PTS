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
    public class InvestigationController : Controller
    {
        private ApplicationUserManager _userManager;
        private readonly GenericService genericService = new GenericService();

        public InvestigationController()
        {
        }

        public InvestigationController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        // GET: Investigations
        public ActionResult Index(int? id)
        {
            Investigation investigation = new Investigation();
            if (id != null)
            {
                investigation = genericService.GetList<Investigation>().Where(o => o.Id == id).FirstOrDefault();
            }

            ViewBag.InvestigationTypeList = new SelectList(genericService.GetList<InvestigationType>(), "Id", "Name");

            return View(investigation);
        }

        public ActionResult GetList()
        {
            List<Investigation> list = genericService.GetList<Investigation>();
            List<InvestigationType> investigationTypelist = genericService.GetList<InvestigationType>();

            List<InvestigationViewModel> modelList = new List<InvestigationViewModel>();

            foreach (var item in list)
            {
                string investigationTypeName = investigationTypelist.Where(it => it.Id == item.InvestigationTypeId).FirstOrDefault().Name;
                modelList.Add(new InvestigationViewModel() { Id = item.Id, IsActive = item.IsActive, Name = item.Name, InvestigationTypeName = investigationTypeName });
            }

            return Json(new { data = modelList }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveOrUpdate(Investigation model)
        {
            string newData = string.Empty, oldData = string.Empty;

            try
            {
                int id = model.Id;
                Investigation Investigation = null;
                Investigation oldInvestigation = null;
                if (model.Id == 0)
                {
                    Investigation = new Investigation
                    {
                        Name = model.Name,
                        IsActive = true,
                        InvestigationTypeId = model.InvestigationTypeId,
                    };

                    oldInvestigation = new Investigation();
                    oldData = new JavaScriptSerializer().Serialize(oldInvestigation);
                    newData = new JavaScriptSerializer().Serialize(Investigation);
                }
                else
                {
                    Investigation = genericService.GetList<Investigation>().Where(o => o.Id == model.Id).FirstOrDefault();
                    oldInvestigation = genericService.GetList<Investigation>().Where(o => o.Id == model.Id).FirstOrDefault();

                    oldData = new JavaScriptSerializer().Serialize(new Investigation()
                    {
                        Id = oldInvestigation.Id,
                        Name = oldInvestigation.Name,
                        IsActive = oldInvestigation.IsActive,
                        InvestigationTypeId = oldInvestigation.InvestigationTypeId,
                    });

                    Investigation.Name = model.Name;
                    Investigation.InvestigationTypeId = model.InvestigationTypeId;
                    bool Example = Convert.ToBoolean(Request.Form["IsActive.Value"]);
                    Investigation.IsActive = model.IsActive;

                    newData = new JavaScriptSerializer().Serialize(new Investigation()
                    {
                        Id = Investigation.Id,
                        Name = Investigation.Name,
                        IsActive = Investigation.IsActive,
                        InvestigationTypeId = model.InvestigationTypeId,
                    });
                }

                genericService.SaveOrUpdate(Investigation, Investigation.Id);

                //CommonService.SaveDataAudit(new DataAudit()
                //{
                //    Entity = "Investigations",
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


            return RedirectToAction("Index", "Investigation");
        }
    }
}