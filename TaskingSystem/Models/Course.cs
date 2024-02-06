namespace TaskingSystem.Models
{
    public class Course
    {
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string ProfessorId { get; set; }

        // Navigation property
        public ICollection<AssignmentHeadLine> AssignmentHeadLines { get; set; }
        public ICollection<StudentsCourses> StudentsCourses { get; set; }

        public ApplicationUser Professor { get; set; }

    }
}
