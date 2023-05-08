namespace OrganizerApi.Todo.Models
{
    public class CalendarMonth
    {

        private string MonthName { get; set; }
        private List<CalendarDay> Days { get; set; }

        public CalendarMonth(string monthName, List<CalendarDay> days)
        {
            this.MonthName = monthName;
            Days = days;
        }
    }
}
