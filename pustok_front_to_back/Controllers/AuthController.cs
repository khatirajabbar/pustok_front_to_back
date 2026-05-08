using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using pustok_front_to_back.Models.Entities;
using pustok_front_to_back.Models.ViewModels;
using pustok_front_to_back.Services;
namespace pustok_front_to_back.Controllers;

public class AuthController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IEmailService _emailService;

    public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, IEmailService emailService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailService = emailService;
    }

    // Register GET
    public IActionResult Register()
    {
        return View();
    }

    // Register POST
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = new User
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            UserName = model.Email,
            IsEmailVerified = false
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            // Generate email verification token
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action("VerifyEmail", "Auth", new { userId = user.Id, token = token }, Request.Scheme);

            // Send verification email
            await _emailService.SendEmailAsync(user.Email, "Verify Your Email",
                $"<h2>Welcome to Pustok!</h2><p>Please verify your email by clicking the link below:</p><a href='{confirmationLink}'>Verify Email</a>");

            TempData["Message"] = "Registration successful! Please check your email to verify your account.";
            return RedirectToAction("Login");
        }

        foreach (var error in result.Errors)
            ModelState.AddModelError("", error.Description);

        return View(model);
    }

    // Verify Email
    public async Task<IActionResult> VerifyEmail(Guid userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return NotFound();

        var result = await _userManager.ConfirmEmailAsync(user, token);

        if (result.Succeeded)
        {
            user.IsEmailVerified = true;
            await _userManager.UpdateAsync(user);
            TempData["Message"] = "Email verified successfully! You can now login.";
            return RedirectToAction("Login");
        }

        TempData["Error"] = "Email verification failed!";
        return RedirectToAction("Register");
    }

    // Login GET
    [HttpGet]
    public IActionResult Login(string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    // Login POST
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user == null || !user.IsEmailVerified)
        {
            ModelState.AddModelError("", "Email not verified or user not found");
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

        if (result.Succeeded)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError("", "Invalid login attempt");
        return View(model);
    }

    // Login with Google - NEW ACTION
    [HttpGet("login-with-google")]
    public IActionResult LoginWithGoogle(string returnUrl = null)
    {
        var redirectUrl = Url.Action("GoogleResponse", "Auth", new { returnUrl });
        var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
        return Challenge(properties, "Google");
    }

    // Logout
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    // Google Login Callback
    [HttpGet]
    public async Task<IActionResult> GoogleResponse(string returnUrl = null)
    {
        var result = await _signInManager.GetExternalLoginInfoAsync();
        if (result == null)
            return RedirectToAction("Login");

        var signInResult = await _signInManager.ExternalLoginSignInAsync(result.LoginProvider, result.ProviderKey, false);
        
        if (signInResult.Succeeded)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Index", "Home");
        }

        // Create new user from Google info
        var user = new User
        {
            FirstName = result.Principal.FindFirst("given_name")?.Value ?? "",
            LastName = result.Principal.FindFirst("family_name")?.Value ?? "",
            Email = result.Principal.FindFirst("email")?.Value,
            UserName = result.Principal.FindFirst("email")?.Value,
            IsEmailVerified = true, // Google verified
        };

        var createResult = await _userManager.CreateAsync(user);
        if (createResult.Succeeded)
        {
            await _userManager.AddLoginAsync(user, result);
            await _signInManager.SignInAsync(user, false);
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Index", "Home");
        }

        return RedirectToAction("Login");
    }
}