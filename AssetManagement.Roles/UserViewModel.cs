﻿namespace AssetManagement.Roles
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
    }
}