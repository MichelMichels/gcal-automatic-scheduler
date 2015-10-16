using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CalendarScheduler
{
    class Course
    {
        public Course(string Name, string Day, TimeSpan StartTime, TimeSpan EndTime)
        {
            this.Name = Name;
            this.Day = Day;
            this.StartTime = StartTime;
            this.EndTime = EndTime;
        }

        public Course(string Name, string Day, TimeSpan StartTime, TimeSpan EndTime, string Location)
        {
            this.Name = Name;
            this.Day = Day;
            this.StartTime = StartTime;
            this.EndTime = EndTime;
            this.Location = Location;

        }

        public Event ToEvent(DateTime date)
        {
            Event newEvent = new Event()
            {
                Summary = Name,
                Start = new EventDateTime()
                {
                    DateTime = new DateTime(date.Year, date.Month, date.Day, StartTime.Hours, StartTime.Minutes, 0),
                    TimeZone = "Europe/Brussels",
                },
                End = new EventDateTime()
                {
                    DateTime = new DateTime(date.Year, date.Month, date.Day, EndTime.Hours, EndTime.Minutes, 0),
                    TimeZone = "Europe/Brussels",
                },

            };

            return newEvent;
        }

        public override string ToString()
        {
            return "Course: " + StartTime.Hours + "u" + StartTime.Minutes + " - " + EndTime.Hours + "u" + EndTime.Minutes + ": "  + Name + " (" + Day + ")";
        }

        public string Name { get; set; }
        public string Day { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Location { get; set; }
    }
}
