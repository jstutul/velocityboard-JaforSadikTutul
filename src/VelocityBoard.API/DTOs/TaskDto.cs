namespace VelocityBoard.API.DTOs
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? AssignedToUserId { get; set; }
        public string AssignedToUserName { get; set; }
        public string Status { get; set; } = "Todo";
        public DateTime? DueDate { get; set; }
        public int ProjectId { get; set; }
    }
}
