using Microsoft.AspNetCore.Mvc;

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
