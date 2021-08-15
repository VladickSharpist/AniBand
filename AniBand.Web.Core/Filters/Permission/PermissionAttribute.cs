using System;
using Microsoft.AspNetCore.Mvc;

namespace AniBand.Auth.Web.Filters.Permission
{
    public class PermissionAttribute : TypeFilterAttribute
    {
        public PermissionAttribute(params string[] permissions) 
            : base(typeof(PermissionFilter))
        {
            Arguments = new object[] { permissions } ;
            Order = Int32.MaxValue;
        }
    }
}