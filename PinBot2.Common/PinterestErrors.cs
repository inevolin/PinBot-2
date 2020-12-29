using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinBot2.Common
{
    public class PinterestErrors
    {
        public static string NO_ERROR = "\"error\":null}}";
        public static string AUTHORIZATION_ERROR = "\"message\":\"Authorization failed.\"";
        public static string WRONG_EMAIL_OR_PASS = "\"message\":\"Hmm, wrong email or password. Try again.\"";
        public static string STRANGE_ACTIVITY = "\"message\":\"There's been some strange activity on your account";
        public static string PINNER_ALREADY_INVITED = "\"message\":\"This user has already been invited to pin on this board!\"";
    }
}
