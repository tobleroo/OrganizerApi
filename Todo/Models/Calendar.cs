namespace OrganizerApi.Todo.Models
{
    public class Calendar
    {

        private List<CalendarYear> Years { get; set; }

        public Calendar(List<CalendarYear> years)
        {
            Years = years;
        }
    }
}
