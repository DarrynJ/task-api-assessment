using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks.Application.Task.Models
{
    public class CreateTaskDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid Assignee { get; set; }
        public DateTime DueDate { get; set; }
    }
}
