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
    public class InvestigationTypeController : Controller
    {
        private ApplicationUserManager _userManager;
        private readonly GenericService genericService = new GenericService();

        public InvestigationTypeController()
        {
        }

        public InvestigationTypeController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        // GET: InvestigationTypes
        public ActionResult Index(int? id)
        {
            InvestigationType InvestigationType = new InvestigationType();
            if (id != null)
            {
                InvestigationType = genericService.GetList<InvestigationType>().Where(o => o.Id == id).FirstOrDefault();
            }
            return View(InvestigationType);
        }

        public ActionResult GetList()
        {
            List<InvestigationType> list = genericService.GetList<InvestigationType>();

            List<BaseViewModel> modelList = new List<BaseViewModel>();

            foreach (var item in list)
            {
                modelList.Add(new BaseViewModel() { Id = item.Id, IsActive = item.IsActive, Name = item.Name });
            }

            return Json(new { data = modelList }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveOrUpdate(InvestigationType model)
        {
            string newData = string.Empty, oldData = string.Empty;

            try
            {
                int id = model.Id;
                InvestigationType InvestigationType = null;
                InvestigationType oldInvestigationType = null;
                if (model.Id == 0)
                {
                    InvestigationType = new InvestigationType
                    {
                        Name = model.Name,
                        IsActive = true
                    };

                    oldInvestigationType = new InvestigationType();
                    oldData = new JavaScriptSerializer().Serialize(oldInvestigationType);
                    newData = new JavaScriptSerializer().Serialize(InvestigationType);
                }
                else
                {
                    InvestigationType = genericService.GetList<InvestigationType>().Where(o => o.Id == model.Id).FirstOrDefault();
                    oldInvestigationType = genericService.GetList<InvestigationType>().Where(o => o.Id == model.Id).FirstOrDefault();

                    oldData = new JavaScriptSerializer().Serialize(new InvestigationType()
                    {
                        Id = oldInvestigationType.Id,
                        Name = oldInvestigationType.Name,
                        IsActive = oldInvestigationType.IsActive
                    });

                    InvestigationType.Name = model.Name;
                    bool Example = Convert.ToBoolean(Request.Form["IsActive.Value"]);
                    InvestigationType.IsActive = model.IsActive;

                    newData = new JavaScriptSerializer().Serialize(new InvestigationType()
                    {
                        Id = InvestigationType.Id,
                        Name = InvestigationType.Name,
                        IsActive = InvestigationType.IsActive
                    });
                }

                genericService.SaveOrUpdate<InvestigationType>(InvestigationType, InvestigationType.Id);

                //CommonService.SaveDataAudit(new DataAudit()
                //{
                //    Entity = "InvestigationTypes",
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


            return RedirectToAction("Index", "InvestigationType");
        }
    }
}