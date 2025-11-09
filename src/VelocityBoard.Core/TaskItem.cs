using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VelocityBoard.Core
{
    public enum TaskStatus
    {
        Todo = 0,
        InProgress = 1,
        Done = 2
    }

    public class TaskItem
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;
        public string? Description { get; set; }

        public TaskStatus Status { get; set; } = TaskStatus.Todo;

        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;

        public string? AssignedToUserId { get; set; }
        public DateTime? DueDate { get; set; }

        public string CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
