using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Requests.Create
{
    public class CreateChildIssueRequestModel
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; } = null!;
        public Guid? AssigneeId { get; set; }
        public int? EstimateTime { get; set; }
        public Guid? PriorityId { get; set; }
        public Guid ProjectId { get; set; }
        public Guid ParentId { get; set; }
        public Guid StatusId { get; set; }
        public Guid? TypeId { get; set; }
        public Guid? ReporterId { get; set; }
    }
}
