﻿using System.ComponentModel.DataAnnotations;

namespace AssetManagement.Dto
{
    public class Details
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }

    }
}