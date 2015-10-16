using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Calendar.v3;

namespace CalendarScheduler
{
    class CourseList
    {
        // constructor [FINISHED]
        public CourseList()
        {
            cl = new List<Course>();
        }

        public void AddCourse(Course c)
        {
            cl.Add(c);
        }

        public Course GetCourse(int index)
        {
            return cl[index];
        }

        // read csv file [FINISHED]
        public void ReadCoursesFromFile(string fileName)
        {
            int counter = 0;
            string line;

            StreamReader file = new StreamReader(fileName);
            string day = "ma";

            // while loop checks if there are more lines (line is not empty)
            while((line = file.ReadLine()) != null)
            {
                string[] arr = line.Split(';');
                if (!arr[0].Equals(""))
                {
                    day = arr[0];

                    // debug
                    //Console.WriteLine(day);
                }

                ArrayToCourse(arr, day); 
                counter++;
            }

            file.Close();
        }

        // print courses [FINISHED]
        public void PrintList()
        {
            foreach (var course in cl)
            {
                Console.WriteLine(course);
            }
        }

        // upload courses to google Calendar [WIP]
        public void PushCoursesToGoogleCalendar(CalendarService service)
        {
            int i = 0;
            // iterate over every course
            while(i < cl.Count)
            {
                // get course object from list
                Course currCourse = cl[i];

                // declare startdate
                DateTime startDate = new DateTime();

                // set start dates for lessons on each day
                if (currCourse.Day.Equals("ma")) startDate = new DateTime(2015, 9, 21);
                if (currCourse.Day.Equals("di")) startDate = new DateTime(2015, 9, 22);
                if (currCourse.Day.Equals("wo")) startDate = new DateTime(2015, 9, 23);
                if (currCourse.Day.Equals("do")) startDate = new DateTime(2015, 9, 24);
                if (currCourse.Day.Equals("vr")) startDate = new DateTime(2015, 9, 25);

                // end date is first saturday of christmas holidays
                DateTime endDate = new DateTime(2015, 12, 19);

                // set current date equal to start date
                DateTime currDate = startDate;

                // week of the year
                DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
                System.Globalization.Calendar cal = dfi.Calendar;

                // courses counter
                int counter = 0;

                // conditional statement 
                while (currDate < endDate)
                {
                    if (cal.GetWeekOfYear(currDate, dfi.CalendarWeekRule, dfi.FirstDayOfWeek) == 45 || currDate == new DateTime(2015, 11, 11))
                    {
                        // skip autumn holidays and wapenstilstand
                        currDate = currDate.AddDays(7);
                        continue;
                    }

                    // create event with currDate
                    Event newEvent = currCourse.ToEvent(currDate);
                    
                    // push to google calendar
                    string calendarId = "primary";
                    EventsResource.InsertRequest request = service.Events.Insert(newEvent, calendarId);
                    Event createdEvent = request.Execute();
                    Console.WriteLine("Event created: {0}", createdEvent.HtmlLink);

                    currDate = currDate.AddDays(7);
                    counter++;

                }

                i++;
            }
        }

        // string array to course objects [FINISHED]
        private void ArrayToCourse(string[] arr, string day)
        {
            if (arr[1] != "")
            {
                // timeArr[0] = startTime
                // timeArr[1] = endTime
                string[] timeArr = arr[1].Split('-');

                // get start and end time
                string startTime = timeArr[0];
                string endTime = timeArr[1];

                // split hours and minutes of starttime
                int hourStartTime = int.Parse(startTime.Split('u')[0]);
                int minuteStartTime = int.Parse(startTime.Split('u')[1]);

                // split hours and minutes of endtime
                int hourEndTime = int.Parse(endTime.Split('u')[0]);
                int minuteEndTime = int.Parse(endTime.Split('u')[1]);

                // make TimeSpan objects
                TimeSpan startTimeSpan = new TimeSpan(hourStartTime, minuteStartTime, 0);
                TimeSpan endTimeSpan = new TimeSpan(hourEndTime, minuteEndTime, 0);

                string courseName = arr[2];
                string prof = arr[3];
                string classRoom = null;
                string classGroup = null;

                if (arr[4] != "") classGroup = arr[4];
                if (arr[5] != "") classRoom = arr[5];

                Course c;

                if (classRoom != null)
                {
                    c = new Course(courseName, day, startTimeSpan, endTimeSpan, classRoom);
                } else
                {
                    c = new Course(courseName, day, startTimeSpan, endTimeSpan);
                }

                cl.Add(c);
            }
        }

        private List<Course> cl;
    }
}
