using System.Text.RegularExpressions;

namespace OrganizerApi.Auth.Utils
{
    public class AuthChecks
    {

        public static bool CheckValidEmail(string email)
        {
            string regex = @"^([a-zA-Z0-9_\.]+)@([a-zA-Z0-9\-\.]+)\.([a-zA-Z]{2,})$";
            // Check if the string matches the regular expression
            return Regex.IsMatch(email, regex);
        }
    }
}
