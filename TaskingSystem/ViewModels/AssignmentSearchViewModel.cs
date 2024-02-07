using System.ComponentModel.DataAnnotations;

namespace TaskingSystem.ViewModels
{
    public class AssignmentSearchViewModel
    {
        [Required]
        public string Course { get; set; }

        public string? Student { get; set; }
    }
}
