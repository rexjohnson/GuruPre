using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GuruETC.Web.Models;
using GuruETC.Data;

namespace GuruETC.Web.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        public ActionResult AddRole()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddRole(RoleDB _role)
        {
            AdminManage.SaveRoleinDB(_role.Name);
            return View();
        }


        public ActionResult AddAdmin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddAdmin(AdminDetail _pmodel)
        {
            AdminManageDetail _newuser = new AdminManageDetail() { UserName = _pmodel.UserName, Password = _pmodel.Password, ADOB = _pmodel.ADOB, Email = _pmodel.Email };
            string msg = AdminManage.AddAdmininDB(_newuser);
            if (msg == "Success")
                return RedirectToAction("Index", "Patient");
            else
                return View(_pmodel);
           

        }

    }
}
