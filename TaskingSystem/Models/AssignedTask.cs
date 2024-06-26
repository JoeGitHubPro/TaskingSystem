﻿using System.ComponentModel.DataAnnotations.Schema;

namespace TaskingSystem.Models
{
    public class AssignedTask
    {
        public int AssignedTaskId { get; set; }
        public int TaskId { get; set; }

        public string TaskURL { get; set; }
        public DateTime? AssignedTaskDate { get; set; }
        public string AssignedTaskStudentId { get; set; }

        [NotMapped]
        public IFormFile File { set; get; }

        // Navigation properties
        public AssignmentHeadLine? AssignmentHeadLine { get; set; }
        public ApplicationUser? AssignedTaskStudent { get; set; }
    }
}
