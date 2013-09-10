using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GuruETC.DB;

namespace GuruETC.Data
{
    public class QuestionManage
    {
        public static List<Question> GetPartFirstList()
        {
            GuruETCEntities _etc = new GuruETCEntities();
            return _etc.Questions.OrderBy(d => d.CatId).ToList();
        }

        public static PatientRecord GetPatientAnswer(Guid _pkey)
        {
            GuruETCEntities _etc = new GuruETCEntities();
            long? ParentId = _etc.PatientProfiles.Where(d => d.UserGuid == _pkey).Select(d => d.Id).FirstOrDefault();
            long? DoctorId = _etc.PatientProfiles.Where(d => d.UserGuid == _pkey).Select(d => d.DoctorId).FirstOrDefault();
            List<PMed> Medi = new List<PMed>();
            List<PSup> Supli = new List<PSup>();
            List<PHos> Hosp = new List<PHos>();
            PatientRecord _existrecord = new PatientRecord();


            long? ExamId = _etc.PatientExams.Where(d => d.PatientId == ParentId && d.DoctorId == DoctorId).OrderByDescending(d => d.ExamId).Select(d => d.ExamId).FirstOrDefault();
            PatientResultQuestion _pResult = _etc.PatientResultQuestions.Where(d => d.ExamId == ExamId).FirstOrDefault();
            if (_pResult != null)
            {
                Medi = (from item in _etc.PatientMedications
                        where (item.PHealthId == _pResult.PHealthId)
                        select new PMed()
                        {
                            MedId = item.MedicationId,
                            Title = item.Title,
                            Date = item.DateAdded.Value
                        }).ToList();

                Supli = (from item in _etc.PatientSupplements
                         where (item.PHealthId == _pResult.PHealthId)
                         select new PSup()
                         {
                             SupId = item.SupplementId,
                             Title = item.Title,
                             Date = item.DateAdded.Value
                         }).ToList();

                Hosp = (from item in _etc.PatientHospitalizeds
                        where (item.PHealthId == _pResult.PHealthId)
                        select new PHos()
                        {
                            HosId = item.HospitalizationId,
                            Title = item.Title,
                            Date = item.DateAdded.Value
                        }).ToList();

                _existrecord.Answer = _pResult.patient_concerns;
                _existrecord.meditation = Medi;
                _existrecord.Supliment = Supli;
                _existrecord.Hospital = Hosp;
            }
            return _existrecord;
        }


        /*Save Patient Meditation, supplement, hospitalization etc.. info*/
        public static string SavePatientInfo(string key, string title, Guid _pkey)
        {
            GuruETCEntities _etc = new GuruETCEntities();
            long? ParentId = _etc.PatientProfiles.Where(d => d.UserGuid == _pkey).Select(d => d.Id).FirstOrDefault();
            long? ExamId = _etc.PatientExams.Where(d => d.PatientId == ParentId).OrderByDescending(d => d.ExamId).Select(d=>d.ExamId).FirstOrDefault();
            long? PhealthId = _etc.PatientResultQuestions.Where(d => d.ExamId == ExamId).Select(d => d.PHealthId).FirstOrDefault();
            string Result = string.Empty;
            switch (key)
            {
                case "1":
                    PatientMedication _pmedi = new PatientMedication();
                    _pmedi.Title = title;
                    _pmedi.PHealthId = PhealthId;       //By default 1 change it dynamically
                    _pmedi.DateAdded = DateTime.Now;
                    _etc.AddToPatientMedications(_pmedi);
                    _etc.SaveChanges();
                    Result = _pmedi.MedicationId.ToString();
                    break;

                case "2":
                    PatientSupplement _psup = new PatientSupplement();
                    _psup.Title = title;
                    _psup.PHealthId = PhealthId;       //By default 1 change it dynamically
                    _psup.DateAdded = DateTime.Now;
                    _etc.AddToPatientSupplements(_psup);
                    _etc.SaveChanges();
                    Result = _psup.SupplementId.ToString();
                    break;

                case "3":
                    PatientHospitalized _phos = new PatientHospitalized();
                    _phos.Title = title;
                    _phos.PHealthId = PhealthId;       //By default 1 change it dynamically
                    _phos.DateAdded = DateTime.Now;
                    _etc.AddToPatientHospitalizeds(_phos);
                    _etc.SaveChanges();
                    Result = _phos.HospitalizationId.ToString();
                    break;
            }


            return Result;
        }

        public static string DeletePatientInfo(string key, string delId)
        {
            GuruETCEntities _etc = new GuruETCEntities();
            long GetId = int.Parse(delId);
            string Result = string.Empty;
            switch (key)
            {
                case "1":
                    PatientMedication _pmedi = _etc.PatientMedications.Where(d => d.MedicationId == GetId).FirstOrDefault();
                    if (_pmedi != null)
                    {
                        _etc.DeleteObject(_pmedi);
                        _etc.SaveChanges();
                    }
                    Result = "Success";
                    break;

                case "2":
                    PatientSupplement _psup = _etc.PatientSupplements.Where(d => d.SupplementId == GetId).FirstOrDefault();
                    if (_psup != null)
                    {
                        _etc.DeleteObject(_psup);
                        _etc.SaveChanges();
                    }
                    Result = "Success";
                    break;

                case "3":
                    PatientHospitalized _phos = _etc.PatientHospitalizeds.Where(d => d.HospitalizationId == GetId).FirstOrDefault();
                    if (_phos != null)
                    {
                        _etc.DeleteObject(_phos);
                        _etc.SaveChanges();
                    }
                    Result = "Success";
                    break;
            }

            return Result;
        }

        public static string GetPatientVitalInfo(Guid _pkey)
        {
          
            GuruETCEntities _etc = new GuruETCEntities();
            long? ParentId = _etc.PatientProfiles.Where(d => d.UserGuid == _pkey).Select(d => d.Id).FirstOrDefault();
            long? DoctorId = _etc.PatientProfiles.Where(d => d.UserGuid == _pkey).Select(d => d.DoctorId).FirstOrDefault();
            string Result = string.Empty;
            long? ExamId = _etc.PatientExams.Where(d => d.PatientId == ParentId && d.DoctorId == DoctorId).Max(d => d.ExamId);
            PatientResultQuestion _pResult = _etc.PatientResultQuestions.Where(d => d.ExamId == ExamId).FirstOrDefault();
            if (_pResult != null)
            {
                Result = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}", _pResult.Weight, _pResult.Height, _pResult.feet, _pResult.Packs, _pResult.Cans, _pResult.FormerSmoker, _pResult.FormerChewer, _pResult.dietrating);
            }
            return Result;
        }

        public static string SavePatienthx_Detail(string hxConcerns, string duplicateDental, string duplicateENT, string duplicateSleep, Guid _pkey)
        {
            GuruETCEntities _etc = new GuruETCEntities();
            long? ParentId = _etc.PatientProfiles.Where(d => d.UserGuid == _pkey).Select(d => d.Id).FirstOrDefault();
            long? DoctorId = _etc.PatientProfiles.Where(d => d.UserGuid == _pkey).Select(d => d.DoctorId).FirstOrDefault();
            long? ExamId = _etc.PatientExams.Where(d => d.DoctorId == DoctorId && d.PatientId == ParentId).OrderByDescending(d => d.ExamId).Select(d => d.ExamId).FirstOrDefault();
            if (ExamId == 0)
            {
                PatientExam _newexam = new PatientExam();
                _newexam.PatientId = ParentId;
                _newexam.DoctorId = DoctorId;
                _newexam.Date = DateTime.Now;
                _etc.AddToPatientExams(_newexam);
                _etc.SaveChanges();
                ExamId = _newexam.ExamId;
            }
            PatientResultQuestion _Presult = _etc.PatientResultQuestions.Where(p => p.DoctorId == DoctorId && p.PatientId == ParentId).FirstOrDefault();
            if (_Presult != null)
            {
                //Duplicate Dental items

                string dentalCheckedItems = string.Empty;
                string[] findingitems = duplicateDental.Split('|');
                string[] MatchArray = { "16", "12", "17", "19", "5" };
                foreach (var finding in findingitems)
                {

                    if (finding == "15") { dentalCheckedItems += "16|"; }
                    if (finding == "16") { dentalCheckedItems += "12|"; }
                    if (finding == "36") { dentalCheckedItems += "17|"; }
                    if (finding == "18") { dentalCheckedItems += "19|"; }
                    if (finding == "19") { dentalCheckedItems += "19|"; }
                    if (finding == "20") { dentalCheckedItems += "5|"; }
                }
                IEnumerable<string> intersectDental;
                string[] alreadyitems = (!string.IsNullOrEmpty(_Presult.hx_dental)) ? _Presult.hx_dental.Split('|') : null;
                if (alreadyitems != null)
                {
                  //  string[] arry1 = alreadyitems.Except(MatchArray).ToArray();
                  //  string[] arry2 = MatchArray.Except(alreadyitems).ToArray();

                    //Removes duplicate values
                    intersectDental = alreadyitems.Concat(MatchArray);
                }
                else
                    intersectDental = MatchArray;

                dentalCheckedItems = string.Join("|", intersectDental);

                //Duplicate ENT items

                string entCheckedItems = string.Empty;
                string[] entfindingitems = duplicateENT.Split('|');
                string[] entMatchArray = { "2", "4", "1", "5", "8" };
                foreach (var finding in entfindingitems)
                {

                    if (finding == "27") { entCheckedItems += "2|"; }
                    if (finding == "28") { entCheckedItems += "4|"; }
                    if (finding == "29") { entCheckedItems += "1|"; }
                    if (finding == "30") { entCheckedItems += "5|"; }
                    if (finding == "45") { entCheckedItems += "8|"; }

                }


                IEnumerable<string> intersectENT;
                string[] entitems = (!string.IsNullOrEmpty(_Presult.hx_medical_ent)) ? _Presult.hx_medical_ent.Split('|') : null;
                if (entitems != null)
                {
                   // string[] arry1 = entitems.Except(entMatchArray).ToArray();
                   // string[] arry2 = entMatchArray.Except(entitems).ToArray();
                   
                    //Removes duplicate values
                    intersectENT = entitems.Union(entMatchArray);
                }
                else
                    intersectENT = entMatchArray;

                entCheckedItems = string.Join("|", intersectENT);


                //Duplicate Sleep items

                string SleepCheckedItems = string.Empty;
                string[] Sleepfindingitems = duplicateSleep.Split('|');
                string[] SleepMatchArray = { "4", "4.1", "1", "2" };
                foreach (var finding in Sleepfindingitems)
                {

                    if (finding == "44") { SleepCheckedItems += "4|4.1|"; }
                    if (finding == "45") { SleepCheckedItems += "1|"; }
                    if (finding == "46") { SleepCheckedItems += "2|"; }


                }

                IEnumerable<string> intersectSleep;
                string[] sleepitems = (!string.IsNullOrEmpty(_Presult.hx_medical_sleep)) ? _Presult.hx_medical_sleep.Split('|') : null;
                if (sleepitems != null)
                {
                  //  string[] arry1 = sleepitems.Except(SleepMatchArray).ToArray();
                  //  string[] arry2 = SleepMatchArray.Except(sleepitems).ToArray();

                    //Removes duplicate values
                    intersectSleep = sleepitems.Concat(SleepMatchArray);
                }

                else
                    intersectSleep = SleepMatchArray;

                SleepCheckedItems = string.Join("|", intersectSleep);

                _Presult.hx_medical_sleep = SleepCheckedItems;
                _Presult.hx_medical_ent = entCheckedItems;
                _Presult.hx_dental = dentalCheckedItems;
                _Presult.patient_concerns = hxConcerns;
                _etc.SaveChanges();
                if (!string.IsNullOrEmpty(_Presult.hx_medications) && !string.IsNullOrEmpty(_Presult.hx_allergies))
                    return _Presult.hx_medications + "|" + _Presult.hx_allergies;
                else if (!string.IsNullOrEmpty(_Presult.hx_medications) && string.IsNullOrEmpty(_Presult.hx_allergies))
                    return _Presult.hx_medications;
                else
                    return _Presult.hx_allergies;

            }
            else
            {
                PatientResultQuestion _addPResult = new PatientResultQuestion();
                _addPResult.PatientId = ParentId;
                _addPResult.DoctorId = DoctorId;
                _addPResult.ExamId = ExamId;                 //Change it dynamically for testing its taking 1
                _addPResult.patient_concerns = hxConcerns;
                _addPResult.hx_dental = duplicateDental;
                _addPResult.hx_medical_ent = duplicateENT;
                _addPResult.hx_medical_sleep = duplicateSleep;
                _etc.AddToPatientResultQuestions(_addPResult);
                _etc.SaveChanges();
                return string.Empty;
            }


        }


        public static GeneralNutr SavePatientMedicationInfo(string hxMedication, string hxCategories, Guid _pkey)
        {
            GuruETCEntities _etc = new GuruETCEntities();
            long? ParentId = _etc.PatientProfiles.Where(d => d.UserGuid == _pkey).Select(d => d.Id).FirstOrDefault();
            long? DoctorId = _etc.PatientProfiles.Where(d => d.UserGuid == _pkey).Select(d => d.DoctorId).FirstOrDefault();
            GeneralNutr _gNutri = new GeneralNutr();
            PatientResultQuestion _Presult = _etc.PatientResultQuestions.Where(p => p.DoctorId == DoctorId && p.PatientId == ParentId).FirstOrDefault();
            if (_Presult != null)
            {
                string[] medAlegy = hxMedication.Split('|');
                string[] catMedAlegy = hxCategories.Split('|');
                string AllMedication = string.Empty;
                string AllAllergy = string.Empty;
                int index = 0;
                foreach (var item in catMedAlegy)
                {
                    if (item == "Medications")
                        AllMedication += medAlegy[index] + "|";
                    else
                        AllAllergy += medAlegy[index] + "|";

                    index++;

                }

                if (AllMedication.Length > 1)
                    AllMedication = AllMedication.Substring(0, AllMedication.Length - 1);

                if (AllAllergy.Length > 1)
                    AllAllergy = AllAllergy.Substring(0, AllAllergy.Length - 1);

                _Presult.hx_medications = AllMedication;
                _Presult.hx_allergies = AllAllergy;
                _etc.SaveChanges();
                string Result = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}", _Presult.Weight, _Presult.Height, _Presult.feet, _Presult.Packs, _Presult.Cans, _Presult.FormerSmoker, _Presult.FormerChewer, _Presult.dietrating);
                _gNutri.GHealth = _Presult.hx_general;
                _gNutri.Nutri = _Presult.hx_nutrition_activity;
                _gNutri.Tbc = _Presult.hx_tobacco_alcohol;
                _gNutri.Vital = _Presult.hx_vitals_labs;
                _gNutri.Extra = Result;

            }
            return _gNutri;
        }

        public static medical SavePatientGeneralLifestyle(string hxGeneral, string hxutrition, string hxVitalsLabs, string hxTobacco, string wet, string hgfeet, string hginc, string fsmoke, string fchewer, string drate, Guid _pkey)
        {

            GuruETCEntities _etc = new GuruETCEntities();
            long? ParentId = _etc.PatientProfiles.Where(d => d.UserGuid == _pkey).Select(d => d.Id).FirstOrDefault();
            long? DoctorId = _etc.PatientProfiles.Where(d => d.UserGuid == _pkey).Select(d => d.DoctorId).FirstOrDefault();
            medical _med = new medical();
            PatientResultQuestion _Presult = _etc.PatientResultQuestions.Where(p => p.DoctorId == DoctorId && p.PatientId == ParentId).FirstOrDefault();
            if (_Presult != null)
            {
                _Presult.hx_general = hxGeneral;
                _Presult.hx_nutrition_activity = hxutrition;
                _Presult.hx_vitals_labs = hxVitalsLabs;
                _Presult.hx_tobacco_alcohol = hxTobacco;
                if(!string.IsNullOrEmpty(wet))
                _Presult.Weight = double.Parse(wet);

                if (!string.IsNullOrEmpty(hgfeet))
                _Presult.Height = double.Parse(hgfeet);

                if (!string.IsNullOrEmpty(hginc))
                _Presult.feet = double.Parse(hginc);
                _Presult.FormerSmoker = fsmoke;
                _Presult.FormerChewer = fchewer;
                _Presult.dietrating = drate;
                _etc.SaveChanges();

                _med.cardio = _Presult.hx_medical_cardiovascular;
                _med.Cancer = _Presult.hx_medical_cancer;
                _med.DIS = _Presult.hx_medical_other_diseases;
                _med.ENT = _Presult.hx_medical_ent;
                _med.sleep = _Presult.hx_medical_sleep;
                _med.GHealth = _Presult.hx_medical_gender_health;
                _med.Endo = _Presult.hx_medical_endocrine_disorders;

            }

            return _med;
        }

        public static string SaveCardio(string hxCardiovascular, string hxEndocrine, string hxCancer, string hxEnt, string hxSleep, string hxOther, string hxGender, string diab, Guid _pkey)
        {
            GuruETCEntities _etc = new GuruETCEntities();
            long? ParentId = _etc.PatientProfiles.Where(d => d.UserGuid == _pkey).Select(d => d.Id).FirstOrDefault();
            long? DoctorId = _etc.PatientProfiles.Where(d => d.UserGuid == _pkey).Select(d => d.DoctorId).FirstOrDefault();
            PatientResultQuestion _Presult = _etc.PatientResultQuestions.Where(p => p.DoctorId == DoctorId && p.PatientId == ParentId).FirstOrDefault();
            if (_Presult != null)
            {
                _Presult.hx_medical_cardiovascular = hxCardiovascular;
                _Presult.hx_medical_endocrine_disorders = hxEndocrine;
                _Presult.hx_medical_cancer = hxCancer;
                _Presult.hx_medical_ent = hxEnt;
                _Presult.hx_medical_sleep = hxSleep;
                _Presult.hx_medical_other_diseases = hxOther;
                _Presult.hx_medical_gender_health = hxGender;
                _Presult.Diabetes = diab;
                _etc.SaveChanges();
                return _Presult.hx_dental;
            }

            return string.Empty;
        }

        public static string SaveDental(string hxDental, Guid _pkey)
        {
            GuruETCEntities _etc = new GuruETCEntities();
            long? ParentId = _etc.PatientProfiles.Where(d => d.UserGuid == _pkey).Select(d => d.Id).FirstOrDefault();
            long? DoctorId = _etc.PatientProfiles.Where(d => d.UserGuid == _pkey).Select(d => d.DoctorId).FirstOrDefault();
            PatientResultQuestion _Presult = _etc.PatientResultQuestions.Where(p => p.DoctorId == DoctorId && p.PatientId == ParentId).FirstOrDefault();
            if (_Presult != null)
            {
                _Presult.hx_dental = hxDental;
                _etc.SaveChanges();
            }

            return string.Empty;
        }


        public class PatientRecord
        {
            public string Answer { get; set; }
            public List<PMed> meditation { get; set; }
            public List<PSup> Supliment { get; set; }
            public List<PHos> Hospital { get; set; }
        }

        public class PMed
        {
            public long MedId { get; set; }
            public string Title { get; set; }
            public DateTime Date { get; set; }
        }

        public class PSup
        {
            public long SupId { get; set; }
            public string Title { get; set; }
            public DateTime Date { get; set; }
        }

        public class PHos
        {
            public long HosId { get; set; }
            public string Title { get; set; }
            public DateTime Date { get; set; }
        }

        public class GeneralNutr
        {
            public string GHealth { get; set; }
            public string Nutri { get; set; }
            public string Vital { get; set; }
            public string Tbc { get; set; }
            public string Extra { get; set; }
        }

        public class medical
        {
            public string cardio { get; set; }
            public string sleep { get; set; }
            public string ENT { get; set; }
            public string DIS { get; set; }
            public string Cancer { get; set; }
            public string GHealth { get; set; }
            public string Endo { get; set; }
        }

        public class QResult
        {
            public string periodental { get; set; }
            public string diabetic { get; set; }
            public string sleepApnea { get; set; }
            public string tmd { get; set; }
            public string caries { get; set; }
            public string oralCancer { get; set; }
        }


        public static QResult GetPatientFullInfo(Guid _pkey)
        {

            GuruETCEntities _etc = new GuruETCEntities();
            long? ParentId = _etc.PatientProfiles.Where(d => d.UserGuid == _pkey).Select(d => d.Id).FirstOrDefault();
            long? DoctorId = _etc.PatientProfiles.Where(d => d.UserGuid == _pkey).Select(d => d.DoctorId).FirstOrDefault();
            QResult _result = new QResult();
            long? ExamId = _etc.PatientExams.Where(d => d.PatientId == ParentId && d.DoctorId == DoctorId).Max(d => d.ExamId);
            PatientResultQuestion _pResult = _etc.PatientResultQuestions.Where(d => d.ExamId == ExamId).FirstOrDefault();
            if (_pResult != null)
            {
                //Calculation for Periodental - HEALTH HX LINKED QUESTIONS

                DateTime? _pDOB = _etc.PatientProfiles.Where(d => d.Id == _pResult.PatientId).Select(d => d.DOB).FirstOrDefault();
                int Patient_age = (DateTime.Now.Year - _pDOB.Value.Year);
                string[] perioGeneralRAC = { "8.1" };
                string[] perioTobaccoRAC = { "2", "7" };
                string[] perioEndocrineRAC = { "4" };
                string[] perioDentalRAC = { "1.2", "12", "15", "21" };


                string[] hx_general = _pResult.hx_general.Split('|');
                string[] hx_tobacco = _pResult.hx_tobacco_alcohol.Split('|');
                string[] hx_endro = _pResult.hx_medical_endocrine_disorders.Split('|');
                string[] hx_dental = _pResult.hx_dental.Split('|');
                string[] hx_nutrition = _pResult.hx_nutrition_activity.Split('|');
                string[] hx_ent = _pResult.hx_medical_ent.Split('|');
                string[] hx_sleep = _pResult.hx_medical_sleep.Split('|');
                string[] hx_cancer = _pResult.hx_medical_cancer.Split('|');
                string[] hx_other = _pResult.hx_medical_other_diseases.Split('|');

                int perioRAC_Count = hx_general.Intersect(perioGeneralRAC).Count() + hx_tobacco.Intersect(perioTobaccoRAC).Count() + hx_endro.Intersect(perioEndocrineRAC).Count() + hx_dental.Intersect(perioDentalRAC).Count();
                _result.periodental = (27.2 + (27.2 * perioRAC_Count)) + "px";

                //Calculation for DIABETES - HEALTH HX LINKED QUESTIONS

                string[] diabetesGeneralRAC = { "8.3" };
                string[] diabetesVitalsRAC = { "2" };
                string[] diabetesNutritionRAC = { "7" };

                int diabetesRAC_Count = hx_general.Intersect(diabetesGeneralRAC).Count() + hx_tobacco.Intersect(diabetesVitalsRAC).Count() + hx_nutrition.Intersect(diabetesNutritionRAC).Count();

                if (Patient_age > 45)
                    diabetesRAC_Count++;

                _result.diabetic = (49 + (49 * diabetesRAC_Count)) + "px";


                //Calculation for SLEEP - HEALTH HX LINKED QUESTIONS

                string[] sleepEntRAC = { "6" };
                string[] sleepSleepRAC = { "1", "2", "3", "5", "6" };

                int sleepRAC_Count = hx_ent.Intersect(sleepEntRAC).Count() + hx_sleep.Intersect(sleepSleepRAC).Count();

                _result.sleepApnea = (35 + (35 * sleepRAC_Count)) + "px";


                //Calculation for TMD - HEALTH HX LINKED QUESTIONS	 

                string[] tmdEntRAC = { "1", "2", "3", "4", "5", "8" };
                string[] tmdDentalRAC = { "19" };

                int tmdRAC_Count = hx_ent.Intersect(tmdEntRAC).Count() + hx_dental.Intersect(tmdDentalRAC).Count();

                _result.tmd = (30.6 + (30.6 * tmdRAC_Count)) + "px";


                //Calculation for CARIES - HEALTH HX LINKED QUESTIONS	 

                string[] cariesGeneralRAC = { "9.1", "9.2" };
                string[] cariesNutritionRAC = { "4", "5", "13" };
                string[] cariesCancerRAC = { "2" };
                string[] cariesOtherDiseasesRAC = { "9", "10", "24" };
                string[] cariesDentalRAC = { "1.2", "2.2", "3", "7", "13", "19", "20" };

                int cariesRAC_Count = hx_general.Intersect(cariesGeneralRAC).Count() + hx_nutrition.Intersect(cariesNutritionRAC).Count() + hx_cancer.Intersect(cariesCancerRAC).Count() + hx_other.Intersect(cariesOtherDiseasesRAC).Count() + hx_dental.Intersect(cariesDentalRAC).Count();

                _result.caries = (15.3 + (15.3 * cariesRAC_Count)) + "px";

                //Calculation for ORAL CANCER - HEALTH HX LINKED QUESTIONS	 

                string[] oralCancerGeneralRAC = { "8.5" };
                string[] oralCancerNutritionRAC = { "1.2", "1.3" };
                string[] oralCancerTobaccoRAC = { "1.1", "1.2", "4.1", "4.2", "4.3", "4.4", "3" };
                string[] oralCancerCancerRAC = { "1", "3", "4", "5" };
                string[] oralCancerEntRAC = { "11", "12", "14", "15", "16", "17", "19", "20", "18" };

                int oralCancerRAC_Count = hx_general.Intersect(oralCancerGeneralRAC).Count() + hx_nutrition.Intersect(oralCancerNutritionRAC).Count() + hx_tobacco.Intersect(oralCancerTobaccoRAC).Count() + hx_cancer.Intersect(oralCancerCancerRAC).Count() + hx_ent.Intersect(oralCancerEntRAC).Count();

                if (Patient_age > 40)
                {
                    oralCancerRAC_Count++;
                }
                _result.oralCancer = oralCancerRAC_Count.ToString();
            }


            return _result;
        }
    }
}
