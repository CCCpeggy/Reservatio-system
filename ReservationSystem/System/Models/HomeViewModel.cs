using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace RoomSystem.Models
{
    public class ReserveModel
    {
        public DateTime Date { set; get; }
        public List<System.Models.Rooms> Rooms { set; get; }
        public List<SessionModel> Session { set; get; }
        public List<bool> WantToReserve { set; get; }
        public int? RoomId { set; get; }
        public string BorrowList { set; get; }
    }

    public class SessionModel
    {
        public int SessionNo { set; get; }
        public TimeSpan StartTime { set; get; }
        public TimeSpan EndTime { set; get; }
        public bool Borrowed { set; get; }
        public SessionModel(TimeSpan startTime, TimeSpan endTime, bool borrowed, int sessionNo)
        {
            StartTime = startTime;
            EndTime = endTime;
            Borrowed = borrowed;
            SessionNo = sessionNo;
        }
    }
    public class ReservationModel
    {
        public int Id { get; set; }
        public string BorrowerList { get; set; }
        public bool Disable { get; set; }
        public string AspNetUserId { get; set; }
        public int RoomId { get; set; }
        public int SessionNo { get; set; }
        public System.DateTime Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Email { get; set; }
        //public RoomSystem.Models.AspNetUsers AspNetUsers { get; set; }
        //public RoomSystem.Models.Rooms Rooms { get; set; }

        public ReservationModel(System.Models.Reservations r)
        {
            Id = r.Id;
            BorrowerList = r.BorrowerList;
            Disable = r.Disable;
            AspNetUserId = r.AspNetUserId;
            RoomId = r.RoomId;
            SessionNo = r.SessionNo;
            Date = r.Date;
            //AspNetUsers = new RoomSystem.Models.AspNetUsers(r.AspNetUsers);
            //Rooms = new RoomSystem.Models.Rooms(r.Rooms);

            using (System.Models.RoomSystemEntities db = new System.Models.RoomSystemEntities())
            {
                System.Models.ReservationCenters center = (from s in db.ReservationCenters select s).First();
                // Models.Rooms room = rooms.Where(x => x.Id == roomId).First();
                if (center != null)
                {
                    bool isWeekend = Date.DayOfWeek == DayOfWeek.Sunday || Date.DayOfWeek == DayOfWeek.Saturday;
                    TimeSpan openTime = isWeekend ? center.WeekendOpenTime : center.WeekdaysOpenTime;
                    TimeSpan closeTime = isWeekend ? center.WeekendCloseTime : center.WeekdaysCloseTime;
                    TimeSpan startTime = openTime;
                    TimeSpan interval = new TimeSpan(0, center.TimePerTimePeriod, 0);
                    int i = SessionNo;
                    while (i > 0)
                    {
                        startTime = startTime.Add(interval);
                        i--;
                    }
                    TimeSpan endTime = startTime.Add(interval);
                    endTime = endTime <= closeTime ? endTime : closeTime;
                    StartTime = string.Format("{0:hh\\:mm}", startTime);
                    EndTime = string.Format("{0:hh\\:mm}", endTime);
                    Email = r.AspNetUsers.Email;
                }
            }
        }

    }
    public class RoomModle
    {
        public int Id { get; set; }
        public short MinNumberOfUsers { get; set; }
        public short MaxNumberOfUsers { get; set; }
        public bool Enable { get; set; }

        public RoomModle()
        {
        }
        public RoomModle(System.Models.Rooms r)
        {
            Id = r.Id;
            MinNumberOfUsers = r.MinNumberOfUsers;
            MaxNumberOfUsers = r.MaxNumberOfUsers;
            Enable = r.Enable;
        }
    }
    public class ReservationCenterModel
    {
        public int Id { get; set; }
        public System.TimeSpan WeekdaysOpenTime { get; set; }
        public System.TimeSpan WeekdaysCloseTime { get; set; }
        public System.TimeSpan WeekendOpenTime { get; set; }
        public System.TimeSpan WeekendCloseTime { get; set; }
        public short TimePerTimePeriod { get; set; }
        public ReservationCenterModel(System.Models.ReservationCenters r)
        {
            Id = r.Id;
            WeekdaysOpenTime = r.WeekdaysOpenTime;
            WeekdaysCloseTime = r.WeekdaysCloseTime;
            WeekendOpenTime = r.WeekendOpenTime;
            WeekendCloseTime = r.WeekendCloseTime;
            TimePerTimePeriod = r.TimePerTimePeriod;
        }
    }
    public class AspNetUserModel
    {
        public string Id { get; set; }
        public string Hometown { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public Nullable<System.DateTime> LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string UserName { get; set; }
        public int Authority { get; set; }
        public AspNetUserModel(System.Models.AspNetUsers u)
        {
            Id = u.Id;
            Email = u.Email;
            UserName = u.UserName;
            Authority = u.Authority;
        }

    }
}