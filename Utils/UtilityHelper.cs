namespace ProveedorApi.Auth;

public class UtilityHelper
{
    public static DateTime ExpireToken(string p_expire)
    {
        DateTime Now = DateTime.Now;
        DateTime expire_token = DateTime.MinValue;
        string expires_token_loc = p_expire ?? "1h";
        short expire = Int16.Parse(expires_token_loc[0..^1]);
        char type_expire = expires_token_loc[^1];

        expire_token = type_expire switch
        {
            'm' => DateTime.UtcNow.AddMinutes(expire),
            'h' => DateTime.UtcNow.AddHours(expire),
            'd' => DateTime.UtcNow.AddDays(expire),
            'y' => DateTime.UtcNow.AddYears(expire),
            _ => DateTime.UtcNow.AddHours(1),
        };
        return expire_token;
    }
}