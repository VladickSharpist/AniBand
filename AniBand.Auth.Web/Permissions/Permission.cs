namespace AniBand.Auth.Web.Permissions
{
    public static class Permission
    {
        public static class AdminPermission
        {
            public const string AddVideo = "api.AniBand.Admin.AddVideo";
            public const string RemoveVideo = "api.AniBand.Admin.RemoveVideo";
            public const string ApproveUser = "api.AniBand.Admin.ApproveUser";
            public const string DeclineUser = "api.AniBand.Admin.DeclineUser";
            public const string GetUsers = "api.AniBand.Admin.GetUsers";
        }
        
        public static class UserPermission
        {
            public const string Approved = "api.AniBand.User.Approved";
            public const string CommentVideo = "api.AniBand.User.CommentVideo";
            public const string WatchVideo = "api.AniBand.User.WatchVideo";
        }
    }
}