using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GuruETC.Data;
using GuruETC.Web.Models;
using System.IO;
using Newtonsoft.Json;
using System.Web.Security;

namespace GuruETC.Web.Controllers
{
    public class QuestionController : Controller
    {
        QuestionPart _Qmodel = new QuestionPart();

        //
        // GET: /Question/

        public ActionResult QuestionPart1(string UserName, string Password)
        {
            MembershipUser _getCurrentUser=null;
            if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
            {
                bool isauth = Membership.ValidateUser(UserName, Password);
                if (isauth)
                {
                    _getCurrentUser = Membership.GetUser(UserName);
                    FormsAuthentication.SetAuthCookie(_getCurrentUser.UserName, true);
                    return RedirectToAction("QuestionPart1", "Question");
                }
            }
            else
            {
               _getCurrentUser = Membership.GetUser();
            }

           
            if (_getCurrentUser != null)
            {
                string[] getRoles = Roles.GetRolesForUser(_getCurrentUser.UserName);
                if (getRoles.Contains("Patient"))
                {

                    var QuestionList = QuestionManage.GetPartFirstList();
                    List<QuestionProp> _quesProp2 = new List<QuestionProp>();
                    List<QuestionProp> _quesProp3 = new List<QuestionProp>();
                    List<QuestionProp> _quesProp4 = new List<QuestionProp>();
                    List<QuestionProp> _quesProp5 = new List<QuestionProp>();
                    List<QuestionProp> _quesProp6 = new List<QuestionProp>();
                    List<QuestionProp> _quesProp7 = new List<QuestionProp>();
                    //  QuestionSet(QuestionList);
                    string[] catName = { "Focus and Overall Objective", "Gum Disease", "Dentures and Implants", "Cosmetics", "Jaws / Bite / Orthodontics", "Teeth and Fillings", "Sleep Issues" };
                    string[] catName2 = { "General Health", "Family Disease", "Disabilities", "Nutrition & Lifestyle" };
                    string[] catName3 = { "Cardiovascular", "Sleep", "ENT", "Other Disease & Conditions", "Cancer", "Gender Health", "Endocrine Disorders" };
                    List<QuestionProp> _quesProp1 = (from item in QuestionList
                                                     where (catName.Contains(item.Category.CategoryName))
                                                     select new QuestionProp()
                                                     {
                                                         CatId = item.CatId,
                                                         QId = item.QId,
                                                         QName = item.QuestionName,
                                                         CName = item.Category.CategoryName,
                                                         QValue = item.Value,
                                                         QClass = item.Qclass
                                                     }).ToList();


                    _quesProp2 = (from item in QuestionList
                                  where (item.Category.CategoryName == "Medications" || item.Category.CategoryName == "Allergies")
                                  select new QuestionProp()
                                  {
                                      CatId = item.CatId,
                                      QId = item.QId,
                                      QName = item.QuestionName,
                                      CName = item.Category.CategoryName,
                                      QValue = item.Value,
                                      QClass = item.Qclass
                                  }).ToList();

                    _quesProp3 = (from item in QuestionList
                                  where (catName2.Contains(item.Category.CategoryName))
                                  select new QuestionProp()
                                  {
                                      CatId = item.CatId,
                                      QId = item.QId,
                                      QName = item.QuestionName,
                                      CName = item.Category.CategoryName,
                                      QValue = item.Value,
                                      QClass = item.Qclass
                                  }).ToList();

                    _quesProp4 = (from item in QuestionList
                                  where (item.Category.CategoryName == "Vital Measurements")
                                  select new QuestionProp()
                                  {
                                      CatId = item.CatId,
                                      QId = item.QId,
                                      QName = item.QuestionName,
                                      CName = item.Category.CategoryName,
                                      QValue = item.Value,
                                      QClass = item.Qclass
                                  }).ToList();

                    _quesProp5 = (from item in QuestionList
                                  where (item.Category.CategoryName == "Tobacco, Alcohol & Drugs")
                                  select new QuestionProp()
                                  {
                                      CatId = item.CatId,
                                      QId = item.QId,
                                      QName = item.QuestionName,
                                      CName = item.Category.CategoryName,
                                      QValue = item.Value,
                                      QClass = item.Qclass
                                  }).ToList();
                    _quesProp6 = (from item in QuestionList
                                  where (catName3.Contains(item.Category.CategoryName))
                                  select new QuestionProp()
                                  {
                                      CatId = item.CatId,
                                      QId = item.QId,
                                      QName = item.QuestionName,
                                      CName = item.Category.CategoryName,
                                      QValue = item.Value,
                                      QClass = item.Qclass
                                  }).ToList();
                    _quesProp7 = (from item in QuestionList
                                  where (item.Category.CategoryName == "Dental Health")
                                  select new QuestionProp()
                                  {
                                      CatId = item.CatId,
                                      QId = item.QId,
                                      QName = item.QuestionName,
                                      CName = item.Category.CategoryName,
                                      QValue = item.Value,
                                      QClass = item.Qclass
                                  }).ToList();

                    _Qmodel.QuestionPart1 = _quesProp1.OrderBy(d => d.CatId).ToList();
                    _Qmodel.QuestionPart2 = _quesProp2.OrderBy(d => d.CatId).ToList();
                    _Qmodel.QuestionPart3 = _quesProp3.OrderBy(d => d.CatId).ToList();
                    _Qmodel.QuestionPart4 = _quesProp4.OrderBy(d => d.CatId).ToList();
                    _Qmodel.QuestionPart5 = _quesProp5.OrderBy(d => d.CatId).ToList();
                    _Qmodel.QuestionPart6 = _quesProp6.OrderBy(d => d.CatId).ToList();
                    _Qmodel.QuestionPart7 = _quesProp7.OrderBy(d => d.CatId).ToList();
                    return View(_Qmodel);

                    
                }
                else
                {
                    return RedirectToAction("Index", "Patient");
                }
            }
            else
            {
                return RedirectToAction("Index", "Patient");
            }
            
        }

        private void QuestionSet(List<DB.Question> QuestionList)
        {
            List<QuestionProp> _quesProp1 = (from item in QuestionList
                                             where item.CatId == 1
                                             select new QuestionProp()
                                             {
                                                 CatId = item.CatId,
                                                 QId = item.QId,
                                                 QName = item.QuestionName,
                                                 CName = item.Category.CategoryName
                                             }).ToList();

            List<QuestionProp> _quesProp2 = (from item in QuestionList
                                             where item.CatId == 2
                                             select new QuestionProp()
                                             {
                                                 CatId = item.CatId,
                                                 QId = item.QId,
                                                 QName = item.QuestionName,
                                                 CName = item.Category.CategoryName
                                             }).ToList();

            List<QuestionProp> _quesProp3 = (from item in QuestionList
                                             where item.CatId == 3
                                             select new QuestionProp()
                                             {
                                                 CatId = item.CatId,
                                                 QId = item.QId,
                                                 QName = item.QuestionName,
                                                 CName = item.Category.CategoryName
                                             }).ToList();

            List<QuestionProp> _quesProp4 = (from item in QuestionList
                                             where item.CatId == 4
                                             select new QuestionProp()
                                             {
                                                 CatId = item.CatId,
                                                 QId = item.QId,
                                                 QName = item.QuestionName,
                                                 CName = item.Category.CategoryName
                                             }).ToList();

            List<QuestionProp> _quesProp5 = (from item in QuestionList
                                             where item.CatId == 5
                                             select new QuestionProp()
                                             {
                                                 CatId = item.CatId,
                                                 QId = item.QId,
                                                 QName = item.QuestionName,
                                                 CName = item.Category.CategoryName
                                             }).ToList();

            List<QuestionProp> _quesProp6 = (from item in QuestionList
                                             where item.CatId == 6
                                             select new QuestionProp()
                                             {
                                                 CatId = item.CatId,
                                                 QId = item.QId,
                                                 QName = item.QuestionName,
                                                 CName = item.Category.CategoryName
                                             }).ToList();

            List<QuestionProp> _quesProp7 = (from item in QuestionList
                                             where item.CatId == 7
                                             select new QuestionProp()
                                             {
                                                 CatId = item.CatId,
                                                 QId = item.QId,
                                                 QName = item.QuestionName,
                                                 CName = item.Category.CategoryName
                                             }).ToList();




            _Qmodel.QuestionPart1 = _quesProp1;
            _Qmodel.QuestionPart2 = _quesProp2;
            _Qmodel.QuestionPart3 = _quesProp3;
            _Qmodel.QuestionPart4 = _quesProp4;
            _Qmodel.QuestionPart5 = _quesProp5;
            _Qmodel.QuestionPart6 = _quesProp6;
            _Qmodel.QuestionPart7 = _quesProp7;
        }


        [HttpPost]
        public JsonResult GetPatientAnswer()
        {
           
            Guid _getpkey = Guid.NewGuid();
            MembershipUser _getCurrentUser = Membership.GetUser();
            if (_getCurrentUser != null)
            {
                bool IsRole = Roles.IsUserInRole("Patient");
                if (IsRole)
                {
                    _getpkey = (Guid)_getCurrentUser.ProviderUserKey;
                }
               
            }

            GuruETC.Data.QuestionManage.PatientRecord _result = QuestionManage.GetPatientAnswer(_getpkey);
            return Json(_result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SavePatientInfo(string key, string title)
        {
            Guid _getpkey = Guid.NewGuid();
            MembershipUser _getCurrentUser = Membership.GetUser();
            if (_getCurrentUser != null)
            {
                bool IsRole = Roles.IsUserInRole("Patient");
                if (IsRole)
                {
                    _getpkey = (Guid)_getCurrentUser.ProviderUserKey;
                }

            }
            string _result = QuestionManage.SavePatientInfo(key, title, _getpkey);
            return Json(_result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeletePatientInfo(string key, string delId)
        {
            string _result = QuestionManage.DeletePatientInfo(key, delId);
            return Json(_result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetPatientVitalInfo()
        {
            Guid _getpkey = Guid.NewGuid();
            MembershipUser _getCurrentUser = Membership.GetUser();
            if (_getCurrentUser != null)
            {
                bool IsRole = Roles.IsUserInRole("Patient");
                if (IsRole)
                {
                    _getpkey = (Guid)_getCurrentUser.ProviderUserKey;
                }

            }
            string _result = QuestionManage.GetPatientVitalInfo(_getpkey);
            return Json(_result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetRefreshInfo()
        {
            string _result = "S";
            return Json(_result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult PostResult(List<GetResultValue> values)
        {
            string _result = "S";
            return Json(_result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SavePatientHealthInfo(string hxConcerns, string duplicateDental, string duplicateENT, string duplicateSleep)
        {
            Guid _getpkey = Guid.NewGuid();
            MembershipUser _getCurrentUser = Membership.GetUser();
            if (_getCurrentUser != null)
            {
                bool IsRole = Roles.IsUserInRole("Patient");
                if (IsRole)
                {
                    _getpkey = (Guid)_getCurrentUser.ProviderUserKey;
                }

            }
            string PatientInfo = QuestionManage.SavePatienthx_Detail(hxConcerns, duplicateDental, duplicateENT, duplicateSleep, _getpkey);
            return Json(PatientInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SavePatientMedicationInfo(string hxMedication, string hxCategories)
        {
            Guid _getpkey = Guid.NewGuid();
            MembershipUser _getCurrentUser = Membership.GetUser();
            if (_getCurrentUser != null)
            {
                bool IsRole = Roles.IsUserInRole("Patient");
                if (IsRole)
                {
                    _getpkey = (Guid)_getCurrentUser.ProviderUserKey;
                }

            }
            GuruETC.Data.QuestionManage.GeneralNutr _result = QuestionManage.SavePatientMedicationInfo(hxMedication, hxCategories, _getpkey);
            return Json(_result, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult SavePatientGeneralLifestyle(string hxGeneral, string hxutrition, string hxVitalsLabs, string hxTobacco, string wet, string hgfeet, string hginc, string fsmoke, string fchewer, string drate)
        {
            Guid _getpkey = Guid.NewGuid();
            MembershipUser _getCurrentUser = Membership.GetUser();
            if (_getCurrentUser != null)
            {
                bool IsRole = Roles.IsUserInRole("Patient");
                if (IsRole)
                {
                    _getpkey = (Guid)_getCurrentUser.ProviderUserKey;
                }

            }
            GuruETC.Data.QuestionManage.medical _result = QuestionManage.SavePatientGeneralLifestyle(hxGeneral, hxutrition, hxVitalsLabs, hxTobacco, wet, hgfeet, hginc, fsmoke, fchewer, drate, _getpkey);
            return Json(_result, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult SaveCardio(string hxCardiovascular, string hxEndocrine, string hxCancer, string hxEnt, string hxSleep, string hxOther, string hxGender, string diab)
        {
            Guid _getpkey = Guid.NewGuid();
            MembershipUser _getCurrentUser = Membership.GetUser();
            if (_getCurrentUser != null)
            {
                bool IsRole = Roles.IsUserInRole("Patient");
                if (IsRole)
                {
                    _getpkey = (Guid)_getCurrentUser.ProviderUserKey;
                }

            }
            string _result = QuestionManage.SaveCardio(hxCardiovascular, hxEndocrine, hxCancer, hxEnt, hxSleep, hxOther, hxGender, diab, _getpkey);
            return Json(_result, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult SaveDental(string hxDental)
        {
            Guid _getpkey = Guid.NewGuid();
            MembershipUser _getCurrentUser = Membership.GetUser();
            if (_getCurrentUser != null)
            {
                bool IsRole = Roles.IsUserInRole("Patient");
                if (IsRole)
                {
                    _getpkey = (Guid)_getCurrentUser.ProviderUserKey;
                }

            }
            string PatientId = QuestionManage.SaveDental(hxDental, _getpkey);
            string _result = "S";
            return Json(_result, JsonRequestBehavior.AllowGet);

        }



        public class GetResultValue
        {
            public string value { get; set; }
            public string CatId { get; set; }
        }

        public ActionResult result()
        {
            Guid _getpkey = Guid.NewGuid();
            MembershipUser _getCurrentUser = Membership.GetUser();
            if (_getCurrentUser != null)
            {
                bool IsRole = Roles.IsUserInRole("Patient");
                if (IsRole)
                {
                    _getpkey = (Guid)_getCurrentUser.ProviderUserKey;
                }

            }
            GuruETC.Data.QuestionManage.QResult _result = QuestionManage.GetPatientFullInfo(_getpkey);
            return View(_result);

        }



    }
}
