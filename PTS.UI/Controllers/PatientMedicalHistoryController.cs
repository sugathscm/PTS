using Newtonsoft.Json;
using PTS.BAL;
using PTS.DAL;
using PTS.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PTS.UI.Controllers
{
    [Authorize]
    public class PatientMedicalHistoryController : Controller
    {
        private readonly GenericService genericService = new GenericService();
        private readonly PatientService patientService = new PatientService();
        private readonly PatientMedicalHistoryService patientMedicalHistoryService = new PatientMedicalHistoryService();

        // GET: Patient
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetList()
        {
            List<PatientMedicalHistory> repoerList = genericService.GetList<PatientMedicalHistory>();
            List<Patient> patientList = genericService.GetList<Patient>();

            List<PatientMedicalHistoryViewModel> modelList = new List<PatientMedicalHistoryViewModel>();

            foreach (var item in repoerList)
            {
                Patient patient = patientList.Where(p => p.Id == item.PatientId.Value).FirstOrDefault();
                modelList.Add(new PatientMedicalHistoryViewModel() { Id = item.Id, PatientName = patient.FirstName + " " + patient.LastName });
            }

            return Json(new { data = modelList }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPatient(int? id)
        {
            var patient = genericService.GetList<Patient>().Select(p => new
            {
                p.Id,
                p.FirstName,
                p.LastName,
                p.TitleId,
                p.GenderId,
                p.DOB,
                p.Email,
                p.Height,
                p.MaritialStatusId,
                p.Mobile,
                p.NIC,
                p.Occupation,
                p.Weight
            }).Where(p => p.Id == id).FirstOrDefault();
            return Json(patient, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Details(int? id)
        {
            PatientMedicalHistory patientMedicalHistory = new PatientMedicalHistory();
            PatientMedicalHistoryViewModel medicalHistoryViewModel = new PatientMedicalHistoryViewModel();

            patientMedicalHistory = genericService.GetList<PatientMedicalHistory>().Where(mh => mh.Id == id).FirstOrDefault();

            //Patient Details
            ViewBag.GenderList = new SelectList(genericService.GetList<Gender>(), "Id", "Name");
            ViewBag.TitleList = new SelectList(genericService.GetList<Title>(), "Id", "Name");
            ViewBag.MaritialStatuList = new SelectList(genericService.GetList<MaritialStatu>(), "Id", "Name");

            //Clinical
            var logicalList = new List<LogicalItem>
            {
                new LogicalItem() { Id = 1, Name = "Yes" },
                new LogicalItem() { Id = 2, Name = "No" }
            };

            ViewBag.LogicalList = new SelectList(logicalList, "Id", "Name");
            ViewBag.ComorbidityList = new SelectList(genericService.GetList<Comorbidity>(), "Id", "Name");
            ViewBag.SmokingHistoryList = new SelectList(genericService.GetList<SmokingHistory>(), "Id", "Name");
            ViewBag.FamilyHistoryList = new SelectList(genericService.GetList<FamilyHistory>(), "Id", "Name");
            ViewBag.VaccinationHistoryList = new SelectList(genericService.GetList<VaccinationHistory>(), "Id", "Name");
            ViewBag.ComplicationList = new SelectList(genericService.GetList<Complication>(), "Id", "Name");
            ViewBag.PhysicalExaminationList = new SelectList(genericService.GetList<PhysicalExamination>(), "Id", "Name");
            ViewBag.ExacerbationHistoryList = new SelectList(genericService.GetList<ExacerbationHistory>(), "Id", "Name");
            ViewBag.FEV1StagingList = new SelectList(genericService.GetList<FEV1Staging>(), "Id", "Text");
            ViewBag.GOLDFEV1StagingList = new SelectList(genericService.GetList<GOLDFEV1Staging>(), "Id", "Text");

            var BODEIndexes = genericService.GetList<BODEIndex>();
            var BODEIndexVariable1List = new List<BaseViewModel>();
            var BODEIndexVariable2List = new List<BaseViewModel>();
            var BODEIndexVariable3List = new List<BaseViewModel>();
            var BODEIndexVariable4List = new List<BaseViewModel>();

            foreach (var BODEIndex in BODEIndexes)
            {
                if (BODEIndex.VAriable == "BMI (Kg/m2)")
                {
                    BODEIndexVariable1List.Add(new BaseViewModel() { Id = 1, Name = BODEIndex.Point1 });
                    BODEIndexVariable1List.Add(new BaseViewModel() { Id = 2, Name = BODEIndex.Point2 });
                    BODEIndexVariable1List.Add(new BaseViewModel() { Id = 3, Name = BODEIndex.Point3 });
                    BODEIndexVariable1List.Add(new BaseViewModel() { Id = 4, Name = BODEIndex.Point4 });
                }

                if (BODEIndex.VAriable == "FEV1 % predicted")
                {
                    BODEIndexVariable2List.Add(new BaseViewModel() { Id = 1, Name = BODEIndex.Point1 });
                    BODEIndexVariable2List.Add(new BaseViewModel() { Id = 2, Name = BODEIndex.Point2 });
                    BODEIndexVariable2List.Add(new BaseViewModel() { Id = 3, Name = BODEIndex.Point3 });
                    BODEIndexVariable2List.Add(new BaseViewModel() { Id = 4, Name = BODEIndex.Point4 });
                }
                if (BODEIndex.VAriable == "mMRC scale")
                {
                    BODEIndexVariable3List.Add(new BaseViewModel() { Id = 1, Name = BODEIndex.Point1 });
                    BODEIndexVariable3List.Add(new BaseViewModel() { Id = 2, Name = BODEIndex.Point2 });
                    BODEIndexVariable3List.Add(new BaseViewModel() { Id = 3, Name = BODEIndex.Point3 });
                    BODEIndexVariable3List.Add(new BaseViewModel() { Id = 4, Name = BODEIndex.Point4 });
                }
                if (BODEIndex.VAriable == "6MWT distance (m)")
                {
                    BODEIndexVariable4List.Add(new BaseViewModel() { Id = 1, Name = BODEIndex.Point1 });
                    BODEIndexVariable4List.Add(new BaseViewModel() { Id = 2, Name = BODEIndex.Point2 });
                    BODEIndexVariable4List.Add(new BaseViewModel() { Id = 3, Name = BODEIndex.Point3 });
                    BODEIndexVariable4List.Add(new BaseViewModel() { Id = 4, Name = BODEIndex.Point4 });
                }
            }

            ViewBag.BODEIndexVariable1List = new SelectList(BODEIndexVariable1List, "Id", "Name");
            ViewBag.BODEIndexVariable2List = new SelectList(BODEIndexVariable2List, "Id", "Name");
            ViewBag.BODEIndexVariable3List = new SelectList(BODEIndexVariable3List, "Id", "Name");
            ViewBag.BODEIndexVariable4List = new SelectList(BODEIndexVariable4List, "Id", "Name");

            ViewBag.PatientList = new SelectList(genericService.GetList<Patient>(), "Id", "FirstName");

            ViewBag.InvestigationList = genericService.GetList<Investigation>();
            ViewBag.InvestigationTypeList = genericService.GetList<InvestigationType>();

            List<mMRCGrade> list = genericService.GetList<mMRCGrade>();
            List<BaseViewModel> modelList = new List<BaseViewModel>();
            foreach (var item in list)
            {
                modelList.Add(new BaseViewModel() { Id = item.Id, IsActive = item.IsActive, Name = item.Weight + "-" + item.Name });
            }
            ViewBag.mMRCGradeList = new SelectList(modelList, "Id", "Name");

            medicalHistoryViewModel.Patient = genericService.GetList<Patient>().Where(p => p.Id == medicalHistoryViewModel.PatientId).FirstOrDefault();
            medicalHistoryViewModel.CATScoreList = genericService.GetList<CATScore>();

            if (patientMedicalHistory != null)
            {
                PropertyCopier<PatientMedicalHistory, PatientMedicalHistoryViewModel>.Copy(patientMedicalHistory, medicalHistoryViewModel);

                ViewBag.VisitHistory = FillVisitHistory(medicalHistoryViewModel.PatientId.Value);
            }
            return View(medicalHistoryViewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveOrUpdate(PatientMedicalHistoryViewModel model, FormCollection formCollection)
        {
            string newData = string.Empty, oldData = string.Empty;
            PatientMedicalHistory patientMedicalHistory = null;
            Patient patient = null;

            var comorbidityArray = formCollection["comorbidityArray"].Split(',');
            var familyHistoryArray = formCollection["familyHistoryArray"].Split(',');
            var complicationsArray = formCollection["complicationsArray"].Split(',');
            var catscoreArray = formCollection["catscoreArray"].Split(',');

            try
            {
                int id = model.Id;

                if (id == 0)
                {
                    patientMedicalHistory = new PatientMedicalHistory();

                    PropertyCopier<PatientMedicalHistoryViewModel, PatientMedicalHistory>.Copy(model, patientMedicalHistory);

                    if (model.PatientId == null)
                    {
                        patientMedicalHistory.Patient.GenderId = model.GenderId;
                        patientMedicalHistory.Patient.MaritialStatusId = model.MaritialStatusId;
                        patientMedicalHistory.Patient.TitleId = model.TitleId;
                        patientMedicalHistory.DateCreated = DateTime.Now;
                        patientMedicalHistory.Patient.Designation = model.Designation;
                        patientMedicalHistory.Patient.EmergencyContactName = model.EmergencyContactName;
                        patientMedicalHistory.Patient.EmergencyContactNo = model.EmergencyContactNo;
                        patientMedicalHistory.Patient.CodePrefix = SetCodePrefix(model.Patient.DOB);
                        patientMedicalHistory.Patient.CodeNumber = SetCodeNumber(model.Patient.DOB);
                    }
                    else
                    {
                        patient = patientService.GetPatientById(model.PatientId);
                        patient.FirstName = model.Patient.FirstName;
                        patient.LastName = model.Patient.LastName;
                        patient.Weight = model.Patient.Weight;
                        patient.Height = model.Patient.Height;
                        patient.TitleId = model.TitleId;
                        patient.GenderId = model.GenderId;
                        patient.MaritialStatusId = model.MaritialStatusId;
                        patient.Designation = model.Designation;
                        patient.EmergencyContactName = model.EmergencyContactName;
                        patient.EmergencyContactNo = model.EmergencyContactNo;
                        patient.CodePrefix = SetCodePrefix(model.Patient.DOB);
                        patient.CodeNumber = SetCodeNumber(model.Patient.DOB);


                        patientService.SaveOrUpdate(patient);

                        patientMedicalHistory.PatientId = model.PatientId;
                    }
                }
                else
                {
                    patientMedicalHistory = genericService.GetList<PatientMedicalHistory>().Where(mh => mh.Id == id).FirstOrDefault();
                    PropertyCopier<PatientMedicalHistoryViewModel, PatientMedicalHistory>.Copy(model, patientMedicalHistory);
                }

                patientMedicalHistory.ComorbidityId = formCollection["comorbidityArray"];
                patientMedicalHistory.ComorbidityOthers = formCollection["ComorbidityOthers"];
                patientMedicalHistory.FamilyHistoryId = formCollection["familyHistoryArray"];
                patientMedicalHistory.FamilyHistoryOthers = formCollection["FamilyHistoryOthers"];
                patientMedicalHistory.ComplicationId = formCollection["complicationsArray"];
                patientMedicalHistory.ComplicationOther = formCollection["ComplicationOther"];
                patientMedicalHistory.CATScore = formCollection["catscoreArray"];

                genericService.SaveOrUpdate(patientMedicalHistory, model.Id);

                TempData["Message"] = ResourceData.SaveSuccessMessage;
            }
            catch (Exception ex)
            {
                TempData["Message"] = string.Format(ResourceData.SaveErrorMessage, ex.InnerException);
            }

            return RedirectToAction("Index", "PatientMedicalHistory");
        }

        private VisitHistoryViewModel FillVisitHistory(int PatientId)
        {
            VisitHistoryViewModel visitHistoryViewModel = new VisitHistoryViewModel();

            var medicalHistoryList = patientMedicalHistoryService.GetPatientMedicalHistoryListByPatient(PatientId).OrderByDescending(mh => mh.DateCreated).Take(3).ToList();

            foreach (var medicalHistory in medicalHistoryList)
            {
                visitHistoryViewModel.Date.Add(medicalHistory.DateCreated.Value.ToShortDateString());
                visitHistoryViewModel.Compalaints.Add("");
                visitHistoryViewModel.SystemsOfExacerbation.Add("");
                visitHistoryViewModel.mMRC.Add((medicalHistory.DyspnoeaId.Value == 1) ? "Yes" : "No" + " (" + ((medicalHistory.mMRCGradeId == null) ? "" : medicalHistory.mMRCGrade.Name) + ")");
                visitHistoryViewModel.AnkleOedema.Add((medicalHistory.PhysicalExaminationId != null) ? ((medicalHistory.PhysicalExaminationId.Contains("3")) ? "Yes" : "No") : "");
                visitHistoryViewModel.BloodPressure.Add(medicalHistory.BloodPressure);
                visitHistoryViewModel.WeightBMI.Add(medicalHistory.BMI);
                visitHistoryViewModel.PEFR.Add("");
                visitHistoryViewModel.BODEIndex.Add("");
                visitHistoryViewModel.SixminWalkTestSpO2Atrest.Add("");
                visitHistoryViewModel.SixminWalkTestSpO2After6minwalk.Add("");
                visitHistoryViewModel.SixminWalkTestDistanceWalked.Add("");
                visitHistoryViewModel.SpirometryFEV1.Add("");
                visitHistoryViewModel.SpirometryFEV1Predicted.Add("");
                visitHistoryViewModel.SpirometryFVC.Add("");
                visitHistoryViewModel.SpirometryFEV1FVC.Add("");
                visitHistoryViewModel.SpirometryFEF25ofFVC.Add("");
                visitHistoryViewModel.SpirometryFEF50ofFVC.Add("");
                visitHistoryViewModel.SpirometryFEF75ofFVC.Add("");
                visitHistoryViewModel.PostBronchodilatorFEV1.Add("");
                visitHistoryViewModel.PulmonaryRehabilitationSmokingStatus.Add("");
                visitHistoryViewModel.PulmonaryRehabilitationExerciseTraining.Add("");
                visitHistoryViewModel.PulmonaryRehabilitationNutritionalAdvice.Add("");
                visitHistoryViewModel.AdherenceToTreatmentCompliance.Add("");
                visitHistoryViewModel.AdherenceToTreatmentTechnique.Add("");
                visitHistoryViewModel.HealthEducation.Add("");
                visitHistoryViewModel.SmokingStatus.Add((medicalHistory.SmokingHistoryId != null) ? medicalHistory.SmokingHistory.Name : "");
                visitHistoryViewModel.DepressionAssessedByPHQ9.Add("");
                visitHistoryViewModel.Vaccination.Add("");
            }

            return visitHistoryViewModel;

        }

        public ActionResult GetInvestigations(string investigations)
        {
            string[] investigationList = investigations.Split(',');

            int[] investigationIdList = Array.ConvertAll(investigationList, s => int.Parse(s));

            List<Investigation> list = genericService.GetList<Investigation>().Where(i => investigationIdList.Contains(i.Id)).OrderBy(o => o.Id).ToList();

            List<BaseViewModel> nameList = new List<BaseViewModel>();

            foreach (var item in list)
            {
                nameList.Add(new BaseViewModel() { Id = item.Id, Name = item.Name });
            }

            return PartialView("_Investigations", nameList);
        }

        public class LogicalItem
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }
        }

        private class PropertyCopier<TParent, TChild> where TParent : class where TChild : class
        {
            public static void Copy(TParent parent, TChild child)
            {
                var parentProperties = parent.GetType().GetProperties();
                var childProperties = child.GetType().GetProperties();

                foreach (var parentProperty in parentProperties)
                {
                    foreach (var childProperty in childProperties)
                    {
                        if (parentProperty.Name == childProperty.Name && parentProperty.PropertyType == childProperty.PropertyType)
                        {
                            try
                            {
                                childProperty.SetValue(child, parentProperty.GetValue(parent));
                            }
                            catch (Exception)
                            {
                                continue;
                            }

                            break;
                        }
                    }
                }
            }
        }

        private string SetCodePrefix(DateTime? DOB)
        {
            string prefix = "";

            if (DOB != null)
            {
                prefix = DOB.Value.ToString("yyMMdd");
            }

            return prefix;
        }

        private int? SetCodeNumber(DateTime? DOB)
        {
            int? number = 0;

            if (DOB != null)
            {
                number = patientService.GetMaxCodeNumber(DOB.Value.ToString("yyMMdd"));

                if (number == null)
                {
                    number = 0;
                }
                else
                {
                    number = number + 1;
                }
            }

            return number;
        }


    }
}