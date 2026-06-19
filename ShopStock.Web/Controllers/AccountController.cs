using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ShopStock.Application.Services.Interfaces;
using ShopStock.Application.DTOs.Account;
using ShopStock.Web.ViewModels.Account;
using ShopStock.Domain.Enums.Account;

namespace ShopStock.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        #region Constructor

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        #endregion

        #region Register

        [HttpGet("Register")]
        public IActionResult Register(string returnUrl = "/")
        {
            // 👈 اگر لاگین بود، نیازی به ثبت‌نام مجدد نیست
            if (User.Identity is { IsAuthenticated: true })
            {
                return Redirect("/");
            }

            returnUrl = SanitizeReturnUrl(returnUrl);
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        [HttpPost("Register"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = "/")
        {
            returnUrl = SanitizeReturnUrl(returnUrl);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // تبدیل ویومدل به DTO
            var registerDto = new RegisterDto
            {
                UserName = model.UserName,
                Email = model.Email,
                Password = model.Password
            };

            // Process the registration logic here
            var result = await _accountService.RegisterAsync(registerDto);

            switch (result)
            {
                case RegisterUserResult.Success:
                    return RedirectToAction("RegistrationSuccess",
                        new { email = registerDto.Email, userName = registerDto.UserName, returnUrl = returnUrl });
                case RegisterUserResult.UserNameDuplicated:
                    ModelState.AddModelError("UserName", "نام کاربری وارد شده تکراری است.");
                    break;
                case RegisterUserResult.EmailDuplicated:
                    ModelState.AddModelError("Email", "ایمیل وارد شده تکراری است.");
                    break;
                case RegisterUserResult.InvalidInputs:
                    ModelState.AddModelError(string.Empty, "لطفاً ورودی های فرم را به درستی وارد کنید.");
                    break;
                case RegisterUserResult.FailedActivationEmailSending:
                    ModelState.AddModelError("Email", "ایمیل فعال‌سازی ارسال نشد. لطفاً دوباره تلاش کنید.");
                    break;
                case RegisterUserResult.Failed:
                    ModelState.AddModelError(string.Empty, "خطای ناشناخته‌ای رخ داد، لطفاً به پشتیبانی اطلاع دهید.");
                    break;
            }

            return View(model);
        }

        [HttpGet("RegistrationSuccess")]
        public IActionResult RegistrationSuccess(string email, string userName, string returnUrl = "/")
        {
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction(nameof(Register));
            }

            // ارسال ایمیل به ویو از طریق ViewBag یا یک ویومدل بسیار کوچک و امن
            ViewBag.Email = email;
            ViewBag.UserName = userName;
            // 👈 اضافه کردن این خط برای نگهداری آدرس در صفحه موفقیت
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        #endregion

        #region Activation Email

        [Route("VerifyEmail/{activeCode}")]
        public async Task<IActionResult> ActiveAccount(string activeCode)
        {
            if (string.IsNullOrEmpty(activeCode))
            {
                return RedirectToAction(nameof(Register));
            }

            ViewBag.ActivationResult = await _accountService.ActiveAccountAsync(activeCode);
            return View();
        }

        #endregion

        #region Login

        [HttpGet("Login")]
        public IActionResult Login(string returnUrl = "/")
        {
            // 👈 گارد امنیتی و UX: اگر کاربر از قبل لاگین است، او را به صفحه اصلی بفرست
            if (User.Identity is { IsAuthenticated: true })
            {
                return Redirect("/"); // یا هدایت به پنل کاربری
            }

            returnUrl = SanitizeReturnUrl(returnUrl);
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost("Login"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = "/")
        {
            returnUrl = SanitizeReturnUrl(returnUrl);
            ViewBag.ReturnUrl = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var loginDto = new LoginDto
            {
                UserNameOrEmail = model.UserNameOrEmail,
                Password = model.Password
            };

            var loginResult = await _accountService.LoginAsync(loginDto);

            switch (loginResult.Status)
            {
                case LoginUserResult.Success:
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier, loginResult.User.Id.ToString()),
                        new Claim(ClaimTypes.Name, loginResult.User.UserName),
                        new Claim(ClaimTypes.Email, loginResult.User.Email),
                        new Claim("FullName",$"{loginResult.User.FirstName} {loginResult.User.LastName}"),
                        new Claim("Mobile", loginResult.User.Mobile ?? ""),
                        new Claim("ProfilePicture", loginResult.User.ProfilePicture)
                    };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    var properties = new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe,
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, properties);
                    return Redirect(returnUrl);

                case LoginUserResult.UserNotFound:
                case LoginUserResult.InvalidPassword:
                    ModelState.AddModelError(string.Empty, "نام کاربری یا ایمیل وارد شده صحیح نیست.");
                    break;
                case LoginUserResult.UserNotActive:
                    ModelState.AddModelError(string.Empty, "حساب کاربری شما توسط مدیر سیستم مسدود شده است.");
                    break;
                case LoginUserResult.EmailNotActivated:
                    ViewBag.EmailNotActivated = true;
                    break;
                default:
                    ModelState.AddModelError(string.Empty, "خطای ناشناخته‌ای رخ داد، لطفاً دوباره تلاش کنید.");
                    break;
            }

            return View(model);
        }

        #endregion

        #region Logout

        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }

        #endregion

        #region Helper Methods

        private string SanitizeReturnUrl(string returnUrl)
        {
            // ۱. گارد اول: اگر خالی بود یا آدرس خارجی (خطرناک) بود، برگرد به صفحه اصلی
            if (string.IsNullOrWhiteSpace(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                return "/";
            }

            // ۲. گارد دوم: اگر آدرس محلی بود اما اشاره به صفحات خودِ احراز هویت داشت، باز هم برگرد به صفحه اصلی
            if (returnUrl.StartsWith("/Register", StringComparison.OrdinalIgnoreCase) ||
                returnUrl.StartsWith("/Login", StringComparison.OrdinalIgnoreCase) ||
                returnUrl.StartsWith("/RegistrationSuccess", StringComparison.OrdinalIgnoreCase))
            {
                return "/";
            }

            // ۳. اگر از هر دو فیلتر سلامت عبور کرد، آدرس معتبر است (مثل /admin یا /cart)
            return returnUrl;
        }

        #endregion
    }
}
