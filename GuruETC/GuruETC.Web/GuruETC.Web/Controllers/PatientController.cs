using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GuruETC.Data;
using GuruETC.Web.Models;
using System.Reflection;
using System.Web.Security;
using System.Configuration;
using GuruETC.DB;

namespace GuruETC.Web.Controllers
{
    public class PatientController : Controller
    {
        //
        // GET: /Patient/


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PatientContact()
        {
            return View();
        }

        [HttpPost]
        public JsonResult PatientContact(string pname, string padd1, string padd2, string pphone, string pdob, string pmedhis, string ppathis, string pemail, string pmot)
        {
            string msg = PatientManage.AddPatientInfo(pname, padd1, padd2, pphone, pdob, pmedhis, ppathis, pemail, pmot, new Guid());
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Registration(string id)
        {
            PatientModel _getregmodel = new PatientModel();
            if (!string.IsNullOrEmpty(id))
            {
                Guid getid = new Guid(id);
                Registeruser _getpinfo = PatientManage.patientInfobyregid(getid);
                if (_getpinfo != null)
                {
                    _getregmodel.doctorkey = _getpinfo.DoctorKey;
                    _getregmodel.Email = _getpinfo.Email;
                    _getregmodel.Name = _getpinfo.Name;
                    _getregmodel.PhoneNumber = _getpinfo.PhoneNumber;
                    _getregmodel.DOB = _getpinfo.DOB;
                    _getregmodel.Address1 = _getpinfo.Address1;
                    _getregmodel.Address2 = _getpinfo.Address2;
                }
                else
                {
                    ViewBag.result = "Invitation code has been suspended!";
                }
            }
            else
            {
                return RedirectToAction("Index", "Patient");
            }
            return View(_getregmodel);
        }

        [HttpPost]
        public ActionResult Registration(PatientModel _pmodel)
        {
            
            Guid getuserkey = (Guid)_pmodel.doctorkey;
            if (getuserkey != default(Guid))
            {
                Registeruser _newuser = new Registeruser() { Name = _pmodel.Name, UserName = _pmodel.UserName, Password = _pmodel.Password, Address1 = _pmodel.Address1, Address2 = _pmodel.Address2, DOB = _pmodel.DOB, Email = _pmodel.Email, MedicalHistory = _pmodel.MedicalHistory, PatientHistorical = _pmodel.PatientHistorical, PhoneNumber = _pmodel.PhoneNumber, PersonalMotivator = _pmodel.PersonalMotivator };
                string msg = Registeruser.RegisterPatient(_newuser, getuserkey);
                ViewBag.result = msg;
            }
            else
            {
                ViewBag.result = "Error, invitation code has been suspended!";
            }
            return View(_pmodel);
        }


        public ActionResult Register()
        {
            //if (!string.IsNullOrEmpty(id))
            //{
            //    var patientDetail = PatientManage.IsValidid(id);
            //    ViewBag.Message = id;
            //    PatientModel _pmodel = new PatientModel();
            //    _pmodel.Name = patientDetail.Name;
            //    _pmodel.Email = patientDetail.Email;
            //    _pmodel.Address1 = patientDetail.Address1;
            //    _pmodel.Address2 = patientDetail.Address2;
            //    _pmodel.DOB = patientDetail.DOB.Value.ToShortDateString();
            //    _pmodel.PhoneNumber = patientDetail.PhoneNumber;
            //    _pmodel.MedicalHistory = patientDetail.MedicalHistory;
            //    _pmodel.PatientHistorical = patientDetail.PatientHistorical;
            //    _pmodel.PersonalMotivator = patientDetail.PersonalMotivator;
            //    return View(_pmodel);
            //}
            //else
            //{
            //    return RedirectToAction("PatientContact", "Patient");
            //}
            ViewBag.result = "";
            MembershipUser _getCurrentUser = Membership.GetUser();
            if (_getCurrentUser != null)
            {
                bool IsRole = Roles.IsUserInRole("Doctor");
                if (IsRole)
                {
                    return View();
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

            return RedirectToAction("Index", "Patient");
        }



        [HttpPost]
        public ActionResult Register(PatientModel _pmodel, FormCollection _frm)
        {
            MembershipUser _getCurrentUser = Membership.GetUser();
            Guid getuserkey =(Guid)_getCurrentUser.ProviderUserKey;
          //  Registeruser _newuser = new Registeruser() { Name = _pmodel.Name, UserName = _pmodel.UserName, Password = _pmodel.Password, Address1 = _pmodel.Address1, Address2 = _pmodel.Address2, DOB = _pmodel.DOB, Email = _pmodel.Email, MedicalHistory = _pmodel.MedicalHistory, PatientHistorical = _pmodel.PatientHistorical, PhoneNumber = _pmodel.PhoneNumber, PersonalMotivator = _pmodel.PersonalMotivator };
         //   string msg = Registeruser.RegisterPatient(_newuser, getuserkey);

            string IsexistUser = Membership.GetUserNameByEmail(_pmodel.Email);
            if (string.IsNullOrEmpty(IsexistUser))
            {

                string msg = PatientManage.AddPatientInfo(_pmodel.Name, _pmodel.Address1, _pmodel.Address2, _pmodel.PhoneNumber, _pmodel.DOB, "", "", _pmodel.Email, "", getuserkey);
                if (msg == "Success")
                    ViewBag.result = "An invitation mail sent to the patient!";
                else
                    ViewBag.result = "Error occured!";
            }
            else
            {
                ViewBag.result = "Email Already Exist!";
                
            }
            return View(_pmodel);

        }


        //[HttpPost]
        //public ActionResult Register(PatientModel _pmodel, FormCollection _frm)
        //{
        //    string getid = _frm["hdgetid"].ToString();
        //    Registeruser _newuser = new Registeruser() { Name = _pmodel.Name, UserName = _pmodel.UserName, Password = _pmodel.Password, Address1 = _pmodel.Address1, Address2 = _pmodel.Address2, DOB = _pmodel.DOB, Email = _pmodel.Email, MedicalHistory = _pmodel.MedicalHistory, PatientHistorical = _pmodel.PatientHistorical, PhoneNumber = _pmodel.PhoneNumber, PersonalMotivator = _pmodel.PersonalMotivator };
        //    string msg = Registeruser.RegisterPatient(_newuser, getid);
        //    if(msg=="Success")
        //        return RedirectToAction("RegisterSuccess", "Patient");
        //    else
        //        return View(_pmodel);
        //}

        public ActionResult RegisterSuccess()
        {
            return View();
        }



        [HttpPost]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Patient");
        }

        public ActionResult Login()
        {
            return View();
        }

        


        [HttpPost]
        public JsonResult Login(UserLogin _login)
        {
            string retmsg = "Error";
            string getid = Request.ServerVariables["http_referer"];
            if (!string.IsNullOrEmpty(getid)) { getid = getid.ToLower(); }
            string ReturnUrl = string.Empty;
            string url = ConfigurationManager.AppSettings["SiteUrl"];
            if (getid.Contains("riskanalysis"))
            {
                if (!string.IsNullOrEmpty(getid))
                {
                    string[] getreturnurl = getid.Split('=');
                    ReturnUrl = getreturnurl[1].ToString();
                }
            }
            PatientLogin _ptlogin = new PatientLogin() { UserName = _login.UserName, Password = _login.Password };
            bool IsAuth = PatientLogin.login(_ptlogin);
            
            if (IsAuth)
            {
                string[] GetRole = Roles.GetRolesForUser(_login.UserName);
                FormsAuthentication.SetAuthCookie(_login.UserName, true);
                //if (!string.IsNullOrEmpty(ReturnUrl))
                //    return Redirect(url + "/" + ReturnUrl);
                //else
                //    return RedirectToAction("QuestionPart1", "Question");
                retmsg = "Success";
            }
            else
            {
                retmsg = "Invalid credentials!";
            }

            return Json(retmsg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ForgotPass()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPass(PatientModel model)
        {
            if (!string.IsNullOrEmpty(model.Email))
            {
                string password = Registeruser.GetUserPassword(model.Email);
                if (!string.IsNullOrEmpty(password))
                {
                    PatientManage.SendPassword(model.Email, string.Empty, password);
                    ModelState.AddModelError("EmptyEmail", "Password has been sent to your email address !");
                }
                else
                {
                    ModelState.AddModelError("EmptyEmail", "Invalid email address!");
                }
            }
            else
            {
                ModelState.AddModelError("EmptyEmail", "Invalid email address!");
            }
            return View();
        }


        public ActionResult PatientList()
        {
            MembershipUser _getCurrentUser = Membership.GetUser();
            if (_getCurrentUser != null)
            {
                bool IsRole = Roles.IsUserInRole("Doctor");
                if (IsRole)
                {
                    Guid _getUserkey = (Guid)_getCurrentUser.ProviderUserKey;
                    List<PatientProfile> _profile = PatientManage.GetPatientList(_getUserkey);
                    return View(_profile);
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

            return RedirectToAction("Index", "Patient");
        }


        public ActionResult AssessmentVideos()
        {
            return View();
        }


        public ActionResult Edit(int id)
        {
            MembershipUser _getCurrentUser = Membership.GetUser();
            PatientModel _getmodel = new PatientModel();
            if (_getCurrentUser != null)
            {
                bool IsRole = Roles.IsUserInRole("Doctor");
                if (IsRole)
                {
                    Guid _doctorkey=(Guid)_getCurrentUser.ProviderUserKey;
                    Registeruser _profile = PatientManage.EditUser(id, _doctorkey);
                    if (_profile != null)
                    {
                        _getmodel.UserName = _profile.UserName;
                        _getmodel.Email = _profile.Email;
                        _getmodel.Name = _profile.Name;
                        _getmodel.MedicalHistory = _profile.MedicalHistory;
                        _getmodel.PatientHistorical = _profile.PatientHistorical;
                        _getmodel.PersonalMotivator = _profile.PersonalMotivator;
                        _getmodel.PhoneNumber = _profile.PhoneNumber;
                        _getmodel.DOB = _profile.DOB;
                        _getmodel.Address1 = _profile.Address1;
                        _getmodel.Address2 = _profile.Address2;
                    }
                    
                    return View(_getmodel);
                }
            }
            
            return RedirectToAction("Index", "Patient");    
        }


        [HttpPost]
        public ActionResult Edit(PatientModel _user)
        {
            MembershipUser _getCurrentUser = Membership.GetUser(_user.UserName);
            if (_getCurrentUser.Email != _user.Email)
            {
                _getCurrentUser.Email = _user.Email;
                Membership.UpdateUser(_getCurrentUser);
            }

            Registeruser _updateUser = new Registeruser { Name = _user.Name, DOB = _user.DOB, Address1 = _user.Address1, Address2 = _user.Address2, MedicalHistory = _user.MedicalHistory, PatientHistorical = _user.PatientHistorical, PhoneNumber = _user.PhoneNumber, PersonalMotivator = _user.PersonalMotivator };

            Guid _getkey = (Guid)_getCurrentUser.ProviderUserKey;
            string msg = PatientManage.UpdateUser(_updateUser, _getkey);

            return RedirectToAction("PatientList", "Patient");  
        }


        public ActionResult ShowProfile()
        {
            MembershipUser _getCurrentUser = Membership.GetUser();
            PatientModel _getmodel = new PatientModel();
            if (_getCurrentUser != null)
            {
                bool IsRole = Roles.IsUserInRole("Patient");
                if (IsRole)
                {
                    Guid _getUserkey = (Guid)_getCurrentUser.ProviderUserKey;
                    Registeruser _profile = PatientManage.GetPatientRecord(_getUserkey);
                    if (_profile != null)
                    {
                        _getmodel.Email = _getCurrentUser.Email;
                        _getmodel.UserName = _getCurrentUser.UserName;
                        _getmodel.Name = _profile.Name;
                        _getmodel.MedicalHistory = _profile.MedicalHistory;
                        _getmodel.PatientHistorical = _profile.PatientHistorical;
                        _getmodel.PersonalMotivator = _profile.PersonalMotivator;
                        _getmodel.PhoneNumber = _profile.PhoneNumber;
                        _getmodel.DOB = _profile.DOB;
                        _getmodel.Address1 = _profile.Address1;
                        _getmodel.Address2 = _profile.Address2;
                    }
                    return View(_getmodel);
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

        public ActionResult Profile()
        {
            MembershipUser _getCurrentUser = Membership.GetUser();
            PatientModel _getmodel = new PatientModel();
            if (_getCurrentUser != null)
            {
                bool IsRole = Roles.IsUserInRole("Patient");
                if (IsRole)
                {
                    Guid _getUserkey = (Guid)_getCurrentUser.ProviderUserKey;
                    Registeruser _profile = PatientManage.GetPatientRecord(_getUserkey);
                    if (_profile != null)
                    {
                        _getmodel.Email = _getCurrentUser.Email;
                        _getmodel.UserName = _getCurrentUser.UserName;
                        _getmodel.Name = _profile.Name;
                        _getmodel.MedicalHistory = _profile.MedicalHistory;
                        _getmodel.PatientHistorical = _profile.PatientHistorical;
                        _getmodel.PersonalMotivator = _profile.PersonalMotivator;
                        _getmodel.PhoneNumber = _profile.PhoneNumber;
                        _getmodel.DOB = _profile.DOB;
                        _getmodel.Address1 = _profile.Address1;
                        _getmodel.Address2 = _profile.Address2;
                     }
                    return View(_getmodel);
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

        [HttpPost]
        public ActionResult Profile(PatientModel _user)
        {
            MembershipUser _getCurrentUser = Membership.GetUser();
            if (_getCurrentUser.Email != _user.Email)
            {
                _getCurrentUser.Email = _user.Email;
                Membership.UpdateUser(_getCurrentUser);
            }

            Registeruser _updateUser=new Registeruser{Name=_user.Name, DOB=_user.DOB, Address1=_user.Address1,Address2=_user.Address2, MedicalHistory=_user.MedicalHistory, PatientHistorical=_user.PatientHistorical, PhoneNumber=_user.PhoneNumber, PersonalMotivator=_user.PersonalMotivator };

            Guid _getkey = (Guid)_getCurrentUser.ProviderUserKey;
            string msg = PatientManage.UpdateUser(_updateUser, _getkey);

            return View(_user);
        }

        [HttpPost]
        public JsonResult ChangePassword(string old, string nepass)
        {
            string msg=string.Empty;
            try
            {
                MembershipUser _getCurrentUser = Membership.GetUser();
                if (_getCurrentUser != null)
                {
                    bool result = _getCurrentUser.ChangePassword(old, nepass);
                    if (result)
                        msg = "Password Changed!";
                    else
                        msg = "Error Occured!";
                }
            }
            catch
            {
                msg = "Error Occured!";
            }

            return Json(msg, JsonRequestBehavior.AllowGet);
        }



    }
}
