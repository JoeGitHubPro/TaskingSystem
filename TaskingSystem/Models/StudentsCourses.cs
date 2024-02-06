namespace TaskingSystem.Models
{
    public class StudentsCourses
    {
        public string StudentId { get; set; }
        public string CourseCode { get; set; }

        // Navigation properties
        public ApplicationUser? Student { get; set; }
        public Course? Course { get; set; }
    }
}
