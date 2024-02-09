using System.ComponentModel.DataAnnotations;

namespace TaskingSystem.Models
{
    public class Theme
    {
        [Required]
        [Key]
        public int Id { get; set; }

        public string? ThemeName { get; set; }

        [Required]
        public bool ThemeSelected { get; set; }



    }

}
