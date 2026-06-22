using ShopStock.Domain.Enums.User;

namespace ShopStock.Domain.Results.User
{
    public class CreateUserServiceResult
    {
        public bool IsSuccess { get; set; }
        public List<CreateUserResult> Errors { get; set; } = new();

        public static CreateUserServiceResult Success() => new() { IsSuccess = true };

        public static CreateUserServiceResult Failure(params CreateUserResult[] errors)
            => new() { IsSuccess = false, Errors = errors.ToList() };
    }
}
