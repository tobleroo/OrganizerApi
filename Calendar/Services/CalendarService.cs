using Microsoft.Azure.Cosmos;
using OrganizerApi.Calendar.Models;
using OrganizerApi.Calendar.Models.CalendarDTOs;
using OrganizerApi.models;
using System;
using System.Globalization;

namespace OrganizerApi.Calendar.Services
{
    public class CalendarService : ICalendarService
    {

        public CalendarService() { }

        public List<CalendarDay> CreateCalendar()
        {
            var dates = CreateDays();
            
            var CalendarDays = CreateCalendarDays(dates);
            return CalendarDays;
        }

        public List<DateTime> CreateDays()
        {
            List<DateTime> dates = new List<DateTime>();

            for (int i = 0; i < 365; i++)
            {
                dates.Add(DateTime.Now.AddDays(i));
            }
            return dates;
        }

        public List<CalendarDay> CreateCalendarDays(List<DateTime> dates)
        {
            List<CalendarDay> days = new();

            foreach (DateTime date in dates)
            {
                string dayDate = date.ToString("yyyy-MM-dd");
                days.Add(new CalendarDay(dayDate, date.DayOfWeek.ToString()));
            }

            return days;
        }

        public void AddNewDates(List<CalendarDay> existingCalendar)
        {
            //get date from latest date in calendar
            var latest = existingCalendar[existingCalendar.Count - 1].Date;
            //create datetime object from latest
            string[] dateNumbers = latest.Split('-');

            DateTime dt = new DateTime(int.Parse(dateNumbers[0]), int.Parse(dateNumbers[1]), int.Parse(dateNumbers[2]));
            List<DateTime> newDates = new List<DateTime>();
            for (int i = 0;i < 365;i++) {
                newDates.Add(dt.AddDays(i));
            }

            var newCalendarDates = CreateCalendarDays(newDates);

            newCalendarDates.ForEach(dt =>
            {
                existingCalendar.Add(dt);
            });
        }

        public void AddTaskToCalendar(List<CalendarDay> existingCalendar, NewTaskDTO newTask)
        {
            existingCalendar.ForEach(dt =>
            {
                if (dt.Date == newTask.DateToAdd)
                {
                    dt.Tasks.Add(new CalendarTask(newTask.TaskName, newTask.timeToDoMinutes));
                }
            });
        }

        public void RemoveTaskFromCalendar(List<CalendarDay> existingCalendar, NewTaskDTO newTaskDTO)
        {
            existingCalendar
                .Where(dt => dt.Date == newTaskDTO.DateToAdd)
                .ToList()
                .ForEach(dt => dt.Tasks.RemoveAll(task => task.Title == newTaskDTO.TaskName));
        }

        public void UpdateTaskInCalendar(List<CalendarDay> existingCalendar, UpdateCalendarTaskDTO newTaskDTO)
        {

            var taskToUpdate = existingCalendar
                .Where(dt => dt.Date == newTaskDTO.DateToUpdate)
                .SelectMany(dt => dt.Tasks)
                .FirstOrDefault(task => task.Title == newTaskDTO.oldName);


            if (taskToUpdate != null)
            {
                taskToUpdate.Title = newTaskDTO.TaskName;
                taskToUpdate.timeToDoMinutes = newTaskDTO.timeToDoMinutes;
            }
        }

        public void AddNewEventToCalendar(List<CalendarDay> calendar, NewEventDTO newEvent)
        {
            calendar.Where(dt => dt.Date == newEvent.Date)
                .ToList()
                .ForEach(dt => dt.Events.Add(new CalendarEvent(newEvent.Title, newEvent.Description)));
        }

        public void RemoveEventFromCalendar(List<CalendarDay> calendar, NewEventDTO eventRemove)
        {
            calendar.Where(dt => dt.Date == eventRemove.Date)
                .ToList()
                .ForEach(dt => dt.Events.RemoveAll(ev => ev.Title == eventRemove.Title));
        }

        public CalendarEvent GetEventFromCalendar(CalendarEvent myEvent)
        {
            throw new NotImplementedException();
        }

        public CalendarTask GetTaskFromCalendar(CalendarEvent myEvent)
        {
            throw new NotImplementedException();
        }

        void ICalendarService.UpdateCalendar(AppUser user, List<CalendarDay> newCalendar)
        {
            user.Calendar = newCalendar;
        }
    }
}
