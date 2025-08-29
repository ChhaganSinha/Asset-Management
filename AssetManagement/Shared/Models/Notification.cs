using System;

namespace AssetManagement.Dto.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
