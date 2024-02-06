namespace TaskingSystem.Models
{
    public class AssignedTask
    {
        public int TaskId { get; set; }
        public int? AssignedTaskId { get; set; }
        public string TaskURL { get; set; }
        public DateTime? AssignedTaskDate { get; set; }
        public string AssignedTaskStudentId { get; set; }

        // Navigation properties
        public AssignmentHeadLine AssignmentHeadLine { get; set; }
        public ApplicationUser AssignedTaskStudent { get; set; }
    }
}
