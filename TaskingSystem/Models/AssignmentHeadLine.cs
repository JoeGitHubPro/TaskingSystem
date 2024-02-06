namespace TaskingSystem.Models
{
    public class AssignmentHeadLine
    {
        public int AssignmentId { get; set; }
        public string AssignmentName { get; set; }
        public DateTime? AssignmentDate { get; set; }
        public string ProfessorId { get; set; }
        public string CourseCode { get; set; }

        // Navigation properties
        public Course Course { get; set; }
        public ApplicationUser Professor { get; set; }
    }
}
