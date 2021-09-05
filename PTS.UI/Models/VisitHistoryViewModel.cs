using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PTS.UI.Models
{
    public class VisitHistoryViewModel
    {
        public List<string> Date { get; set; } = new List<string>(); 
        public List<string> Compalaints { get; set; } = new List<string>();
        public List<string> SystemsOfExacerbation { get; set; } = new List<string>();
        public List<string> mMRC { get; set; } = new List<string>();
        public List<string> AnkleOedema { get; set; } = new List<string>();
        public List<string> BloodPressure { get; set; } = new List<string>();
        public List<string> WeightBMI { get; set; } = new List<string>();
        public List<string> PEFR { get; set; } = new List<string>();
        public List<string> BODEIndex { get; set; } = new List<string>();
        public List<string> SixminWalkTestSpO2Atrest { get; set; } = new List<string>();
        public List<string> SixminWalkTestSpO2After6minwalk { get; set; } = new List<string>();
        public List<string> SixminWalkTestDistanceWalked { get; set; } = new List<string>();
        public List<string> SpirometryFEV1 { get; set; } = new List<string>();
        public List<string> SpirometryFEV1Predicted { get; set; } = new List<string>();
        public List<string> SpirometryFVC { get; set; } = new List<string>();
        public List<string> SpirometryFEV1FVC { get; set; } = new List<string>();
        public List<string> SpirometryFEF25ofFVC { get; set; } = new List<string>();
        public List<string> SpirometryFEF50ofFVC { get; set; } = new List<string>();
        public List<string> SpirometryFEF75ofFVC { get; set; } = new List<string>();
        public List<string> PostBronchodilatorFEV1 { get; set; } = new List<string>();
        public List<string> PulmonaryRehabilitationSmokingStatus { get; set; } = new List<string>();
        public List<string> PulmonaryRehabilitationExerciseTraining { get; set; } = new List<string>();
        public List<string> PulmonaryRehabilitationNutritionalAdvice { get; set; } = new List<string>();
        public List<string> AdherenceToTreatmentCompliance { get; set; } = new List<string>();
        public List<string> AdherenceToTreatmentTechnique { get; set; } = new List<string>();
        public List<string> HealthEducation { get; set; } = new List<string>();
        public List<string> SmokingStatus { get; set; } = new List<string>();
        public List<string> DepressionAssessedByPHQ9 { get; set; } = new List<string>();
        public List<string> Vaccination { get; set; } = new List<string>();
    }
}