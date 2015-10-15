using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CalendarScheduler
{
    class CourseList
    {
        public CourseList()
        {
            cl = new List<Course>();
        }

        public void AddCourse(Course c)
        {
            cl.Add(c);
        }

        public Course getCourse(int index)
        {
            return cl[index];
        }

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

        public void printList()
        {
            foreach (var course in cl)
            {
                Console.WriteLine(course);
            }
        }

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
