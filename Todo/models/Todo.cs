using OrganizerApi.Calendar.Models;

namespace OrganizerApi.Todo.models
{
    public class Todo
    { 
        public List<CalendarTask> ActiveTasks { get; set; }
        public List<RepeatTask> RepeatableTasks{ get; set; }
    }
}
