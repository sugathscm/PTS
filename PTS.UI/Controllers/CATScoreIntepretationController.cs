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
    public class CATScoreIntepretationController : Controller
    {
        private ApplicationUserManager _userManager;
        private readonly GenericService genericService = new GenericService();

        public CATScoreIntepretationController()
        {
        }

        public CATScoreIntepretationController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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
            CATScoreIntepretation catScore = new CATScoreIntepretation();
            if (id != null)
            {
                catScore = genericService.GetList<CATScoreIntepretation>().Where(o => o.Id == id).FirstOrDefault();
            }
            return View(catScore);
        }

        public ActionResult GetList()
        {
            List<CATScoreIntepretation> list = genericService.GetList<CATScoreIntepretation>().OrderBy(o => o.Id).ToList();

            List<CATScoreIntepretationViewModel> modelList = new List<CATScoreIntepretationViewModel>();

            foreach (var item in list)
            {
                modelList.Add(new CATScoreIntepretationViewModel() { Id = item.Id, Range = item.Range, IsActive = item.IsActive, Name = item.Name, Text = item.Text });
            }

            return Json(new { data = modelList }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveOrUpdate(CATScoreIntepretation model)
        {
            string newData = string.Empty, oldData = string.Empty;

            try
            {
                int id = model.Id;
                CATScoreIntepretation catScore = null;
                CATScoreIntepretation oldDiastolic = null;
                if (model.Id == 0)
                {
                    catScore = new CATScoreIntepretation
                    {
                        Range = model.Range,
                        Name = model.Name,
                        Text = model.Text,
                        DetailList = model.DetailList,
                        IsActive = true
                    };

                    oldDiastolic = new CATScoreIntepretation();
                    oldData = new JavaScriptSerializer().Serialize(oldDiastolic);
                    newData = new JavaScriptSerializer().Serialize(catScore);
                }
                else
                {
                    catScore = genericService.GetList<CATScoreIntepretation>().Where(o => o.Id == model.Id).FirstOrDefault();
                    oldDiastolic = genericService.GetList<CATScoreIntepretation>().Where(o => o.Id == model.Id).FirstOrDefault();

                    oldData = new JavaScriptSerializer().Serialize(new CATScoreIntepretation()
                    {
                        Id = oldDiastolic.Id,
                        Name = oldDiastolic.Name,
                        Text = oldDiastolic.Text,
                        IsActive = oldDiastolic.IsActive
                    });

                    catScore.Range = model.Range;
                    catScore.Name = model.Name;
                    catScore.Text = model.Text;
                    catScore.DetailList = model.DetailList;
                    bool Example = Convert.ToBoolean(Request.Form["IsActive.Value"]);
                    catScore.IsActive = model.IsActive;

                    newData = new JavaScriptSerializer().Serialize(new CATScoreIntepretation()
                    {
                        Id = catScore.Id,
                        Name = catScore.Name,
                        IsActive = catScore.IsActive
                    });
                }

                genericService.SaveOrUpdate<CATScoreIntepretation>(catScore, catScore.Id);

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


            return RedirectToAction("Index", "CATScoreIntepretation");
        }

        public ActionResult GetByCATScore(string score)
        {
            score = (score == null) ? "0" : score;

            List<CATScoreIntepretation> list = genericService.GetList<CATScoreIntepretation>().OrderBy(o => o.Id).ToList();

            CATScoreIntepretation cATScoreIntepretation = new CATScoreIntepretation();

            cATScoreIntepretation = list.Where(c => int.Parse(score) <= (c.Id * 10) && int.Parse(score) >= ((c.Id-1) * 10)).FirstOrDefault();

            return Json(new { data = cATScoreIntepretation }, JsonRequestBehavior.AllowGet);
        }

    }
}