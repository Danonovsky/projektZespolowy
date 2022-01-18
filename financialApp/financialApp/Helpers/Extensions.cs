namespace financialApp.Helpers;

public static class Extensions
{
    public static Guid GetUserId(this IHttpContextAccessor httpContextAccessor)
    {
        var id = new Guid(httpContextAccessor.HttpContext.User.Claims.First(_ => _.Type == "Id").Value);
        return id;
    }
}
