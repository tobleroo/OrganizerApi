using Microsoft.AspNetCore.Mvc;
using OrganizerApi.Calendar.Models;
using OrganizerApi.Calendar.Models.CalendarDTOs;
using OrganizerApi.models;

namespace OrganizerApi.Calendar.Services
{
    public interface ICalendarService
    {

        List<CalendarDay> CreateCalendar();
        List<DateTime> CreateDays();
        List<CalendarDay> CreateCalendarDays(List<DateTime> dates);
        void AddNewDates(List<CalendarDay> existingCalendar);
        void AddTaskToCalendar(List<CalendarDay> existingCalendar, NewTaskDTO newTask);
        void RemoveTaskFromCalendar(List<CalendarDay> existingCalendar, NewTaskDTO newTaskDTO);
        void UpdateTaskInCalendar(List<CalendarDay> existingCalendar, UpdateCalendarTaskDTO updateCalendarTask);
        void AddNewEventToCalendar(List<CalendarDay> calendar, NewEventDTO newEvent);

        void RemoveEventFromCalendar(List<CalendarDay> calendar, NewEventDTO eventRemove);
        void UpdateCalendar(AppUser user,List<CalendarDay> newCalendar);

        CalendarEvent GetEventFromCalendar(CalendarEvent myEvent);
        CalendarTask GetTaskFromCalendar(CalendarEvent myEvent);


    }
}
