using System.Collections;

namespace PoliBaza.Pages.Shared;

public static class RoleNames
{
    public const string ADMIN = "ADMIN";
    public const string USER = "USER";

    public static IEnumerable<string> GetAll()
    {
        return new[] { ADMIN, USER };
    }
}