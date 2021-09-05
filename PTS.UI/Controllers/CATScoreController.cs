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
    public class CATScoreController : Controller
    {
        private ApplicationUserManager _userManager;
        private readonly GenericService genericService = new GenericService();

        public CATScoreController()
        {
        }

        public CATScoreController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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
            CATScore catScore = new CATScore();
            if (id != null)
            {
                catScore = genericService.GetList<CATScore>().Where(o => o.Id == id).FirstOrDefault();
            }
            return View(catScore);
        }

        public ActionResult GetList()
        {
            List<CATScore> list = genericService.GetList<CATScore>().ToList();

            List<CATScoreViewModel> modelList = new List<CATScoreViewModel>();

            foreach (var item in list)
            {
                modelList.Add(new CATScoreViewModel() { Id = item.Id, IsActive = item.IsActive, Name = item.Name, Text = item.Text });
            }

            return Json(new { data = modelList }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveOrUpdate(CATScore model)
        {
            string newData = string.Empty, oldData = string.Empty;

            try
            {
                int id = model.Id;
                CATScore catScore = null;
                CATScore oldDiastolic = null;
                if (model.Id == 0)
                {
                    catScore = new CATScore
                    {
                        Name = model.Name,
                        Text = model.Text,
                        IsActive = true
                    };

                    oldDiastolic = new CATScore();
                    oldData = new JavaScriptSerializer().Serialize(oldDiastolic);
                    newData = new JavaScriptSerializer().Serialize(catScore);
                }
                else
                {
                    catScore = genericService.GetList<CATScore>().Where(o => o.Id == model.Id).FirstOrDefault();
                    oldDiastolic = genericService.GetList<CATScore>().Where(o => o.Id == model.Id).FirstOrDefault();

                    oldData = new JavaScriptSerializer().Serialize(new CATScore()
                    {
                        Id = oldDiastolic.Id,
                        Name = oldDiastolic.Name,
                        Text = oldDiastolic.Text,
                        IsActive = oldDiastolic.IsActive
                    });

                    catScore.Name = model.Name;
                    catScore.Text = model.Text;
                    bool Example = Convert.ToBoolean(Request.Form["IsActive.Value"]);
                    catScore.IsActive = model.IsActive;

                    newData = new JavaScriptSerializer().Serialize(new CATScore()
                    {
                        Id = catScore.Id,
                        Name = catScore.Name,
                        IsActive = catScore.IsActive
                    });
                }

                genericService.SaveOrUpdate<CATScore>(catScore, catScore.Id);

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


            return RedirectToAction("Index", "CATScore");
        }
    }
}