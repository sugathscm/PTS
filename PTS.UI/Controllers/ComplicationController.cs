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
    public class ComplicationController : Controller
    {
        private ApplicationUserManager _userManager;
        private readonly GenericService genericService = new GenericService();

        public ComplicationController()
        {
        }

        public ComplicationController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        // GET: Complications
        public ActionResult Index(int? id)
        {
            Complication complication = new Complication();
            if (id != null)
            {
                complication = genericService.GetList<Complication>().Where(o => o.Id == id).FirstOrDefault();
            }
            return View(complication);
        }

        public ActionResult GetList()
        {
            List<Complication> list = genericService.GetList<Complication>();

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
        public ActionResult SaveOrUpdate(Complication model)
        {
            string newData = string.Empty, oldData = string.Empty;

            try
            {
                int id = model.Id;
                Complication complication = null;
                Complication oldComplication = null;
                if (model.Id == 0)
                {
                    complication = new Complication
                    {
                        Name = model.Name,
                        IsActive = true
                    };

                    oldComplication = new Complication();
                    oldData = new JavaScriptSerializer().Serialize(oldComplication);
                    newData = new JavaScriptSerializer().Serialize(complication);
                }
                else
                {
                    complication = genericService.GetList<Complication>().Where(o => o.Id == model.Id).FirstOrDefault();
                    oldComplication = genericService.GetList<Complication>().Where(o => o.Id == model.Id).FirstOrDefault();

                    oldData = new JavaScriptSerializer().Serialize(new Complication()
                    {
                        Id = oldComplication.Id,
                        Name = oldComplication.Name,
                        IsActive = oldComplication.IsActive
                    });

                    complication.Name = model.Name;
                    bool Example = Convert.ToBoolean(Request.Form["IsActive.Value"]);
                    complication.IsActive = model.IsActive;

                    newData = new JavaScriptSerializer().Serialize(new Complication()
                    {
                        Id = complication.Id,
                        Name = complication.Name,
                        IsActive = complication.IsActive
                    });
                }

                genericService.SaveOrUpdate<Complication>(complication, complication.Id);

                //CommonService.SaveDataAudit(new DataAudit()
                //{
                //    Entity = "Complications",
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


            return RedirectToAction("Index", "Complication");
        }
    }
}