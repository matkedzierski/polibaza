using System.ComponentModel.DataAnnotations;
using System.Drawing;
using Microsoft.AspNetCore.Identity;
using PoliBaza.Pages.Shared;

namespace PoliBaza.Data;

public class UserPreferences
{
    public static readonly UserPreferences DEFAULT = new UserPreferences() { Theme = ThemeMode.Light, PrimaryColor = ColorTranslator.ToHtml(Color.Navy)};
    public enum ThemeMode
    {
        Dark, Light
    }

    [Key]
    public Guid Id { get; set; }
    public IdentityUser? User { get; init; }
    public ThemeMode? Theme { get; set; }
    public string? PrimaryColor { get; set; }
}