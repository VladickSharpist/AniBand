namespace AniBand.SignalR.Services.Abstractions.Models
{
    public class NotificationDto
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public string Message { get; set; }
    }
}