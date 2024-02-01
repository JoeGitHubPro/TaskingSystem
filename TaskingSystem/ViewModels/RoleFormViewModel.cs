﻿using System.ComponentModel.DataAnnotations;

namespace TaskingSystem.ViewModels
{
    public class RoleFormViewModel
    {
        [Required, StringLength(256)]
        public string Name { get; set; }
    }
}
