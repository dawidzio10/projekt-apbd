﻿using System.ComponentModel.DataAnnotations;

namespace APBD_projekt.DTOs.Requests
{
    public class LoginRequest
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }
}