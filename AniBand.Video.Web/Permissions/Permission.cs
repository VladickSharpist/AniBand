namespace AniBand.Video.Web.Permissions
{
    public static class Permission
    {
        public static class AdminPermission
        {
            public const string AddVideo = "api.AniBand.Admin.AddVideo";
            public const string RemoveVideo = "api.AniBand.Admin.RemoveVideo";
        }
        
        public static class UserPermission
        {
            public const string CommentVideo = "api.AniBand.User.CommentVideo";
            public const string WatchVideo = "api.AniBand.User.WatchVideo";
        }
    }
}