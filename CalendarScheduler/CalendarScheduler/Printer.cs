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
    class Printer
    {
        public Printer(CalendarService serv)
        {
            // get the calender api
            this.serv = serv;
        }

        public void AllCalendarNames()
        {
           // Print all calendars in console
           CalendarListResource.ListRequest request = serv.CalendarList.List();
           CalendarList calendarList = request.Execute();

           foreach (var item in calendarList.Items)
           {
                Console.WriteLine(item.Summary);
           }
        }

        public void AllCalendarIDs()
        {
            // Print all calendars in console
            CalendarListResource.ListRequest request = serv.CalendarList.List();
            CalendarList calendarList = request.Execute();

            foreach (var item in calendarList.Items)
            {
                Console.WriteLine(item.Id);
            }
        }

        private CalendarService serv;
    }
}
