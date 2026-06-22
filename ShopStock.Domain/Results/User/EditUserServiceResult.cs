using ShopStock.Domain.Enums.User;

namespace ShopStock.Domain.Results.User
{
    public class EditUserServiceResult
    {
        public bool IsSuccess { get; set; }
        public List<EditUserResult> Errors { get; set; } = new();

        public static EditUserServiceResult Success() => new() { IsSuccess = true };

        public static EditUserServiceResult Failure(params EditUserResult[] errors)
            => new() { IsSuccess = false, Errors = errors.ToList() };
    }
}
