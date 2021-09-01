using System;

namespace AniBand.Auth.Services.Abstractions.Models
{
    public class UserDto
    {
        public long Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public DateTime RegistrationDate { get; set; }
    }
}