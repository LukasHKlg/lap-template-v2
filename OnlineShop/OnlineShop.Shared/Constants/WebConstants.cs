using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Shared.Constants
{
    public static class WebConstants
    {
        public const string PasswordRegexPattern = "(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{8,}";
    }
}
