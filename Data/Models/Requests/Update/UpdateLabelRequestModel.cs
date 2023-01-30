using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Requests.Update
{
    public class UpdateLabelRequestModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
