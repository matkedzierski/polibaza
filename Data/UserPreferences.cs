using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace PoliBaza.Data;

public class UserPreferences
{
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