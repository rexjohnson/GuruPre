using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using GuruETC.DB;

namespace GuruETC.Data
{
    public class AdminManage
    {

        public static void SaveRoleinDB(string RoleName)
        {
            if (!Roles.RoleExists(RoleName))
            {
                Roles.CreateRole(RoleName);
            }
        }


      

        public static string AddAdmininDB(AdminManageDetail _newuser)
        {
            GuruETC.DB.GuruETCEntities _etc = new DB.GuruETCEntities();
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
                        Roles.AddUserToRole(_getUser.UserName, "Admin");
                        AdminProfile _prof = new AdminProfile() { UserGuid = (Guid)_getUser.ProviderUserKey, DOB = DateTime.Parse(_newuser.ADOB), Name = _newuser.UserName};
                        _etc.AddToAdminProfiles(_prof);
                        _etc.SaveChanges();
                        msg = "Success";

                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return msg;
        }
    }



    public class AdminManageDetail
    {
       
        public string UserName { get; set; }

      
        public string Password { get; set; }

        
        public string ADOB { get; set; }

        public string Email { get; set; }
    }



}
