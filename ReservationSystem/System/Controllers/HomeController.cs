using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Net.Mail;

namespace System.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult AuthorityLayout()
        {
            string ASPNetUserID = User.Identity.GetUserId();
            ViewBag.Authority = 2;
            if (ASPNetUserID == null)
            {
                ViewBag.Authority = -1;
            }
            else
            {
                using (System.Models.RoomSystemEntities db = new System.Models.RoomSystemEntities())
                {
                    System.Models.AspNetUsers user = (from s in db.AspNetUsers where s.Id == ASPNetUserID select s).FirstOrDefault();
                    ViewBag.Authority = user.Id == ASPNetUserID ? user.Authority : -1;
                }
            }
            return PartialView();
        }

        public ActionResult Reserve(int? roomId, string Date)
        {
            RoomSystem.Models.ReserveModel reserveModel = new RoomSystem.Models.ReserveModel();
            List<Models.Rooms> rooms = new List<Models.Rooms>();

            using (Models.RoomSystemEntities db = new Models.RoomSystemEntities())
            {
                rooms = (from s in db.Rooms where s.Enable select s).ToList();
            }

            DateTime lastDate = DateTime.Today.AddDays(1);
            DateTime date = lastDate;
            if (Date != null && Date.Length > 0)
            {
                date = DateTime.ParseExact(Date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                //date = date.AddDays(AddDate);
                if (date < lastDate) date = lastDate;
            }
            List<RoomSystem.Models.SessionModel> sessions = new List<RoomSystem.Models.SessionModel>();

            if (roomId != null)
            {
                using (Models.RoomSystemEntities db = new Models.RoomSystemEntities())
                {
                    Models.ReservationCenters center = (from s in db.ReservationCenters select s).First();
                    // Models.Rooms room = rooms.Where(x => x.Id == roomId).First();
                    if (center != null)
                    {
                        bool isWeekend = date.DayOfWeek == DayOfWeek.Sunday || date.DayOfWeek == DayOfWeek.Saturday;
                        TimeSpan openTime = isWeekend ? center.WeekendOpenTime : center.WeekdaysOpenTime;
                        TimeSpan closeTime = isWeekend ? center.WeekendCloseTime : center.WeekdaysCloseTime;
                        TimeSpan interval = new TimeSpan(0, center.TimePerTimePeriod, 0);
                        List<int> borrowedSessionList = (from s in db.Reservations where s.RoomId == roomId && s.Date == date && !s.Disable select s.SessionNo).ToList();
                        int i = 0;
                        for (TimeSpan startTime = openTime, endTime = startTime.Add(interval); startTime < closeTime; startTime = endTime, endTime = startTime.Add(interval))
                        {
                            sessions.Add(new RoomSystem.Models.SessionModel(startTime, endTime <= closeTime ? endTime : closeTime, borrowedSessionList.Contains(i), i));
                            i++;
                        }
                    }
                }
            }
            reserveModel.Date = date;
            reserveModel.Rooms = rooms;
            reserveModel.WantToReserve = new bool[sessions.Count].ToList();
            reserveModel.Session = sessions;
            reserveModel.RoomId = roomId;
            return View(reserveModel);
        }

        [HttpPost]
        public ActionResult Reserve(RoomSystem.Models.ReserveModel reserveModel)
        {
            string date = string.Format("{0:yyyy-MM-dd}", reserveModel.Date);
            List<string> borrower = new List<string>();
            if (reserveModel.BorrowList != null && reserveModel.BorrowList != "")
            {
                borrower = reserveModel.BorrowList.Split(';').Distinct().ToList();
                foreach (var email in borrower)
                {
                    if (!IsValidEmail(email))
                    {
                        return Reserve(reserveModel.RoomId, date);
                    }
                }
            }

            string ASPNetUserID = User.Identity.GetUserId();
            using (Models.RoomSystemEntities db = new Models.RoomSystemEntities())
            {
                //var loginInfo = await Net.AuthenticationManager.GetExternalLoginInfoAsync();
                //loginInfo.Email
                if (reserveModel.RoomId == null) return Reserve(reserveModel.RoomId.Value, string.Format("{0:yyyy-MM-dd}", reserveModel.Date));

                Models.Rooms room = (from s in db.Rooms where reserveModel.RoomId.Value == s.Id select s).FirstOrDefault();
                if (room.Id != reserveModel.RoomId.Value || !(borrower.Count + 1 >= room.MinNumberOfUsers && borrower.Count < room.MaxNumberOfUsers))
                    return Reserve(reserveModel.RoomId, date);
                int i = 0;
                var Reservations = (from s in db.Reservations where s.RoomId == reserveModel.RoomId && s.Date == reserveModel.Date && !s.Disable select s.SessionNo).ToList();
                foreach (var wantToReserve in reserveModel.WantToReserve)
                {
                    if (wantToReserve && !Reservations.Contains(i))
                    {
                        Models.Reservations reservation = new Models.Reservations();
                        reservation.BorrowerList = "";
                        reservation.Disable = false;
                        reservation.AspNetUserId = ASPNetUserID;
                        reservation.RoomId = reserveModel.RoomId.Value;
                        reservation.SessionNo = i;
                        reservation.Date = reserveModel.Date;
                        reservation.BorrowerList = string.Join(";", borrower);
                        db.Reservations.Add(reservation);
                    }
                    i++;
                }
                db.SaveChanges();

                System.Models.AspNetUsers user = (from s in db.AspNetUsers where s.Id == ASPNetUserID select s).First();

                string subject = "已向軟工作業會議預約系統預約會議室";
                string body = string.Format("{0}:\n您已預約 r {1} 會議室，於 {2:yyyy-MM-dd}，但這是軟工作業，所以沒有會議室可以用喔", user.UserName, room.Id, reserveModel.Date);
                SendEmail(user.Email, user.UserName, subject, body);
                
            }
            return Reserve(reserveModel.RoomId.Value, string.Format("{0:yyyy-MM-dd}", reserveModel.Date));
        }

        public ActionResult Record(int? roomId, string Date, string SearchEmail)
        {
            string ASPNetUserID = User.Identity.GetUserId();
            ViewBag.Date = Date;
            ViewBag.SearchEmail = SearchEmail;
            using (System.Models.RoomSystemEntities db = new System.Models.RoomSystemEntities())
            {
                System.Models.AspNetUsers user = (from s in db.AspNetUsers where s.Id == ASPNetUserID select s).First();
                List<System.Models.Reservations> reservations;
                ViewBag.Authority = user.Authority;
                if (user.Authority > 0) {
                    reservations = (from s in db.Reservations where !s.Disable select s).ToList();
                    if (SearchEmail != null)
                    {
                        reservations = reservations.Where(x => x.BorrowerList.Split(';').Where(y => y.Contains(SearchEmail)).Any() || db.AspNetUsers.Where(w => w.Id == x.AspNetUserId).First().Email.Contains(SearchEmail)).ToList();
                    }
                }
                else
                {
                    reservations = (from s in db.Reservations where s.AspNetUserId == ASPNetUserID && !s.Disable select s).ToList();
                }
                if (Date != null && Date != "")
                {
                    DateTime date = DateTime.ParseExact(Date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                    reservations = reservations.Where(x => x.Date == date).ToList();
                }
                if (roomId != null)
                {
                    reservations = reservations.Where(x => x.RoomId == roomId).ToList();
                }
                return View(reservations.Select(x => new RoomSystem.Models.ReservationModel(x)).ToList());
            }
        }

        public ActionResult Cancel(int id)
        {
            using (System.Models.RoomSystemEntities db = new System.Models.RoomSystemEntities())
            {
                System.Models.Reservations reservation = (from s in db.Reservations where s.Id == id select s).First();
                reservation.Disable = true;
                db.SaveChanges();
            }
            return RedirectToAction("Record", "Home");
        }

        public ActionResult RoomSetting()
        {
            using (System.Models.RoomSystemEntities db = new System.Models.RoomSystemEntities())
            {
                string ASPNetUserID = User.Identity.GetUserId();
                System.Models.AspNetUsers user = (from s in db.AspNetUsers where s.Id == ASPNetUserID select s).First();
                if (user.Authority == 0) return View();
                System.Models.ReservationCenters center = (from s in db.ReservationCenters select s).First();
               return View(new RoomSystem.Models.ReservationCenterModel(center));
            }
        }
        [HttpPost]
        public ActionResult RoomSetting(RoomSystem.Models.ReservationCenterModel reserveCenterModel)
        {
            using (System.Models.RoomSystemEntities db = new System.Models.RoomSystemEntities())
            {
                string ASPNetUserID = User.Identity.GetUserId();
                System.Models.AspNetUsers user = (from s in db.AspNetUsers where s.Id == ASPNetUserID select s).First();
                if (user.Authority == 0) return View();
                bool hasReservation = (from s in db.Reservations where s.Date >= DateTime.Today && !s.Disable select s).Any();
                System.Models.ReservationCenters center = (from s in db.ReservationCenters select s).First();
                if (!hasReservation)
                {
                    center.WeekdaysOpenTime = reserveCenterModel.WeekdaysOpenTime;
                    center.WeekdaysCloseTime = reserveCenterModel.WeekdaysCloseTime;
                    center.WeekendOpenTime = reserveCenterModel.WeekendOpenTime;
                    center.WeekendCloseTime = reserveCenterModel.WeekendCloseTime;
                    center.TimePerTimePeriod = reserveCenterModel.TimePerTimePeriod;
                    db.SaveChanges();
                }
                return View(new RoomSystem.Models.ReservationCenterModel(center));
            }
        }
        public ActionResult RoomList()
        {
            using (System.Models.RoomSystemEntities db = new System.Models.RoomSystemEntities())
            {
                string ASPNetUserID = User.Identity.GetUserId();
                System.Models.AspNetUsers user = (from s in db.AspNetUsers where s.Id == ASPNetUserID select s).First();
                if (user.Authority == 0) return PartialView();
                List<System.Models.Rooms> rooms = (from s in db.Rooms where s.Enable select s).ToList();
                return PartialView(rooms.Select(x => new RoomSystem.Models.RoomModle(x)).ToList());
            }
        }
        public ActionResult RoomCreate()
        {
            using (System.Models.RoomSystemEntities db = new System.Models.RoomSystemEntities())
            {
                string ASPNetUserID = User.Identity.GetUserId();
                System.Models.AspNetUsers user = (from s in db.AspNetUsers where s.Id == ASPNetUserID select s).First();
                if (user.Authority == 0) return View();
            }
            return View(new RoomSystem.Models.RoomModle());
        }
        [HttpPost]
        public ActionResult RoomCreate(RoomSystem.Models.RoomModle r)
        {
            if (!(r.MinNumberOfUsers > 0 && r.MaxNumberOfUsers > r.MinNumberOfUsers))
            {
                return View(r);
            }
            using (System.Models.RoomSystemEntities db = new System.Models.RoomSystemEntities())
            {
                string ASPNetUserID = User.Identity.GetUserId();
                System.Models.AspNetUsers user = (from s in db.AspNetUsers where s.Id == ASPNetUserID select s).First();
                if (user.Authority == 0) return View();
                System.Models.Rooms room = new System.Models.Rooms();
                room.MinNumberOfUsers = r.MinNumberOfUsers;
                room.MaxNumberOfUsers = r.MaxNumberOfUsers;
                room.Enable = true;
                db.Rooms.Add(room);
                db.SaveChanges();
            }
            return RedirectToAction("RoomSetting", "Home");
        }
        public ActionResult RoomEdit(int id)
        {
            using (System.Models.RoomSystemEntities db = new System.Models.RoomSystemEntities())
            {
                string ASPNetUserID = User.Identity.GetUserId();
                System.Models.AspNetUsers user = (from s in db.AspNetUsers where s.Id == ASPNetUserID select s).First();
                if (user.Authority == 0) return View();
                System.Models.Rooms room = (from s in db.Rooms where s.Id == id select s).First();
                if (room != null)
                    return View(new RoomSystem.Models.RoomModle(room));
            }
            return RedirectToAction("RoomSetting", "Home");
        }
        [HttpPost]
        public ActionResult RoomEdit(RoomSystem.Models.RoomModle r)
        {
            if (!(r.MinNumberOfUsers > 0 && r.MaxNumberOfUsers > r.MinNumberOfUsers))
            {
                return View(r);
            }
            using (System.Models.RoomSystemEntities db = new System.Models.RoomSystemEntities())
            {
                string ASPNetUserID = User.Identity.GetUserId();
                System.Models.AspNetUsers user = (from s in db.AspNetUsers where s.Id == ASPNetUserID select s).First();
                if (user.Authority == 0) return View();
                System.Models.Rooms room = (from s in db.Rooms where s.Id == r.Id select s).First();
                room.MinNumberOfUsers = r.MinNumberOfUsers;
                room.MaxNumberOfUsers = r.MaxNumberOfUsers;
                db.SaveChanges();
            }
            return RedirectToAction("RoomSetting", "Home");
        }
        public ActionResult RoomDelete(int id)
        {
            using (System.Models.RoomSystemEntities db = new System.Models.RoomSystemEntities())
            {
                string ASPNetUserID = User.Identity.GetUserId();
                System.Models.AspNetUsers user = (from s in db.AspNetUsers where s.Id == ASPNetUserID select s).First();
                if (user.Authority == 0) return View();
                System.Models.Rooms room = (from s in db.Rooms where s.Id == id select s).First();
                room.Enable = false;
                db.SaveChanges();
            }
            return RedirectToAction("RoomSetting", "Home");
        }

        public ActionResult UsersSetting()
        {
            using (System.Models.RoomSystemEntities db = new System.Models.RoomSystemEntities())
            {
                string ASPNetUserID = User.Identity.GetUserId();
                System.Models.AspNetUsers user = (from s in db.AspNetUsers where s.Id == ASPNetUserID select s).First();
                if (user.Authority == 0) return View();
                ViewBag.Authority = user.Authority;
                List<System.Models.AspNetUsers> users = (from s in db.AspNetUsers select s).ToList();
                return View(users.Select(x => new RoomSystem.Models.AspNetUserModel(x)).ToList());
            }
        }
        public ActionResult UsersEdit(string id)
        {
            using (System.Models.RoomSystemEntities db = new System.Models.RoomSystemEntities())
            {
                string ASPNetUserID = User.Identity.GetUserId();
                System.Models.AspNetUsers user = (from s in db.AspNetUsers where s.Id == ASPNetUserID select s).First();
                if (user.Authority == 0) return View();
                System.Models.AspNetUsers editUser = (from s in db.AspNetUsers where s.Id == id select s).First();
                return View(new RoomSystem.Models.AspNetUserModel(editUser));
            }
        }
        [HttpPost]
        public ActionResult UsersEdit(int Authority, string Id)
        {
            using (System.Models.RoomSystemEntities db = new System.Models.RoomSystemEntities())
            {
                string ASPNetUserID = User.Identity.GetUserId();
                System.Models.AspNetUsers user = (from s in db.AspNetUsers where s.Id == ASPNetUserID select s).First();
                if (user.Authority == 0) return View();
                if (!(Authority >= 0 && Authority < user.Authority))
                {
                    return UsersEdit(Id);
                }
                System.Models.AspNetUsers saveUser = (from s in db.AspNetUsers where s.Id == Id select s).First();
                saveUser.Authority = Authority;
                db.SaveChanges();
                return RedirectToAction("UsersSetting", "Home");
            }
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        public static void SendEmail(string email, string name, string subject, string body)
        {
            var fromAddress = new MailAddress("S0ftwar33ngineering@gmail.com", "軟體工程會議室系統客服");
            var toAddress = new MailAddress(email, name);

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 25,
                EnableSsl = true,
                Credentials = new Net.NetworkCredential("Email", "Password")
            };

            try
            {
                using (var message = new MailMessage(fromAddress, toAddress) { Subject = subject, Body = body })
                {
                    smtp.Send(message);
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
