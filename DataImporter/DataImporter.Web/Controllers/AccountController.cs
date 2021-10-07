using DataImporter.Importing.Services.Mail;
using DataImporter.Membership.Entities;
using DataImporter.Membership.Services;
using DataImporter.Web.Models;
using DataImporter.Web.Models.Account;
using DataImporter.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace DataImporter.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AccountController> _logger;
        private readonly RoleManager<Role> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly RecaptchaService _recaptcha;
        private readonly IMailService _mailService;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<Role> roleManager,
            ILogger<AccountController> logger,
            IEmailSender emailSender,
            RecaptchaService recaptcha,
            IMailService mailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
            _emailSender = emailSender;
            _recaptcha = recaptcha;
            _mailService = mailService;
        }

        public async Task<IActionResult> ConfirmEmail(RegisterConfirmationModel model, string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                model.Status = false;
                return View(model);
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            model.Status = result.Succeeded;
            model.Email = user.Email;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            var recaptcha = _recaptcha.RecaptchaVerify(model.Token);

            if (!recaptcha.Result.success && recaptcha.Result.score <= 0.5)
            {
                ModelState.AddModelError(string.Empty, "Verification failed...");
            }

            model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                if (!await _roleManager.RoleExistsAsync("Member"))
                    await _roleManager.CreateAsync(new Role("Member"));

                var user = new ApplicationUser { UserName = model.Email, Email = model.Email,
                    FirstName = model.FullName.Split()[0], LastName = model.FullName.Split()[1] };
                var result = await _userManager.CreateAsync(user, model.Password);

                await _userManager.AddToRoleAsync(user, "Member");

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.ActionLink(
                        action: "ConfirmEmail",
                        controller: "Account",
                        values: new { userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    //await _emailSender.SendEmailAsync(model.Email, "Confirm your email",
                    //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    await _mailService.SendEmailAsync(new MailRequest()
                    {
                        Subject = "Email Confirmation",
                        ToEmail = model.Email,
                        Body = $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>."
                    });

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return Ok("Please Check your Mailbox");
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                    }

                }
                string messages = string.Join("; ", result.Errors
                        .Select(x => x.Description));
                return BadRequest(messages);
            }
            else
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                return BadRequest(messages);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {

            model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            var recaptcha = _recaptcha.RecaptchaVerify(model.Token);

            if (!recaptcha.Result.success && recaptcha.Result.score <= 0.5)
            {
                ModelState.AddModelError(string.Empty, "Verification failed...");
            }

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }

                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    ModelState.AddModelError(string.Empty, "User account locked out.");
                }
            }

            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                return BadRequest(messages);
            }
            else
            {
                return Ok();
            }
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction("Index", "Home");
        }
    }
}
