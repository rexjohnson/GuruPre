using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GuruETC.DB;
using System.Threading;
using System.Net.Mail;
using System.Configuration;

namespace GuruETC.Data
{
    public class RiskManage
    {

        public static string AddRisk(string title, string desc, string type)
        {
            GuruETCEntities _etc;
            string msg = string.Empty;
            try
            {
                using (_etc = new DB.GuruETCEntities())
                {
                    Risk_Assessment _risk = new Risk_Assessment();
                    _risk.Risk = title;
                    _risk.Description = desc;
                    _risk.Risk_Type = type;
                    _etc.AddToRisk_Assessment(_risk);
                    _etc.SaveChanges();
                    return "Success_" + _risk.Id;
                }
               
            }
            catch (Exception ex)
            {
                msg = "Error";
                
            }
            return msg;
        }

        public static List<Risk_Assessment> getAllRisks()
        {
            GuruETCEntities _etc;
            using (_etc = new DB.GuruETCEntities())
            {
                return _etc.Risk_Assessment.ToList();
            }
        }

        public static Risk_Assessment getRiskDetail(int id)
        {
            GuruETCEntities _etc;
            using (_etc = new DB.GuruETCEntities())
            {
                return _etc.Risk_Assessment.Where(d => d.Id == id).FirstOrDefault();
            }
        }
         
        public static string EditRisk(string id, string title, string desc, string type)
        {
            GuruETCEntities _etc;
            string msg = string.Empty;
            int getID = int.Parse(id);
            try
            {
                using (_etc = new DB.GuruETCEntities())
                {
                    Risk_Assessment _risk = _etc.Risk_Assessment.Where(d => d.Id == getID).FirstOrDefault();
                    _risk.Risk = title;
                    _risk.Description = desc;
                    _risk.Risk_Type = type;
                    _etc.SaveChanges();
                }
                msg = "Success";
            }
            catch (Exception ex)
            {
                msg = "Error";
            }
            return msg;
        }

        public static string DeleteRisk(string id)
        {
            GuruETCEntities _etc;
            string msg = string.Empty;
            int getID = int.Parse(id);
            try
            {
                using (_etc = new DB.GuruETCEntities())
                {
                    Risk_Assessment _risk = _etc.Risk_Assessment.Where(d => d.Id == getID).FirstOrDefault();
                    if (_risk != null)
                    {
                        _etc.DeleteObject(_risk);
                        _etc.SaveChanges();
                    }

                }
                msg = "Success";
            }
            catch (Exception ex)
            {
                msg = "Error";
            }
            return msg;
        }



        public static void SaveEncrypted(string unique, string id, string username)
        {
            try
            {
                GuruETCEntities _etc;

                using (_etc = new DB.GuruETCEntities())
                {
                    bool _Isexist = _etc.EncryptedDatas.Where(d => d.metadata == id && d.username == username).Count() > 0;
                    if (!_Isexist)
                    {
                        EncryptedData _risk = new EncryptedData();
                        _risk.encryptedvalues = id;
                        _risk.metadata = unique;
                        _risk.username = username;
                        _etc.AddToEncryptedDatas(_risk);
                        _etc.SaveChanges();
                    }

                }
            }
            catch
            {

            }
        }


        [STAThread]
        private static void bitMethod(string getrandom, string Email)
        {
            try
            {
                string senderDisplayName = string.Empty;
                SmtpClient client = new SmtpClient();
                using (MailMessage m = new MailMessage())
                {
                    string url =ConfigurationManager.AppSettings["SiteUrl"]+"/Risk/RiskAnalysis/" + getrandom;
                    string fromDisplayName = string.IsNullOrEmpty(senderDisplayName) ? AppSettings.GetServiceEmailName() : senderDisplayName;
                    m.From = new MailAddress(AppSettings.GetServiceEmail(), fromDisplayName);
                    m.To.Add(new MailAddress(Email));
                    m.Subject = "Patient Info";
                    m.Body = "Dear " + Email + ",<br /> Check your Risk Factors.. <br /> click on the link <a href='" + url + "'>RiskCalculator</a>";
                    m.IsBodyHtml = true;
                    client.Send(m);
                }
            }
            catch { }

        }

        public static string getEncryptedKey(string id)
        {

            GuruETCEntities _etc;
            using (_etc = new DB.GuruETCEntities())
            {
                return _etc.EncryptedDatas.Where(d => d.metadata == id).Select(d => d.encryptedvalues).FirstOrDefault();
            }

        }

        public static void SendEmail(string id, string p)
        {
            ThreadStart starterimg = () => bitMethod(id, p);
            Thread threadimg = new Thread(starterimg);
            threadimg.ApartmentState = ApartmentState.STA;
            threadimg.Start();
        }

        public static string GetEncrypted(string id, string p)
        {
            GuruETCEntities _etc;
            using (_etc = new DB.GuruETCEntities())
            {
                return _etc.EncryptedDatas.Where(d => d.metadata == id && d.username == p).Select(d => d.metadata).FirstOrDefault();
            }
        }
    }


  



}
