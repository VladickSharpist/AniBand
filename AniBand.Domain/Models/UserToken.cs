using AniBand.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace AniBand.Domain.Models
{
    public class UserToken
        : IdentityUserToken<long>,
          IEntity
    {
        public long Id { get; set; }
    }
}