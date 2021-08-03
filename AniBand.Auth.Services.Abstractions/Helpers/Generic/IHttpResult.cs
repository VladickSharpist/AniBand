namespace AniBand.Auth.Services.Abstractions.Helpers.Generic
{
    public interface IHttpResult<T>:IHttpResult
    {
        public T Data { get; set; }

        bool IsEmpty { get; }
    }
}