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
            

            // Print all calendars in console
            CalendarListResource.ListRequest request = service.CalendarList.List();
            CalendarList calendarList = request.Execute();

            foreach (var item in calendarList.Items)
            {
                Console.WriteLine(item.Id);
                Console.WriteLine(item.ETag);
                Console.WriteLine(item.Summary);
                Console.WriteLine();
            }
            


            /*
            // Define parameters of request.
            EventsResource.ListRequest request = service.Events.List("primary");
            request.TimeMin = DateTime.Now;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.MaxResults = 10;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;
            */



            /*
            // List events.
            Events events = request.Execute();
            Console.WriteLine("Upcoming events:");
            if (events.Items != null && events.Items.Count > 0)
            {
                foreach (var eventItem in events.Items)
                {
                    string when = eventItem.Start.DateTime.ToString();
                    if (String.IsNullOrEmpty(when))
                    {
                        when = eventItem.Start.Date;
                    }
                    Console.WriteLine("{0} ({1})", eventItem.Summary, when);
                }
            }
            else
            {
                Console.WriteLine("No upcoming events found.");
            }
            Console.Read();
            */

            // keep console window open
            Console.Read();
        }
    }
}
