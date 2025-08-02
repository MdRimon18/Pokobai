using System.Security.Claims;

public static class ClaimsPrincipalExtensions
{
    public static long GetUserId(this ClaimsPrincipal principal)
    {
        var value = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return value != null && long.TryParse(value, out var userId) ? userId : 0;
    }

    public static string GetName(this ClaimsPrincipal principal)
    {
        return principal.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
    }

    public static string GetRoleName(this ClaimsPrincipal principal)
    {
        return principal.FindFirst(ClaimTypes.Role)?.Value ?? "User";
    }

    public static string GetPhoneNo(this ClaimsPrincipal principal)
    {
        return principal.FindFirst("PhoneNo")?.Value ?? string.Empty;
    }

    public static long GetCompanyId(this ClaimsPrincipal principal)
    {
        var value = principal.FindFirst("CompanyId")?.Value;
        return value != null && long.TryParse(value, out var companyId) ? companyId : 0;
    }

    public static string GetCompanyLogo(this ClaimsPrincipal principal)
    {
        return principal.FindFirst("CompanyLogoImgLink")?.Value ?? string.Empty;
    }
    public static string GetUserImage(this ClaimsPrincipal principal)
    {
        return principal.FindFirst("UserImage")?.Value ?? string.Empty;
    }
    public static string GetCompanyName(this ClaimsPrincipal principal)
    {
        return principal.FindFirst("CompanyName")?.Value ?? string.Empty;
    }
    public static bool IsAuthenticated(this ClaimsPrincipal principal)
    {
        return principal.Identity?.IsAuthenticated ?? false;
    }
}