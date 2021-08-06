namespace AniBand.Core.Abstractions.Infrastructure.Helpers.Generic
{
    public interface IHttpResult<T>:IHttpResult
    {
        public T Data { get; set; }

        bool IsEmpty { get; }
    }
}