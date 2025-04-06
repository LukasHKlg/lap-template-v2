using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Shared.Models
{
    public static class Roles
    {
        public static string Admin { get; set; } = "Admin";
        public static string User { get; set; } = "User";

        public static List<string> SelectableRoles = [Admin, User];
    }
}
