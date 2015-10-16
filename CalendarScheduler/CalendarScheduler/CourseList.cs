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
        public void PushCoursesToGoogleCalendar(CalendarService service, int semester)
        {
            //
            // METHOD VARIABLES
            //
            int i = 0;
            bool abort = false;
            
            //
            // A) iterate over every course
            //

            while(i < cl.Count & !abort)
            {
                // get course object from list
                Course currCourse = cl[i];

                // declare startdate and enddate
                DateTime startDate = new DateTime();
                DateTime endDate = new DateTime();

                // day array
                string[] dayOfTheWeek = { "ma", "di", "wo", "do", "vr" };

                // check semester
                if (semester == 0)
                {
                    // set start dates for lessons on each day (for loop for shorter code)
                    for (int j = 0; j < 5; j++)
                    {
                        if (currCourse.Day.Equals(dayOfTheWeek[i])) startDate = new DateTime(2015, 9, 21 + i);
                    }

                    // end date is first saturday of christmas holidays
                    endDate = new DateTime(2015, 12, 19);
                } else if (semester == 1)
                {
                    // set start dates for lessons on each day
                    for (int j = 0; j < 5; j++)
                    {
                        if (currCourse.Day.Equals(dayOfTheWeek[i])) startDate = new DateTime(2016, 1, 15 + i);
                    }

                    // end date is first saturday of summer holiday
                    endDate = new DateTime(2016, 5, 28);
                } else
                {
                    Console.WriteLine("Not a valid selection. Abort.");
                    abort = true;
                }
                
                // set current date equal to start date
                DateTime currDate = startDate;

                // week of the year
                DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
                System.Globalization.Calendar cal = dfi.Calendar;

                // courses counter
                int counter = 0;

                // check if current date doesn't go over the end date + abort boolean 
                while (currDate < endDate && !abort)
                {
                    //
                    // 1) Check holidays and skip them by adding 7 days (for each semester)
                    //

                    if (semester == 0)
                    {
                        if (cal.GetWeekOfYear(currDate, dfi.CalendarWeekRule, dfi.FirstDayOfWeek) == 45 || currDate == new DateTime(2015, 11, 11))
                        {
                            // skip autumn holidays and wapenstilstand
                            currDate = currDate.AddDays(7);
                            continue;
                        }
                    } else if (semester == 1)
                    {
                        if (// winter holiday
                            cal.GetWeekOfYear(currDate, dfi.CalendarWeekRule, dfi.FirstDayOfWeek) == 5 
                            // spring break
                            || cal.GetWeekOfYear(currDate, dfi.CalendarWeekRule, dfi.FirstDayOfWeek) == 6
                            // easter holidays
                            || cal.GetWeekOfYear(currDate, dfi.CalendarWeekRule, dfi.FirstDayOfWeek) == 13
                            || cal.GetWeekOfYear(currDate, dfi.CalendarWeekRule, dfi.FirstDayOfWeek) == 14
                            // OLH Hemelvaart
                            || currDate == new DateTime(2016, 5, 5)
                            // pinkstermaandag
                            || currDate == new DateTime(2016, 5, 16)
                            )
                        {
                            // skip holidays and free days
                            currDate = currDate.AddDays(7);
                            continue;
                        }
                    } else
                    {
                        Console.WriteLine("Bad semester number. Abort.");
                        abort = true;
                    }

                    //
                    // 2) Create event with currDate and 'ToEvent' method from Course.cs
                    //

                    Event newEvent = currCourse.ToEvent(currDate);
                    
                    //
                    // 3) Push to google calendar
                    //

                    string calendarId = "primary";
                    EventsResource.InsertRequest request = service.Events.Insert(newEvent, calendarId);
                    Event createdEvent = request.Execute();
                    Console.WriteLine("Event created: {0} ({1})", createdEvent.Summary, createdEvent.Start.Date);

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

        public void RemoveEventsFromCalendar(CalendarService service)
        {
            
        }

        private List<Course> cl;
    }
}
