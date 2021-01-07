 using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace System
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            System.Timers.Timer myTimer = new System.Timers.Timer(1000);
            //TaskAction.SetContent 表示要調用的方法
            myTimer.Elapsed += new System.Timers.ElapsedEventHandler(TaskAction.SetContent);
            myTimer.Enabled = true;
            myTimer.AutoReset = true;
        }
    }

    public static class TaskAction
    {
        private static string content = "";
        
        public static void SetContent(object source, Timers.ElapsedEventArgs e)
        {
            List<string> emails = new List<string>();
            DateTime tomorrow = DateTime.Today.AddDays(1);
            if (DateTime.Now.ToString("HH:mm:ss") == "20:00:00")
            {
                using (System.Models.RoomSystemEntities db = new System.Models.RoomSystemEntities())
                {
                    foreach (var r in from s in db.Reservations where s.Date == tomorrow && !s.Disable select s)
                    {
                        var user = (from u in db.AspNetUsers where u.Id == r.AspNetUserId select u).First();
                        emails.Add(user.Email);
                        foreach (var email in r.BorrowerList.Split(';'))
                        {
                            emails.Add(email);
                        }

                    }
                }
            }
            emails = emails.Distinct().ToList();
            foreach (var email in emails)
            {
                string subject = "會議系統提醒";
                string body = string.Format("{0:yyyy-MM-dd} 有預約會議室，但不用到喔", tomorrow);
                System.Controllers.HomeController.SendEmail(email, "", subject, body);
            }
        }
    }
}