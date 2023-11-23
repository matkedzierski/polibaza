using System.Drawing;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using PoliBaza.Data;

namespace PoliBaza.Pages.Shared;

public class ColorService
{
    private ApplicationDbContext _db;
    private UserManager<IdentityUser> _userManager;

    public ColorService(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager)
    {
        _db = dbContext;
        _userManager = userManager;
    }

    private static readonly Color LIGHT_COLOR = Color.White;
    private static readonly Color DARK_COLOR = Color.FromArgb(1, 50, 50,50);

    public async Task<string> GetUserColor(ClaimsPrincipal principal, bool inverted = false)
    {
        var user = await _userManager.GetUserAsync(principal);
        var color = _db.UserPreferences.FirstOrDefault(p => p.User == user)?.PrimaryColor ?? ColorTranslator.ToHtml(Color.Navy);

        if (inverted)
        {
            color = FindContrastingColor(color);
        }
        
        return color;
    }

    public static string FindContrastingColor(string col1)
    {
        var color = ColorTranslator.FromHtml(col1); 
        int d = 0;  

        // Counting the perceptive luminance - human eye favors green color...      
        double luminance = (0.299 * color.R + 0.587 * color.G + 0.114 * color.B)/255;

        if (luminance > 0.5)
            d = 0; // bright colors - black font
        else
            d = 255; // dark colors - white font
            
        return  ColorTranslator.ToHtml((Color.FromArgb(d, d, d)));
    }
    
    public async Task<string> GetUserBackground(ClaimsPrincipal principal, bool inverted = false)
    {
        var user = await _userManager.GetUserAsync(principal);
        var theme = _db.UserPreferences.FirstOrDefault(p => p.User == user)?.Theme ?? UserPreferences.ThemeMode.Light;
        if (inverted)
        {
            theme = theme switch
            {
                UserPreferences.ThemeMode.Dark => UserPreferences.ThemeMode.Light,
                UserPreferences.ThemeMode.Light => UserPreferences.ThemeMode.Dark,
                _ => theme
            };
        }
        var color = theme == UserPreferences.ThemeMode.Dark ? DARK_COLOR : LIGHT_COLOR;
        return ColorTranslator.ToHtml(color);
    }
    
    public string PageNavColor(ViewContext viewContext, string page, String userColor)
    {
        var activePage = viewContext.ViewData["ActivePage"] as string
                         ?? Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
        return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? userColor : "transparent";
    }
}