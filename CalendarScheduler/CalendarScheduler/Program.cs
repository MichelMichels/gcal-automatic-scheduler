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
            CourseList cl = new CourseList();

            // read courses to list
            cl.ReadCoursesFromFile("C:\\Users\\Michel\\Source\\Repos\\gcal-automatic-scheduler\\CalendarScheduler\\CalendarScheduler\\uurrooster.csv");

            // push all courses on the list to the calendar
            int semester = 0;
            cl.PushCoursesToGoogleCalendar(service, semester);

            // keep console window open
            Console.WriteLine("Done");
            Console.Read();
        }
    }
}
