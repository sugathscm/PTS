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
    public class COPDPSQuestionController : Controller
    {
        private ApplicationUserManager _userManager;
        private readonly GenericService genericService = new GenericService();

        public COPDPSQuestionController()
        {
        }

        public COPDPSQuestionController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        // GET: COPDPSQuestions
        public ActionResult Index(int? id)
        {
            COPDPSQuestion COPDPSQuestion = new COPDPSQuestion();
            if (id != null)
            {
                COPDPSQuestion = genericService.GetList<COPDPSQuestion>().Where(o => o.Id == id).FirstOrDefault();
            }
            return View(COPDPSQuestion);
        }

        public ActionResult GetList()
        {
            List<COPDPSQuestion> list = genericService.GetList<COPDPSQuestion>();

            List<BaseViewModel> modelList = new List<BaseViewModel>();

            foreach (var item in list)
            {
                modelList.Add(new BaseViewModel() { Id = item.Id, IsActive = item.IsActive, Name = item.Question });
            }

            return Json(new { data = modelList }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveOrUpdate(COPDPSQuestion model)
        {
            string newData = string.Empty, oldData = string.Empty;

            try
            {
                int id = model.Id;
                COPDPSQuestion COPDPSQuestion = null;
                COPDPSQuestion oldCOPDPSQuestion = null;
                if (model.Id == 0)
                {
                    COPDPSQuestion = new COPDPSQuestion
                    {
                        Question = model.Question,
                        IsActive = true
                    };

                    oldCOPDPSQuestion = new COPDPSQuestion();
                    oldData = new JavaScriptSerializer().Serialize(oldCOPDPSQuestion);
                    newData = new JavaScriptSerializer().Serialize(COPDPSQuestion);
                }
                else
                {
                    COPDPSQuestion = genericService.GetList<COPDPSQuestion>().Where(o => o.Id == model.Id).FirstOrDefault();
                    oldCOPDPSQuestion = genericService.GetList<COPDPSQuestion>().Where(o => o.Id == model.Id).FirstOrDefault();

                    oldData = new JavaScriptSerializer().Serialize(new COPDPSQuestion()
                    {
                        Id = oldCOPDPSQuestion.Id,
                        Question = oldCOPDPSQuestion.Question,
                        IsActive = oldCOPDPSQuestion.IsActive
                    });

                    COPDPSQuestion.Question = model.Question;
                    bool Example = Convert.ToBoolean(Request.Form["IsActive.Value"]);
                    COPDPSQuestion.IsActive = model.IsActive;

                    newData = new JavaScriptSerializer().Serialize(new COPDPSQuestion()
                    {
                        Id = COPDPSQuestion.Id,
                        Question = COPDPSQuestion.Question,
                        IsActive = COPDPSQuestion.IsActive
                    });
                }

                genericService.SaveOrUpdate<COPDPSQuestion>(COPDPSQuestion, COPDPSQuestion.Id);

                //CommonService.SaveDataAudit(new DataAudit()
                //{
                //    Entity = "COPDPSQuestions",
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


            return RedirectToAction("Index", "COPDPSQuestion");
        }
    }
}