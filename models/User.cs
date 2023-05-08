using OrganizerApi.Todo.Models;

namespace OrganizerApi.models
{
    public class User
    {
        private int Id { get; set; }
        private string Name { get; set; }
        private string EmailAddress { get; set; }
        private string Password { get; set; }

        private Calendar TaskCalendar { get; set; }

        //public TaskList taskList { get; set; }

        public User(string name, string emailAddress, string password) { 
        
            // create calendar
            //create task list

            Name = name;
            EmailAddress = emailAddress;
            Password = password;

        }


    }
}
