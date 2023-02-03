using Data.Models.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Requests.Update
{
    public class UpdateChildIssueRequestModel
    {
        public Guid? Id { get; set; } = null!;
        public string? Name { get; set; } = null!;
        public string? Description { get; set; }
        public Guid? AssigneeId { get; set; }
        public Guid? PriorityId { get; set; }
        public Guid? StatusId { get; set; }
        public Guid? ReporterId { get; set; }
        public ICollection<LabelViewModel>? Labels { get; set; } = null!;
        public bool? IsClose { get; set; }
    }
}
