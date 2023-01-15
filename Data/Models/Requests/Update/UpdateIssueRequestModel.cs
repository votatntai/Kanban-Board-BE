using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Requests.Update
{
    public class UpdateIssueRequestModel
    {
        public Guid? Id { get; set; } = null!;
        public string? Name { get; set; } = null!;
        public string? Description { get; set; }
        public Guid? AssigneeId { get; set; }
        public int? EstimateTime { get; set; }
        public Guid? PriorityId { get; set; }
        public Guid? StatusId { get; set; }
        public int? Position { get; set; }
        public Guid? TypeId { get; set; }
        public DateTime? DueDate { get; set; }
        public Guid? ReporterId { get; set; }
        public bool? IsClose { get; set; }
    }
}
