using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace demoapp.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }
        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();

            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }

            return RedirectToPage();
        }

        private IActionResult RedirectToPage()
        {
            throw new NotImplementedException();
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
