using System.Text.RegularExpressions;

namespace Fundipedia.TechnicalInterview.Helpers
{
    public static class ValidationHelper
    {
        public static bool IsValidEmail(string email)
        {
            return new Regex(@"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$").IsMatch(email);
        }

        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            // Could expand on this but not in instructions
            return new Regex(@"^\d{1,10}$").IsMatch(phoneNumber);
        }
    }
}