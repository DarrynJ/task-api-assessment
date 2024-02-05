using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks.Domain.Entities
{
    public class Task
    {
        public Task(string title, string description, Guid assignee, DateTime dueDate)
        {
            Id = Guid.NewGuid();

            Title = title;
            Description = description;
            Assignee = assignee;
            DueDate = dueDate;
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid Assignee { get; set; }
        public DateTime DueDate { get; set; }
    }
}
