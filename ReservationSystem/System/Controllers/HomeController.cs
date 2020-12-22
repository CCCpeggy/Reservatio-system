using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

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
            if (ASPNetUserID == null)
            {
                ViewBag.Authority = -1;
            }
            else
            {
                using (System.Models.RoomSystemEntities db = new System.Models.RoomSystemEntities())
                {
                    System.Models.AspNetUsers user = (from s in db.AspNetUsers where s.Id == ASPNetUserID select s).First();
                    ViewBag.Authority = user.Authority;
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
                        List<int> borrowedSessionList = (from s in db.Reservations where s.RoomId == roomId && s.Date == date select s.SessionNo).ToList();
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
            string ASPNetUserID = User.Identity.GetUserId();
            using (Models.RoomSystemEntities db = new Models.RoomSystemEntities())
            {
                //var loginInfo = await Net.AuthenticationManager.GetExternalLoginInfoAsync();
                //loginInfo.Email
                int i = 0;
                foreach (var wantToReserve in reserveModel.WantToReserve)
                {
                    if (wantToReserve && reserveModel.RoomId != null)
                    {
                        Models.Reservations reservation = new Models.Reservations();
                        reservation.BorrowerList = "";
                        reservation.Disable = false;
                        reservation.AspNetUserId = ASPNetUserID;
                        reservation.RoomId = reserveModel.RoomId.Value;
                        reservation.SessionNo = i;
                        reservation.Date = reserveModel.Date;
                        reservation.BorrowerList = reserveModel.BorrowList;
                        db.Reservations.Add(reservation);
                    }
                    i++;
                }
                db.SaveChanges();
            }
            return Reserve(reserveModel.RoomId.Value, string.Format("{0:yyyy-MM-dd}", reserveModel.Date));
        }

        public ActionResult Record(int? roomId, string Date)
        {
            string ASPNetUserID = User.Identity.GetUserId();
            ViewBag.Date = Date;
            using (System.Models.RoomSystemEntities db = new System.Models.RoomSystemEntities())
            {
                System.Models.AspNetUsers user = (from s in db.AspNetUsers where s.Id == ASPNetUserID select s).First();
                List<System.Models.Reservations> reservations;
                if (user.Authority > 0) {
                    reservations = (from s in db.Reservations select s).ToList();
                }
                else
                {
                    reservations = (from s in db.Reservations where s.AspNetUserId == ASPNetUserID select s).ToList();
                }
                if (Date != null)
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
        public ActionResult UsersEdit(string Authority, string Id)
        {
            using (System.Models.RoomSystemEntities db = new System.Models.RoomSystemEntities())
            {
                string ASPNetUserID = User.Identity.GetUserId();
                System.Models.AspNetUsers user = (from s in db.AspNetUsers where s.Id == ASPNetUserID select s).First();
                if (user.Authority == 0) return View();
                System.Models.AspNetUsers saveUser = (from s in db.AspNetUsers where s.Id == Id select s).First();
                saveUser.Authority = int.Parse(Authority);
                db.SaveChanges();
                return RedirectToAction("UsersSetting", "Home");
            }
        }
    }
}
