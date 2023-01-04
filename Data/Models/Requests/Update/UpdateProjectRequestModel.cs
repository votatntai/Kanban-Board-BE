using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Requests.Update
{
    public class UpdateProjectRequestModel
    {
        public string? Name { get; set; } = null!;
        public string? Description { get; set; }
        public Guid? LeaderId { get; set; }
        public Guid? DefaultAssigneeId { get; set; }
    }
}
