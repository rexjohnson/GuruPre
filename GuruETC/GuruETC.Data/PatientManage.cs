using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GuruETC.DB;
using System.Net.Mail;
using System.Threading;
using System.Web.Security;
using System.Configuration;

namespace GuruETC.Data
{
    public class PatientManage
    {
        public static string AddPatientInfo(string pname, string padd1, string padd2, string pphone, string pdob, string pmedhis, string ppathis, string pemail, string pmot, Guid dguid)
        {
            GuruETC.DB.GuruETCEntities _etc = new DB.GuruETCEntities();
            string msg = string.Empty;
            string senderDisplayName = string.Empty;
            try
            {
               
                Patientinfo _pinfo = new Patientinfo();
                _pinfo.Name = pname;
                _pinfo.Address1 = padd1;
                _pinfo.Address2 = padd2;
                _pinfo.DOB = DateTime.Parse(pdob);
                _pinfo.PhoneNumber = pphone;
                _pinfo.MedicalHistory = pmedhis;
                _pinfo.PatientHistorical = ppathis;
                _pinfo.PersonalMotivator = pmot;
                _pinfo.Email = pemail;
                _pinfo.refby = dguid;
                _etc.AddToPatientinfoes(_pinfo);
                _etc.SaveChanges();

                RegistrationCode _rcode = new RegistrationCode();
                _rcode.createdDate = DateTime.Today;
                _rcode.ExpiryDate = DateTime.Today.AddDays(1);
                _rcode.PatientInfoId = _pinfo.PID;
                Guid Getcode = Guid.NewGuid();
                _rcode.RegCode = Getcode;
                _rcode.IsAuth = true;
                _etc.AddToRegistrationCodes(_rcode);
                _etc.SaveChanges();



                ThreadStart starterimg = () => bitMethod(pemail,pname, senderDisplayName, Getcode);
                Thread threadimg = new Thread(starterimg);
                threadimg.ApartmentState = ApartmentState.STA;
                threadimg.Start();
                msg = "Success";

            }

            catch(Exception ex)
            {
                throw ex;
            }
            return msg;
        }

        [STAThread]
        private static void bitMethod(string pemail,string name, string senderDisplayName, Guid Getcode)
        {
            try
            {

                SmtpClient client = new SmtpClient();
                using (MailMessage m = new MailMessage())
                {
                    string url = ConfigurationManager.AppSettings["SiteUrl"] + "/Patient/Registration/" + Getcode;
                    string fromDisplayName = string.IsNullOrEmpty(senderDisplayName) ? AppSettings.GetServiceEmailName() : senderDisplayName;
                    m.From = new MailAddress(AppSettings.GetServiceEmail(), fromDisplayName);
                    m.To.Add(new MailAddress(pemail));
                    m.Subject = "Patient Info";
                    m.Body = "Dear " + name + ",<br /> We Would like to gather your medical information. firstly set up an account on our website. <br /> click on the link <a href='" + url + "'>Guru-SignIn</a>";
                    m.IsBodyHtml = true;
                    client.Send(m);
                }
            }
            catch { }
        }

        public static Patientinfo IsValidid(string id)
        {
            Patientinfo _getInfo = new Patientinfo();
            GuruETC.DB.GuruETCEntities _etc = new DB.GuruETCEntities();
            try
            {
                Guid getId = Guid.Parse(id);
                RegistrationCode _rcode = _etc.RegistrationCodes.Where(d => d.RegCode == getId && d.ExpiryDate >= DateTime.Today).FirstOrDefault();
                if (_rcode != null)
                {
                    _getInfo = _etc.Patientinfoes.Where(d => d.PID == _rcode.PatientInfoId).FirstOrDefault();
                }
            }
            catch
            {
            }
            return _getInfo;
        }


        public static void SendPassword(string pemail, string senderDisplayName, string password)
        {
            try
            {

                SmtpClient client = new SmtpClient();
                using (MailMessage m = new MailMessage())
                {

                    string fromDisplayName = string.IsNullOrEmpty(senderDisplayName) ? AppSettings.GetServiceEmailName() : senderDisplayName;
                    m.From = new MailAddress(AppSettings.GetServiceEmail(), fromDisplayName);
                    m.To.Add(new MailAddress(pemail));
                    m.Subject = "Guru - Reset Password";
                    m.Body = "Dear " + pemail + ",<br /> You have request for reset your password.<br/> Your Email is:" + pemail + "<br/> Your Password is:" + password + "<br/> Thanks for using guru.";
                    m.IsBodyHtml = true;
                    client.Send(m);
                }
            }
            catch { }
        }



        public static List<PatientProfile> GetPatientList(Guid pkey)
        {
            List<PatientProfile> _getUserList = new List<PatientProfile>();
            GuruETCEntities _etc = new GuruETCEntities();
            long? DoctorKey = _etc.DoctorProfiles.Where(d => d.UserGuid == pkey).Select(d => d.Id).FirstOrDefault();
            if (DoctorKey != 0)
            {
                _getUserList = _etc.PatientProfiles.Where(d => d.DoctorId == DoctorKey).ToList();
            }
            return _getUserList;
        }

        public static Registeruser GetPatientRecord(Guid _getUserkey)
        {
            Registeruser _edituser = new Registeruser();
            GuruETCEntities _etc = new GuruETCEntities();
            PatientProfile _profile = _etc.PatientProfiles.Where(d => d.UserGuid == _getUserkey).FirstOrDefault();
            if (_profile != null)
            {
                _edituser.PhoneNumber = _profile.PhoneNumber;
                _edituser.PersonalMotivator = _profile.PersonalMotivator;
                _edituser.PatientHistorical = _profile.PersonalHistorical;
                _edituser.MedicalHistory = _profile.MedicalHistory;
                _edituser.DOB = _profile.DOB.Value.ToShortDateString();
                _edituser.Address1 = _profile.Address1;
                _edituser.Address2 = _profile.Address2;
                _edituser.Name = _profile.Name;
            }
            return _edituser;
        }

        public static string UpdateUser(Registeruser _updateUser, Guid _pkey)
        {
            string msg = "Error";
            try
            {
                GuruETCEntities _etc = new GuruETCEntities();
                PatientProfile _profile = _etc.PatientProfiles.Where(d => d.UserGuid == _pkey).FirstOrDefault();
                if (_profile != null)
                {
                    _profile.Name = _updateUser.Name;
                    _profile.MedicalHistory = _updateUser.MedicalHistory;
                    _profile.PersonalHistorical = _updateUser.PatientHistorical;
                    _profile.PersonalMotivator = _updateUser.PersonalMotivator;
                    _profile.PhoneNumber = _updateUser.PhoneNumber;
                    _profile.Address1 = _updateUser.Address1;
                    _profile.Address2 = _updateUser.Address2;
                    _profile.DOB = Convert.ToDateTime(_updateUser.DOB);
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

        public static Registeruser EditUser(int id, Guid _doctorkey)
        {
            Registeruser _edituser = new Registeruser();
            GuruETCEntities _etc = new GuruETCEntities();
            long? DoctorKey = _etc.DoctorProfiles.Where(d => d.UserGuid == _doctorkey).Select(d => d.Id).FirstOrDefault();
            PatientProfile _profile = _etc.PatientProfiles.Where(d => d.Id == id).FirstOrDefault();
            MembershipUser _getUserbyName = Membership.GetUser(_profile.UserGuid);
            if (_profile != null)
            {
                if (_profile.DoctorId == DoctorKey)
                {
                    _edituser.Email = _getUserbyName.Email;
                    _edituser.UserName = _getUserbyName.UserName;
                    _edituser.PhoneNumber = _profile.PhoneNumber;
                    _edituser.PersonalMotivator = _profile.PersonalMotivator;
                    _edituser.PatientHistorical = _profile.PersonalHistorical;
                    _edituser.MedicalHistory = _profile.MedicalHistory;
                    _edituser.DOB = _profile.DOB.Value.ToShortDateString();
                    _edituser.Address1 = _profile.Address1;
                    _edituser.Address2 = _profile.Address2;
                    _edituser.Name = _profile.Name;
                }
            }
            return _edituser;
        }

        public static Registeruser patientInfobyregid(Guid getid)
        { 
            GuruETCEntities _etc = new GuruETCEntities();
            Registeruser _edituser = new Registeruser();
            long? PinfoId = _etc.RegistrationCodes.Where(d => d.RegCode == getid).Select(d => d.PatientInfoId).FirstOrDefault();
            if (PinfoId != 0)
            {
                Patientinfo _getpinfo = _etc.Patientinfoes.Where(d => d.PID == PinfoId).FirstOrDefault();
                if (_getpinfo != null)
                {
                    _edituser.Email = _getpinfo.Email;
                    _edituser.DOB = _getpinfo.DOB.Value.ToShortDateString();
                    _edituser.Name = _getpinfo.Name;
                    _edituser.Address1 = _getpinfo.Address1;
                    _edituser.Address2 = _getpinfo.Address2;
                    _edituser.PhoneNumber = _getpinfo.PhoneNumber; ;
                    _edituser.DoctorKey = (Guid)_getpinfo.refby;

                }
                else {
                    return null;
                }
            }
            return _edituser;
        }
    }

    public class PatientLogin
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public static bool login(PatientLogin _ptlogin)
        {
            return Membership.ValidateUser(_ptlogin.UserName, _ptlogin.Password);

        }
    }

    public class Registeruser
    {
        public string Name { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string PhoneNumber { get; set; }

        public string DOB { get; set; }

        public string MedicalHistory { get; set; }

        public string PatientHistorical { get; set; }

        public string PersonalMotivator { get; set; }

        public Guid DoctorKey { get; set; }

        public static string RegisterPatient(Registeruser _newuser, Guid getuserkey)
        {
            GuruETC.DB.GuruETCEntities _etc = new DB.GuruETCEntities();

            long DoctorId = _etc.DoctorProfiles.Where(d => d.UserGuid == getuserkey).Select(d => d.Id).FirstOrDefault();
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
                        Roles.AddUserToRole(_getUser.UserName, "Patient");
                        PatientProfile _prof = new PatientProfile() { UserGuid = (Guid)_getUser.ProviderUserKey, PhoneNumber = _newuser.PhoneNumber, Address1 = _newuser.Address1, Address2 = _newuser.Address2, DOB = DateTime.Parse(_newuser.DOB), Name = _newuser.Name, MedicalHistory = _newuser.MedicalHistory, PersonalHistorical = _newuser.PatientHistorical, PersonalMotivator = _newuser.PersonalMotivator, DoctorId = DoctorId };
                        _etc.AddToPatientProfiles(_prof);
                        _etc.SaveChanges();
                        msg = "A mail sent regarding registration!";

                        ThreadStart starterimg = () => SendMethod(_getUser.Email, "Your Dentist", _newuser.Name,_getUser.UserName, _newuser.Password);
                        Thread threadimg = new Thread(starterimg);
                        threadimg.ApartmentState = ApartmentState.STA;
                        threadimg.Start();

                    }
                }
                else
                {
                    msg = sts.ToString();
                }
            }
            catch
            {
                msg = "Error Occured!";
            }

            return msg;
        }


        [STAThread]
        private static void SendMethod(string pemail, string senderDisplayName, string Name, string UserName, string Password)
        {
            try
            {

                SmtpClient client = new SmtpClient();
                using (MailMessage m = new MailMessage())
                {
                    string url = ConfigurationManager.AppSettings["SiteUrl"] + "/Question/questionpart1?UserName="+UserName+"&Password="+Password;
                    string fromDisplayName = string.IsNullOrEmpty(senderDisplayName) ? AppSettings.GetServiceEmailName() : senderDisplayName;
                    m.From = new MailAddress(AppSettings.GetServiceEmail(), fromDisplayName);
                    m.To.Add(new MailAddress(pemail));
                    m.Subject = "Patient Info";
                    string body = "Dear " + Name + ",<br /><br /> We Would like to gather your medical information. Your Account have set up on our website. <br /> click on the link <a href='" + url + "'>" + url + "</a>" ;
                    body = body + "<br /><br />regards<br />" + senderDisplayName;
                    m.Body = body;
                    m.IsBodyHtml = true;
                    client.Send(m);
                }
            }
            catch { }
        }


        //public static string RegisterPatient(Registeruser _newuser, string getid)
        //{
        //    GuruETC.DB.GuruETCEntities _etc = new DB.GuruETCEntities();
        //    string msg = string.Empty;
        //    MembershipCreateStatus sts;
        //    try
        //    {
        //        Membership.CreateUser(_newuser.UserName, _newuser.Password, _newuser.Email, null, null, true, out sts);
        //        if (sts == MembershipCreateStatus.Success)
        //        {
        //            MembershipUser _getUser = Membership.GetUser(_newuser.UserName);
        //            if (_getUser != null)
        //            {
        //                PatientProfile _prof = new PatientProfile() { UserGuid = (Guid)_getUser.ProviderUserKey, PhoneNumber = _newuser.PhoneNumber, Address1 = _newuser.Address1, Address2 = _newuser.Address2, DOB = DateTime.Parse(_newuser.DOB), Name = _newuser.Name, MedicalHistory = _newuser.MedicalHistory, PersonalHistorical = _newuser.PatientHistorical, PersonalMotivator = _newuser.PersonalMotivator };
        //                _etc.AddToPatientProfiles(_prof);
        //                _etc.SaveChanges();
        //                msg = "Success";
        //                Guid getnewid = Guid.Parse(getid);
        //                List<RegistrationCode> _getsameId = _etc.RegistrationCodes.Where(d => d.RegCode == getnewid).ToList();
        //                foreach (var item in _getsameId)
        //                {
        //                    _etc.DeleteObject(item);
        //                    _etc.SaveChanges();
        //                }

        //            }
        //        }
        //    }
        //    catch
        //    {
        //        msg = "Error";
        //    }

        //    return msg;
        //}


        public static string GetUserPassword(string email)
        {
            string userId = string.Empty;
            string userName = Membership.GetUserNameByEmail(email);
            if (!string.IsNullOrEmpty(userName))
            {
                MembershipUser usr = Membership.GetUser(userName);
                if (usr != null)
                {
                    userId = usr.GetPassword();

                }
            }
            return userId;
        }
    }


}
