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
    class Program
    {
        static void Main(string[] args)
        {
            // Get connection
            APIConnection conn = new APIConnection();
            CalendarService service = conn.service;

            // New CourseList object
            CourseList evenWeekCourses = new CourseList();
            CourseList oddWeekCourses = new CourseList();

            // read courses to list
            oddWeekCourses.ReadCoursesFromFile("C:\\Users\\Michel\\Source\\Repos\\gcal-automatic-scheduler\\CalendarScheduler\\CalendarScheduler\\uurrooster.csv", true);
            evenWeekCourses.ReadCoursesFromFile("C:\\Users\\Michel\\Source\\Repos\\gcal-automatic-scheduler\\CalendarScheduler\\CalendarScheduler\\uurrooster.csv", false);

            // push all courses on the list to the calendar
            //int semester = 0;
            //oddWeekCourses.PushCoursesToGoogleCalendar(service, semester, true);
            //evenWeekCourses.PushCoursesToGoogleCalendar(service, semester, false);

            //semester = 1;
            //oddWeekCourses.Push
            //
            //

            // keep console window open
            Console.WriteLine("Done");
            Console.Read();
        }
    }
}
