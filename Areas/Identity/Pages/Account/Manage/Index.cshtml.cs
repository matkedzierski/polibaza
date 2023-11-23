﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Drawing;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PoliBaza.Data;
using PoliBaza.Pages.Shared;

namespace PoliBaza.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _db;
        private readonly UserPreferencesService _prefsService;

        public IndexModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ApplicationDbContext dbContext,
            UserPreferencesService prefsService,
            ILogger<IndexModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = dbContext;
            _prefsService = prefsService;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Display(Name = "Nazwa użytkownika")]
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone(ErrorMessage = "{0} nie jest poprawny.")]
            [Display(Name = "Numer telefonu")]
            public string PhoneNumber { get; init; }


            [Display(Name = "Motyw")] public bool IsDarkMode { get; init; }

            [Display(Name = "Kolor przewodni")] public string PrimaryColor { get; init; }
        }

        private async Task LoadAsync(IdentityUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            var prefs = await _prefsService.GetUserPreferences(User);
            var theme = prefs.Theme;
            var color = prefs.PrimaryColor;

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                IsDarkMode = theme == UserPreferences.ThemeMode.Dark,
                PrimaryColor = color
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Nie znaleziono użytkownika o ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Nie znaleziono użytkownika o ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Nieoczekiwany błąd podczas zmiany numeru telefonu.";
                    return RedirectToPage();
                }
            }

            if (!await UpdatePrefs(user))
            {
                return RedirectToPage();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Twój profil został zaktualizowany";
            return RedirectToPage();
        }

        public async Task<bool> UpdatePrefs(IdentityUser user)
        {
            var prefs = await _prefsService.GetUserPreferences(User);
            var prefsChanged = false;
            var theme = prefs.Theme ?? UserPreferences.ThemeMode.Light;
            var isDark = theme == UserPreferences.ThemeMode.Dark;
            var color = prefs.PrimaryColor ?? ColorTranslator.ToHtml(Color.Navy).ToLower();

            if (Input.IsDarkMode != isDark)
            {
                prefs.Theme = Input.IsDarkMode ? UserPreferences.ThemeMode.Dark : UserPreferences.ThemeMode.Light;
                prefsChanged = true;
            }
            
            if (Input.PrimaryColor != color)
            {
                prefs.PrimaryColor = Input.PrimaryColor;
                prefsChanged = true;
            }

            if (!prefsChanged) return true;
            await _db.SaveChangesAsync();
            _prefsService.StoreCachedPrefs(user.Id, prefs);
            return false;
        }
    }
}