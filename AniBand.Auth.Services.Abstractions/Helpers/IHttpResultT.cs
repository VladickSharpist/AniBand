namespace AniBand.Auth.Services.Abstractions.Helpers
{
    public interface IHttpResult<T>:IHttpResult
    {
        public T Data { get; set; }
        
        bool IsEmpty => Data == null;
        
    }
}