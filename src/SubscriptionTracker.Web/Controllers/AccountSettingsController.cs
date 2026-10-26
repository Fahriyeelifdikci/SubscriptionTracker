
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SubscriptionTracker.Web.ViewModels;

namespace SubscriptionTracker.Web.Controllers;

[Authorize]
public class AccountSettingsController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public AccountSettingsController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            return Challenge();
        }

        var model = new ChangePasswordViewModel
        {
            Email = user.Email ?? string.Empty
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(
        ChangePasswordViewModel model)
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            return Challenge();
        }

        model.Email = user.Email ?? string.Empty;

        if (!ModelState.IsValid)
        {
            return View("Index", model);
        }

        var result = await _userManager.ChangePasswordAsync(
            user,
            model.CurrentPassword,
            model.NewPassword
        );

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                var message = error.Code switch
                {
                    "PasswordMismatch" =>
                        "Mevcut şifreniz yanlış.",

                    "PasswordTooShort" =>
                        "Yeni şifreniz çok kısa.",

                    "PasswordRequiresDigit" =>
                        "Yeni şifreniz en az bir rakam içermelidir.",

                    "PasswordRequiresUpper" =>
                        "Yeni şifreniz en az bir büyük harf içermelidir.",

                    "PasswordRequiresLower" =>
                        "Yeni şifreniz en az bir küçük harf içermelidir.",

                    _ => "Şifre değiştirilemedi. Lütfen bilgilerinizi kontrol edin."
                };

                ModelState.AddModelError(
                    string.Empty,
                    message
                );
            }

            return View("Index", model);
        }

        // Şifre değiştikten sonra oturum bilgilerini yenile
        await _signInManager.RefreshSignInAsync(user);

        TempData["SuccessMessage"] =
            "Şifreniz başarıyla değiştirildi.";

        return RedirectToAction(nameof(Index));
    }
}
