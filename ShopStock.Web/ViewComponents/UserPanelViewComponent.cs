using Microsoft.AspNetCore.Mvc;
using ShopStock.Application.Contracts;
using ShopStock.Web.Extensions;
using ShopStock.Web.ViewModels.Account;

namespace ShopStock.Web.ViewComponents
{
    public class UserPanelViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
