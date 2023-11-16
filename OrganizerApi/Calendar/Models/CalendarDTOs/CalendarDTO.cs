namespace OrganizerApi.Calendar.Models.CalendarDTOs
{
    public class CalendarDTO
    {
        public string Date { get; set; }
        public string Day { get; set; }
        public List<CalendarEvent> Events { get; set; }
        public List<CalendarTask> Tasks { get; set; }
        public CalendarDTO(string date, string day, List<CalendarEvent> events, List<CalendarTask> tasks)
        {
            Date = date;
            Day = day;
            Events = events;
            Tasks = tasks;
        }
    }
}
