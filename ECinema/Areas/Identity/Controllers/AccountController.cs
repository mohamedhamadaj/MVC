using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace ECinema.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            var user = new ApplicationUser()
            {
                UserName = registerVM.UserName,
                Email = registerVM.Email,
                FirstName = registerVM.FirstName,
                LastName = registerVM.LastName
            };
            var result = await _userManager.CreateAsync(user,registerVM.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Code);
                }
                return View(registerVM);
            }

            // send confirmation mail

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var link = Url.Action(nameof(ConfirmMail),"Account", new {area = "Identity", token,userId = user.Id});
            await _emailSender.SendEmailAsync(registerVM.Email, "ECinema - Confirm Your Email ",
                $"<hi Confirm Your Email By Clicking<a href = '{link}'>Here</a>></hi>");
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> ConfirmMail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                ModelState.AddModelError(string.Empty, "Invalid User");

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
                ModelState.AddModelError(string.Empty, "Invalid User");
            return RedirectToAction("Index", "Home", new { area = "Customer" });

        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            return View(loginVM);

            var user = await _userManager.FindByNameAsync(loginVM.UserNameOrEmail)?? await _userManager.FindByEmailAsync
                (loginVM.UserNameOrEmail);
            if (user is null)
            {
                ModelState.AddModelError(string.Empty, "Invalid User Name / Email OR Passsword");
            }
             var result = await _signInManager.PasswordSignInAsync(user,loginVM.Password,
                 loginVM.RememberMe,lockoutOnFailure:true);
            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "Your Account is Locked. Please try again After 5 min.");
                }
                else if (user.EmailConfirmed)
                {
                    ModelState.AddModelError(string.Empty, "Please Confirm Your Email First");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid User Name / Email OR Passsword");
                }
                return View(loginVM);
            }
            return RedirectToAction("Index", "Home", new { area = "Customer" } );
        }
    }
}
