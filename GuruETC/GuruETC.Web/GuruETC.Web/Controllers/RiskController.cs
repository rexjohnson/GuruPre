using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GuruETC.Data;
using System.Text;
//using System.Web.UI.DataVisualization.Charting;
using GuruETC.Web.Models;
using System.Drawing;

using System.Web.Security;
using System.Web.UI.DataVisualization.Charting;
using System.IO;
using System.Web.UI.WebControls;
using PagedList;


namespace GuruETC.Web.Controllers
{
    public class RiskController : Controller
    {
        //
        // GET: /Risk/


        private ActionResult GenerateRiskFactor(string getinfor)
        {
            RiskCalculator _rcal = new RiskCalculator();

            //string[] getrisks = str.Split('_');
            int Diabetes = 0;
            int Periodontal = 0;
            int Sleep = 0;
            foreach (char item in getinfor)
            {
                if (item == 't') { Periodontal++; }
                else if (item == 'e') { Diabetes++; }
                else { Sleep++; }
            }

            _rcal.DiabetisResult = 84 + (84 * Diabetes) + "px";
            _rcal.PeriodontalResult = 46.6 + (46.6 * Periodontal) + "px";
            _rcal.SleepResult = 60 + (60 * Sleep) + "px";
            System.IO.StreamReader myFile = new System.IO.StreamReader(Server.MapPath("~/Files/MedicalDisclaimer.txt"));
            _rcal.MedicalDisclaimer = myFile.ReadToEnd();
            System.IO.StreamReader myresult = new System.IO.StreamReader(Server.MapPath("~/Files/results.txt"));
            _rcal.Results = myresult.ReadToEnd();
            return View(_rcal);
        }




        public ActionResult Add()
        {
            var list = new SelectList(new[]
                                          {
                                              new {ID="1",Name="Periodontal"},
                                              new{ID="2",Name="Diabetes"},
                                              new{ID="3",Name="Sleep Apnea"},
                                          },
                           "ID", "Name", 2);
            ViewData["list"] = list;
            return View();
        }

        [HttpPost]
        public JsonResult Add(string title, string desc, string type)
        {
            string msg = RiskManage.AddRisk(title, desc, type);
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult NewAdd()
        {
            var list = new SelectList(new[]
                                          {
                                              new {ID="1",Name="Periodontal"},
                                              new{ID="2",Name="Diabetes"},
                                              new{ID="3",Name="Sleep Apnea"},
                                          },
                           "ID", "Name", 2);
            ViewData["list"] = list;
            return View();
        }

        [HttpPost]
        public JsonResult NewAdd(string title, string desc, string type)
        {
            string msg = RiskManage.AddRisk(title, desc, type);
            return Json(msg, JsonRequestBehavior.AllowGet);
        }



        public ActionResult ViewRisk(int page = 1)
        {
            var _getallrisks = RiskManage.getAllRisks();

            return View(_getallrisks.ToPagedList(page, 10));
        }

        public ActionResult EditRisk(string id)
        {
            var list = new SelectList(new[]
                                          {
                                              new {ID="1",Name="Periodontal"},
                                              new{ID="2",Name="Diabetes"},
                                              new{ID="3",Name="Sleep Apnea"},
                                          },
                           "ID", "Name", "2");
            ViewData["list"] = list;
            return View(RiskManage.getRiskDetail(int.Parse(id)));
        }

        [HttpPost]
        public JsonResult EditRisk(string id, string title, string desc, string type)
        {
            string msg = RiskManage.EditRisk(id, title, desc, type);
            return Json(msg, JsonRequestBehavior.AllowGet);
            
        }


        public class RiskUpdate
        {
            public string title { get; set; }
            public string type { get; set; }
            public string desc { get; set; }
        }

        [HttpPost]
        public JsonResult DeleteRisk(string id)
        {
            string msg = RiskManage.DeleteRisk(id);
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult Show()
        {
            return View(RiskManage.getAllRisks());
        }

        //[HttpPost]
        //public ActionResult Show(string id)
        //{
        //    string msg = RiskManage.RiskAnalysisResult(id);
        //    return RedirectToAction("RiskAnalysis", "Risk");
        //}


        public ActionResult RiskCalculate(string id)
        {
            string getinfor = RiskManage.getEncryptedKey(id);
            if (!string.IsNullOrEmpty(getinfor))
            {
                return GenerateRiskFactor(getinfor);
            }
            else
            {
                return RedirectToAction("Index", "Patient");
            }
        }



        public ActionResult RiskAnalysis(string id)
        {
            string getid = Request.ServerVariables["http_referer"];
            if (!string.IsNullOrEmpty(getid)) { getid = getid.ToLower(); }
            MembershipUser _getloginUser = Membership.GetUser();
            if (_getloginUser != null)
            {

                if (!string.IsNullOrEmpty(getid) && !getid.Contains("riskanalysis"))
                {
                    var data = Convert.FromBase64String(id);
                    string unique = Encoding.UTF8.GetString(data);
                    // string getrandom = Guid.NewGuid().ToString().Substring(0, 8);
                    RiskManage.SaveEncrypted(unique, id, _getloginUser.UserName);
                    RiskManage.SendEmail(unique, _getloginUser.Email);
                    return GenerateRiskFactor(unique);
                }

                string getdata = RiskManage.GetEncrypted(id, _getloginUser.UserName);
                if (!string.IsNullOrEmpty(getdata))
                {
                    return GenerateRiskFactor(id);
                }
                return View();

            }
            else
            {
                return RedirectToAction("GetLogin", "Risk", new { ReturnUrl = "Risk/RiskAnalysis/" + id });
               // return View();
            }
        }

        public ActionResult GetLogin(string id)
        {
            return View();
        }


        public ActionResult MyChart()
        {
            return PartialView();
        }


    }



}
