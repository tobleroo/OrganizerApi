namespace OrganizerApi.Calendar.Models.CalendarDTOs
{
    public class UpdateCalendarTaskDTO
    {

        public string DateToUpdate { get; set; }
        public string TaskName { get; set; }
        public int timeToDoMinutes { get; set; }
        public string oldName { get; set; }
    }
}
