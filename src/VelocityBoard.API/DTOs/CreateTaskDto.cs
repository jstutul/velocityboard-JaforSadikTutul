namespace VelocityBoard.API.DTOs
{
    public class CreateTaskDto
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? AssignedToUserId { get; set; }
        public int ProjectId { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
