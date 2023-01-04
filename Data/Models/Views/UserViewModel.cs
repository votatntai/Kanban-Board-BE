using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Views
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
