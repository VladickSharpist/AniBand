using System;

namespace AniBand.Auth.Web.Models
{
    public class ApproveUserVm
    {
        public long Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public DateTime RegistrationDate { get; set; }
    }
}