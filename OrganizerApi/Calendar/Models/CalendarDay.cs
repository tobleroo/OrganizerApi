
using System;
using System.Globalization;

namespace OrganizerApi.Calendar.Models
{
    public class CalendarDay
    {
        public string Date { get; set; }
        public string DayOfWeek { get; set; }
        public List<CalendarTask> Tasks { get; set; }
        public List<CalendarEvent> Events { get; set; }

        public CalendarDay(string date, string dayOfWeek)
        {
            this.Date = date;
            this.DayOfWeek = dayOfWeek;
            Tasks = new List<CalendarTask>();
            Events = new List<CalendarEvent>();
        }

        //create toString with all variables
        public override string ToString()
        {
            return "Date: " + Date + " Day of Week: " + DayOfWeek;
        }

    }
}
