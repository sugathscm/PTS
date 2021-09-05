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
    public class ComorbidityController : Controller
    {
        private ApplicationUserManager _userManager;
        private readonly GenericService genericService = new GenericService();

        public ComorbidityController()
        {
        }

        public ComorbidityController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        // GET: Comorbiditys
        public ActionResult Index(int? id)
        {
            Comorbidity comorbidity = new Comorbidity();
            if (id != null)
            {
                comorbidity = genericService.GetList<Comorbidity>().Where(o => o.Id == id).FirstOrDefault();
            }
            return View(comorbidity);
        }

        public ActionResult GetList()
        {
            List<Comorbidity> list = genericService.GetList<Comorbidity>();

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
        public ActionResult SaveOrUpdate(Comorbidity model)
        {
            string newData = string.Empty, oldData = string.Empty;

            try
            {
                int id = model.Id;
                Comorbidity comorbidity = null;
                Comorbidity oldComorbidity = null;
                if (model.Id == 0)
                {
                    comorbidity = new Comorbidity
                    {
                        Name = model.Name,
                        IsActive = true
                    };

                    oldComorbidity = new Comorbidity();
                    oldData = new JavaScriptSerializer().Serialize(oldComorbidity);
                    newData = new JavaScriptSerializer().Serialize(comorbidity);
                }
                else
                {
                    comorbidity = genericService.GetList<Comorbidity>().Where(o => o.Id == model.Id).FirstOrDefault();
                    oldComorbidity = genericService.GetList<Comorbidity>().Where(o => o.Id == model.Id).FirstOrDefault();

                    oldData = new JavaScriptSerializer().Serialize(new Comorbidity()
                    {
                        Id = oldComorbidity.Id,
                        Name = oldComorbidity.Name,
                        IsActive = oldComorbidity.IsActive
                    });

                    comorbidity.Name = model.Name;
                    bool Example = Convert.ToBoolean(Request.Form["IsActive.Value"]);
                    comorbidity.IsActive = model.IsActive;

                    newData = new JavaScriptSerializer().Serialize(new Comorbidity()
                    {
                        Id = comorbidity.Id,
                        Name = comorbidity.Name,
                        IsActive = comorbidity.IsActive
                    });
                }

                genericService.SaveOrUpdate<Comorbidity>(comorbidity, comorbidity.Id);

                //CommonService.SaveDataAudit(new DataAudit()
                //{
                //    Entity = "Comorbiditys",
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


            return RedirectToAction("Index", "Comorbidity");
        }
    }
}