using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Views
{
    public class LabelViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public Guid ProjectId { get; set; }
        public ICollection<IssueViewModel>? Issues { get; set; }
    }
}
