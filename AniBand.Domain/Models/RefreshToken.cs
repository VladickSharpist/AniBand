namespace AniBand.Domain.Models
{
    public class RefreshToken:BaseModel
    {
        public string Token { get; set; }

        public string OwnerId { get; set; }

        public virtual User Owner { get; set; }
        
    }
}
