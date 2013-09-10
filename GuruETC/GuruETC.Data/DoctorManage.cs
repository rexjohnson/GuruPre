using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using GuruETC.DB;

namespace GuruETC.Data
{
    public class DoctorManage
    {
        public string Name { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string PhoneNumber { get; set; }

        public string DDOB { get; set; }

        public string Practice { get; set; }


        public static string RegisterDoctor(DoctorManage _newuser, Guid getuserkey)
        {
            GuruETC.DB.GuruETCEntities _etc = new DB.GuruETCEntities();
            long AdminId = _etc.AdminProfiles.Where(d => d.UserGuid == getuserkey).Select(d => d.Id).FirstOrDefault();
            string msg = string.Empty;
            MembershipCreateStatus sts;
            try
            {
                Membership.CreateUser(_newuser.UserName, _newuser.Password, _newuser.Email, null, null, true, out sts);
                if (sts == MembershipCreateStatus.Success)
                {
                    MembershipUser _getUser = Membership.GetUser(_newuser.UserName);
                    if (_getUser != null)
                    {
                        Roles.AddUserToRole(_getUser.UserName, "Doctor");
                        DoctorProfile _prof = new DoctorProfile() { UserGuid = (Guid)_getUser.ProviderUserKey, PhoneNumber = _newuser.PhoneNumber, Address1 = _newuser.Address1, Address2 = _newuser.Address2, DOB = DateTime.Parse(_newuser.DDOB), Name = _newuser.Name, Practice = _newuser.Practice, AdminId=AdminId };
                        _etc.AddToDoctorProfiles(_prof);
                        _etc.SaveChanges();
                        msg = "Success";

                    }
                }
            }
            catch
            {
                msg = "Error";
            }

            return msg;
        }

        public static List<DoctorProfile> GetDoctorList(Guid pkey)
        {
            List<DoctorProfile> _getUserList = new List<DoctorProfile>();
            GuruETCEntities _etc = new GuruETCEntities();
            long AdminKey = _etc.AdminProfiles.Where(d => d.UserGuid == pkey).Select(d => d.Id).FirstOrDefault();
            if (AdminKey != null)
            {
                _getUserList = _etc.DoctorProfiles.Where(d => d.AdminId == AdminKey).ToList();
            }
            return _getUserList;
        }

        public static DoctorManage GetDoctorRecord(Guid _getUserkey)
        {
            DoctorManage _edituser = new DoctorManage();
            GuruETCEntities _etc = new GuruETCEntities();
            DoctorProfile _profile = _etc.DoctorProfiles.Where(d => d.UserGuid == _getUserkey).FirstOrDefault();
            if (_profile != null)
            {
                _edituser.PhoneNumber = _profile.PhoneNumber;
                _edituser.DDOB = _profile.DOB.Value.ToShortDateString();
                _edituser.Address1 = _profile.Address1;
                _edituser.Address2 = _profile.Address2;
                _edituser.Name = _profile.Name;
                _edituser.Practice = _profile.Practice;
            }
            return _edituser;
        }



        public static string UpdateUser(DoctorManage _updateUser, Guid _pkey)
        {
            string msg = "Error";
            try
            {
                GuruETCEntities _etc = new GuruETCEntities();
                DoctorProfile _profile = _etc.DoctorProfiles.Where(d => d.UserGuid == _pkey).FirstOrDefault();
                if (_profile != null)
                {
                    _profile.Name = _updateUser.Name;
                    _profile.Practice = _updateUser.Practice;
                    _profile.PhoneNumber = _updateUser.PhoneNumber;
                    _profile.Address1 = _updateUser.Address1;
                    _profile.Address2 = _updateUser.Address2;
                    _profile.DOB = Convert.ToDateTime(_updateUser.DDOB);
                    _etc.SaveChanges();
                    msg = "Success";
                }
            }
            catch (Exception ex)
            {
                msg = "Error";
            }
            return msg;
        }




        public static DoctorManage EditUser(int id, Guid _adminkey)
        {
            DoctorManage _edituser = new DoctorManage();
            GuruETCEntities _etc = new GuruETCEntities();
            long? adminKey = _etc.AdminProfiles.Where(d => d.UserGuid == _adminkey).Select(d => d.Id).FirstOrDefault();
            DoctorProfile _profile = _etc.DoctorProfiles.Where(d => d.Id == id).FirstOrDefault();
            MembershipUser _getUserbyName = Membership.GetUser(_profile.UserGuid);
            if (_profile != null)
            {
                if (_profile.AdminId == adminKey)
                {
                    _edituser.Email = _getUserbyName.Email;
                    _edituser.UserName = _getUserbyName.UserName;
                    _edituser.PhoneNumber = _profile.PhoneNumber;
                    _edituser.DDOB = _profile.DOB.Value.ToShortDateString();
                    _edituser.Address1 = _profile.Address1;
                    _edituser.Address2 = _profile.Address2;
                    _edituser.Name = _profile.Name;
                    _edituser.Practice = _profile.Practice;
                }
            }
            return _edituser;
        }
    }
}
