namespace OrganizerApi.Todo.Models
{
    public class CalendarYear
    {

        private string Year { get; set; }
        private List<CalendarMonth> Months { get; set; }

        public CalendarYear(string year, List<CalendarMonth> months)
        {
            Year = year;
            Months = months;
        }
    }
}
