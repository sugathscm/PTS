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
    public class PHQController : Controller
    {
        private ApplicationUserManager _userManager;
        private readonly GenericService genericService = new GenericService();

        public PHQController()
        {
        }

        public PHQController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        // GET: PHQs
        public ActionResult Index(int? id)
        {
            PHQ investigation = new PHQ();
            if (id != null)
            {
                investigation = genericService.GetList<PHQ>().Where(o => o.Id == id).FirstOrDefault();
            }

            return View(investigation);
        }

        public ActionResult GetList()
        {
            List<PHQ> list = genericService.GetList<PHQ>();

            List<PHQViewModel> modelList = new List<PHQViewModel>();

            foreach (var item in list)
            {
                modelList.Add(new PHQViewModel() { Id = item.Id, IsActive = item.IsActive, Name = item.Name });
            }

            return Json(new { data = modelList }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveOrUpdate(PHQ model)
        {
            string newData = string.Empty, oldData = string.Empty;

            try
            {
                int id = model.Id;
                PHQ PHQ = null;
                PHQ oldPHQ = null;
                if (model.Id == 0)
                {
                    PHQ = new PHQ
                    {
                        Name = model.Name,
                        IsActive = true,
                    };

                    oldPHQ = new PHQ();
                    oldData = new JavaScriptSerializer().Serialize(oldPHQ);
                    newData = new JavaScriptSerializer().Serialize(PHQ);
                }
                else
                {
                    PHQ = genericService.GetList<PHQ>().Where(o => o.Id == model.Id).FirstOrDefault();
                    oldPHQ = genericService.GetList<PHQ>().Where(o => o.Id == model.Id).FirstOrDefault();

                    oldData = new JavaScriptSerializer().Serialize(new PHQ()
                    {
                        Id = oldPHQ.Id,
                        Name = oldPHQ.Name,
                        IsActive = oldPHQ.IsActive,
                    });

                    PHQ.Name = model.Name;
                    bool Example = Convert.ToBoolean(Request.Form["IsActive.Value"]);
                    PHQ.IsActive = model.IsActive;

                    newData = new JavaScriptSerializer().Serialize(new PHQ()
                    {
                        Id = PHQ.Id,
                        Name = PHQ.Name,
                        IsActive = PHQ.IsActive,
                    });
                }

                genericService.SaveOrUpdate(PHQ, PHQ.Id);

                //CommonService.SaveDataAudit(new DataAudit()
                //{
                //    Entity = "PHQs",
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


            return RedirectToAction("Index", "PHQ");
        }
    }
}