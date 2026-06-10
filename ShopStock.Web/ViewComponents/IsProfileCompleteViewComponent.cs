using Microsoft.AspNetCore.Mvc;
using ShopStock.Application.Contracts;
using ShopStock.Web.Extensions;

namespace ShopStock.Web.ViewComponents
{
    public class IsProfileCompleteViewComponent : ViewComponent
    {
        private readonly IAccountService _accountService;

        public IsProfileCompleteViewComponent(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            int userId = HttpContext.User.GetUserId();
            bool model = await _accountService.IsProfileComplete(userId);
            
            return View(model);
        }
    }
}
