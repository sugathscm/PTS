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
    public class COPDPSQuestionAnswerController : Controller
    {
        private ApplicationUserManager _userManager;
        private readonly GenericService genericService = new GenericService();

        public COPDPSQuestionAnswerController()
        {
        }

        public COPDPSQuestionAnswerController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        // GET: COPDPSQuestionAnswers
        public ActionResult Index(int? id)
        {
            COPDPSQuestionAnswer investigation = new COPDPSQuestionAnswer();
            if (id != null)
            {
                investigation = genericService.GetList<COPDPSQuestionAnswer>().Where(o => o.Id == id).FirstOrDefault();
            }

            ViewBag.COPDPSQuestionList = new SelectList(genericService.GetList<COPDPSQuestion>(), "Id", "Question");

            return View(investigation);
        }

        public ActionResult GetList()
        {
            List<COPDPSQuestionAnswer> list = genericService.GetList<COPDPSQuestionAnswer>();
            List<COPDPSQuestion> cOPDPSQuestionlist = genericService.GetList<COPDPSQuestion>();

            List<COPDPSQuestionAnswerViewModel> modelList = new List<COPDPSQuestionAnswerViewModel>();

            foreach (var item in list)
            {
                string cOPDPSQuestionName = cOPDPSQuestionlist.Where(it => it.Id == item.COPDPSQuestionId).FirstOrDefault().Question;
                modelList.Add(new COPDPSQuestionAnswerViewModel() { Id = item.Id, IsActive = item.IsActive, Answer = item.Answer, Question = cOPDPSQuestionName, points = item.points });
            }

            return Json(new { data = modelList }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveOrUpdate(COPDPSQuestionAnswer model)
        {
            string newData = string.Empty, oldData = string.Empty;

            try
            {
                int id = model.Id;
                COPDPSQuestionAnswer COPDPSQuestionAnswer = null;
                COPDPSQuestionAnswer oldCOPDPSQuestionAnswer = null;
                if (model.Id == 0)
                {
                    COPDPSQuestionAnswer = new COPDPSQuestionAnswer
                    {
                        Answer = model.Answer,
                        IsActive = true,
                        COPDPSQuestionId = model.COPDPSQuestionId,
                        points = model.points
                    };

                    oldCOPDPSQuestionAnswer = new COPDPSQuestionAnswer();
                    oldData = new JavaScriptSerializer().Serialize(oldCOPDPSQuestionAnswer);
                    newData = new JavaScriptSerializer().Serialize(COPDPSQuestionAnswer);
                }
                else
                {
                    COPDPSQuestionAnswer = genericService.GetList<COPDPSQuestionAnswer>().Where(o => o.Id == model.Id).FirstOrDefault();
                    oldCOPDPSQuestionAnswer = genericService.GetList<COPDPSQuestionAnswer>().Where(o => o.Id == model.Id).FirstOrDefault();

                    oldData = new JavaScriptSerializer().Serialize(new COPDPSQuestionAnswer()
                    {
                        Id = oldCOPDPSQuestionAnswer.Id,
                        Answer = oldCOPDPSQuestionAnswer.Answer,
                        IsActive = oldCOPDPSQuestionAnswer.IsActive,
                        COPDPSQuestionId = oldCOPDPSQuestionAnswer.COPDPSQuestionId,
                    });

                    COPDPSQuestionAnswer.Answer = model.Answer;
                    COPDPSQuestionAnswer.points = model.points;
                    COPDPSQuestionAnswer.COPDPSQuestionId = model.COPDPSQuestionId;
                    bool Example = Convert.ToBoolean(Request.Form["IsActive.Value"]);
                    COPDPSQuestionAnswer.IsActive = model.IsActive;

                    newData = new JavaScriptSerializer().Serialize(new COPDPSQuestionAnswer()
                    {
                        Id = COPDPSQuestionAnswer.Id,
                        Answer = COPDPSQuestionAnswer.Answer,
                        IsActive = COPDPSQuestionAnswer.IsActive,
                        COPDPSQuestionId = model.COPDPSQuestionId,
                    });
                }

                genericService.SaveOrUpdate(COPDPSQuestionAnswer, COPDPSQuestionAnswer.Id);

                //CommonService.SaveDataAudit(new DataAudit()
                //{
                //    Entity = "COPDPSQuestionAnswers",
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


            return RedirectToAction("Index", "COPDPSQuestionAnswer");
        }
    }
}