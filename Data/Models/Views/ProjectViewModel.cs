using Data.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Views
{
    public class ProjectViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public UserViewModel Leader { get; set; } = null!;
        public UserViewModel DefaultAssignee { get; set; } = null!;
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public bool IsClose { get; set; }
    }
}
