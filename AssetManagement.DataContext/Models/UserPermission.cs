// UserPermission.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssetManagement.DataContext.Models
{
    public class UserPermission
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }  // Assuming UserId is a string

        // Define other properties as needed
        public string Permission { get; set; }
    }
}
