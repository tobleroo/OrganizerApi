namespace OrganizerApi.Todo.Models
{
    public class CalendarDay
    {

        private string Date { get; set; }
        private string dayOfWeek { get; set; }
        private List<CalendarTask> Tasks { get; set; }

        public CalendarDay(string date, string dayOfWeek, List<CalendarTask> tasks)
        {
            Date = date;
            this.dayOfWeek = dayOfWeek;
            Tasks = tasks;
        }
    }
}
