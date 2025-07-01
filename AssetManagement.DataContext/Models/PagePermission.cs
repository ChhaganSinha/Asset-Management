using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssetManagement.DataContext.Models
{
    public class PagePermission
    {
        [Key]
        public Guid Id { get; set; }

        // Foreign key to ApplicationRole
        public Guid ApplicationRoleId { get; set; }

        // Navigation property to ApplicationRole
        [ForeignKey(nameof(ApplicationRoleId))]
        public ApplicationRole ApplicationRole { get; set; }

        // Other properties as needed
        public string PageName { get; set; }
        public bool CanView { get; set; }
        public bool CanEdit { get; set; }
        public bool HasFullAccess { get; set; }
    }
}
