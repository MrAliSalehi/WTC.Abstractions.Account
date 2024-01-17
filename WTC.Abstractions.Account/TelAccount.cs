namespace WTC.Abstractions.Account;

#if NET8_0_OR_GREATER
public sealed class TelAccount(string apiId, string apiHash, string phoneNumber, string? twoFactor = null, string? firstName = null, string? lastName = null, bool isUseTest = false)
{
    public static TelAccount Empty { get; } = new("", "", "");
    public string ApiId { get; internal set; } = apiId;
    public string ApiHash { get; internal set; } = apiHash;
    public string PhoneNumber { get; internal set; } = phoneNumber;
    public string? TwoFactor { get; internal set; } = twoFactor;
    public string? FirstName { get; internal set; } = firstName;
    public string? LastName { get; internal set; } = lastName;
    public bool UseTestServer { get; internal set; } = isUseTest;
    public const string TestServerAddr = "149.154.167.40:443";
}
#else
public sealed class TelAccount
{
    public TelAccount(string apiId, string apiHash, string phoneNumber, string? twoFactor = null, string? firstName = null, string? lastName = null, bool isUseTest = false)
    {
        ApiId = apiId;
        ApiHash = apiHash;
        PhoneNumber = phoneNumber;
        TwoFactor = twoFactor;
        FirstName = firstName;
        LastName = lastName;
        UseTestServer = isUseTest;
    }
    public static TelAccount Empty { get; } = new("", "", "");
    public string ApiId { get; internal set; }
    public string ApiHash { get; internal set; }
    public string PhoneNumber { get; internal set; }
    public string? TwoFactor { get; internal set; }
    public string? FirstName { get; internal set; }
    public string? LastName { get; internal set; }
    public bool UseTestServer { get; internal set; }
    public const string TestServerAddr = "149.154.167.40:443";
}
#endif
