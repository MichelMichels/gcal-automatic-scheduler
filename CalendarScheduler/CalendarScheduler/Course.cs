using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
