namespace ShopStock.Domain.Results
{
    public class ServiceResult<TResult>
    {
        public bool IsSuccess { get; set; }
        public List<TResult> Errors { get; set; } = new();

        public static ServiceResult<TResult> Success() => new() { IsSuccess = true };
        public static ServiceResult<TResult> Failure(params TResult[] errors)
            => new() { IsSuccess = false, Errors = errors.ToList() };
    }
}