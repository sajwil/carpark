using Emprevo.Api.Constants;

namespace Emprevo.Api.Models
{

    public class Result<T>
    {
        public ResultCode Status { get; set; }
        public T? Data { get; set; }
        public string? Message { get; set; }
        public bool IsSuccess => Status == ResultCode.Ok || Status == ResultCode.Created;

        public Result() { }

        public Result(T data)
        {
            Status = ResultCode.Ok;
            Data = data;
        }
    }
}