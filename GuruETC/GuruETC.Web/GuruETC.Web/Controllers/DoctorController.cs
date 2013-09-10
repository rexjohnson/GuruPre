using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GuruETC.Web.Models;
using System.Web.Security;
using GuruETC.Data;
using GuruETC.DB;

namespace GuruETC.Web.Controllers
{
    public class DoctorController : Controller
    {
        //
        // GET: /Doctor/

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult AddDoctor()
        {
            MembershipUser _getCurrentUser = Membership.GetUser();
            if (_getCurrentUser != null)
            {
                bool IsRole = Roles.IsUserInRole("Admin");
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
        public ActionResult AddDoctor(DoctorModel _pmodel)
        {
            MembershipUser _getCurrentUser = Membership.GetUser();
            Guid getuserkey = (Guid)_getCurrentUser.ProviderUserKey;
            DoctorManage _newuser = new DoctorManage() { Name = _pmodel.Name, UserName = _pmodel.UserName, Password = _pmodel.Password, Address1 = _pmodel.Address1, Address2 = _pmodel.Address2, DDOB = _pmodel.DDOB, Email = _pmodel.Email, PhoneNumber = _pmodel.PhoneNumber, Practice=_pmodel.Practice  };
            string msg = DoctorManage.RegisterDoctor(_newuser, getuserkey);
            if (msg == "Success")
                return RedirectToAction("DoctorList", "Doctor");
            else
                return View(_pmodel);
            return View();
        }

        public ActionResult DoctorList()
        {
            MembershipUser _getCurrentUser = Membership.GetUser();
            if (_getCurrentUser != null)
            {
                bool IsRole = Roles.IsUserInRole("Admin");
                if (IsRole)
                {
                    Guid _getUserkey = (Guid)_getCurrentUser.ProviderUserKey;
                    List<DoctorProfile> _profile = DoctorManage.GetDoctorList(_getUserkey);
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

        public ActionResult Edit(int id)
        {
            MembershipUser _getCurrentUser = Membership.GetUser();
            DoctorModel _getmodel = new DoctorModel();
            if (_getCurrentUser != null)
            {
                bool IsRole = Roles.IsUserInRole("Admin");
                if (IsRole)
                {
                    Guid _adminkey = (Guid)_getCurrentUser.ProviderUserKey;
                    DoctorManage _profile = DoctorManage.EditUser(id, _adminkey);
                    if (_profile != null)
                    {
                        _getmodel.UserName = _profile.UserName;
                        _getmodel.Email = _profile.Email;
                        _getmodel.Name = _profile.Name;
                        _getmodel.PhoneNumber = _profile.PhoneNumber;
                        _getmodel.DDOB = _profile.DDOB;
                        _getmodel.Address1 = _profile.Address1;
                        _getmodel.Address2 = _profile.Address2;
                        _getmodel.Practice = _profile.Practice;
                    }

                    return View(_getmodel);
                }
            }

            return RedirectToAction("Index", "Patient");
        }


        [HttpPost]
        public ActionResult Edit(DoctorModel _user)
        {
            MembershipUser _getCurrentUser = Membership.GetUser(_user.UserName);
            if (_getCurrentUser.Email != _user.Email)
            {
                _getCurrentUser.Email = _user.Email;
                Membership.UpdateUser(_getCurrentUser);
            }

            DoctorManage _updateUser = new DoctorManage { Name = _user.Name, DDOB = _user.DDOB, Address1 = _user.Address1, Address2 = _user.Address2, PhoneNumber = _user.PhoneNumber, Practice=_user.Practice };

            Guid _getkey = (Guid)_getCurrentUser.ProviderUserKey;
            string msg = DoctorManage.UpdateUser(_updateUser, _getkey);

            return RedirectToAction("DoctorList", "Doctor");
        }


        public ActionResult ShowProfile()
        {
            MembershipUser _getCurrentUser = Membership.GetUser();
            DoctorModel _getmodel = new DoctorModel();
            if (_getCurrentUser != null)
            {
                bool IsRole = Roles.IsUserInRole("Doctor");
                if (IsRole)
                {
                    Guid _getUserkey = (Guid)_getCurrentUser.ProviderUserKey;
                    DoctorManage _profile = DoctorManage.GetDoctorRecord(_getUserkey);
                    if (_profile != null)
                    {
                        _getmodel.Email = _getCurrentUser.Email;
                        _getmodel.UserName = _getCurrentUser.UserName;
                        _getmodel.Name = _profile.Name;
                        _getmodel.PhoneNumber = _profile.PhoneNumber;
                        _getmodel.DDOB = _profile.DDOB;
                        _getmodel.Practice = _profile.Practice;
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
            DoctorModel _getmodel = new DoctorModel();
            if (_getCurrentUser != null)
            {
                bool IsRole = Roles.IsUserInRole("Doctor");
                if (IsRole)
                {
                    Guid _getUserkey = (Guid)_getCurrentUser.ProviderUserKey;
                    DoctorManage _profile = DoctorManage.GetDoctorRecord(_getUserkey);
                    if (_profile != null)
                    {
                        _getmodel.Email = _getCurrentUser.Email;
                        _getmodel.UserName = _getCurrentUser.UserName;
                        _getmodel.Name = _profile.Name;
                        _getmodel.PhoneNumber = _profile.PhoneNumber;
                        _getmodel.DDOB = _profile.DDOB;
                        _getmodel.Practice = _profile.Practice;
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
        public ActionResult Profile(DoctorModel _user)
        {
            MembershipUser _getCurrentUser = Membership.GetUser();
            if (_getCurrentUser.Email != _user.Email)
            {
                _getCurrentUser.Email = _user.Email;
                Membership.UpdateUser(_getCurrentUser);
            }

            DoctorManage _updateUser = new DoctorManage { Name = _user.Name, DDOB = _user.DDOB, Address1 = _user.Address1, Address2 = _user.Address2, PhoneNumber = _user.PhoneNumber, Practice=_user.Practice };

            Guid _getkey = (Guid)_getCurrentUser.ProviderUserKey;
            string msg = DoctorManage.UpdateUser(_updateUser, _getkey);

            return View(_user);
        }


        [HttpPost]
        public JsonResult ChangePassword(string old, string nepass)
        {
            string msg = string.Empty;
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
