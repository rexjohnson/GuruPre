using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace GuruETC.Data
{
    public static class AppSettings
    {
        public static string GetCnnString()
        {
            return ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        }

        public static string GetServiceEmail()
        {
            return ConfigurationManager.AppSettings["serviceEmail"];
        }

        public static string GetServiceEmailName()
        {
            return ConfigurationManager.AppSettings["serviceEmailName"];
        }

        public static string GetAdminEmail()
        {
            return ConfigurationManager.AppSettings["adminEmail"];
        }

        public static string GetErrorNotificationEmail()
        {
            return ConfigurationManager.AppSettings["errorNotificationEmail"];
        }

        public static bool IsSslEnabled()
        {
            return Convert.ToBoolean(ConfigurationManager.AppSettings["isSslEnabled"]);
        }


    }
}
