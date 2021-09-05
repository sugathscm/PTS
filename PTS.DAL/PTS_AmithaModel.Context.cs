﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PTS.DAL
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class PTS_AmithaEntities : DbContext
    {
        public PTS_AmithaEntities()
            : base("name=PTS_AmithaEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Comorbidity> Comorbidities { get; set; }
        public virtual DbSet<Complication> Complications { get; set; }
        public virtual DbSet<FamilyHistory> FamilyHistories { get; set; }
        public virtual DbSet<Gender> Genders { get; set; }
        public virtual DbSet<MaritialStatu> MaritialStatus { get; set; }
        public virtual DbSet<mMRCGrade> mMRCGrades { get; set; }
        public virtual DbSet<PhysicalExamination> PhysicalExaminations { get; set; }
        public virtual DbSet<SmokingHistory> SmokingHistories { get; set; }
        public virtual DbSet<Title> Titles { get; set; }
        public virtual DbSet<VaccinationHistory> VaccinationHistories { get; set; }
        public virtual DbSet<CATScore> CATScores { get; set; }
        public virtual DbSet<PatientMHCatScore> PatientMHCatScores { get; set; }
        public virtual DbSet<ExacerbationHistory> ExacerbationHistories { get; set; }
        public virtual DbSet<Investigation> Investigations { get; set; }
        public virtual DbSet<PatientInvestigation> PatientInvestigations { get; set; }
        public virtual DbSet<InvestigationType> InvestigationTypes { get; set; }
        public virtual DbSet<PatientMedicalHistory> PatientMedicalHistories { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<CATScoreIntepretation> CATScoreIntepretations { get; set; }
        public virtual DbSet<FEV1Staging> FEV1Staging { get; set; }
        public virtual DbSet<COPDPSQuestion> COPDPSQuestions { get; set; }
        public virtual DbSet<COPDPSQuestionAnswer> COPDPSQuestionAnswers { get; set; }
        public virtual DbSet<GOLDFEV1Staging> GOLDFEV1Staging { get; set; }
        public virtual DbSet<BODEIndex> BODEIndexes { get; set; }
        public virtual DbSet<PHQ> PHQs { get; set; }
    }
}
