using WTC.Abstractions.Account.Extensions;
using WTC.Abstractions.Account.Interfaces;

namespace WTC.Abstractions.Account.Implementations;

public sealed class TelAccountBuilder : IApiHash, IApiId, IPhoneNumber, IOptionalInfo
{
    private readonly TelAccount _account;
    private TelAccountBuilder(TelAccount account)
    {
        _account = account;
    }
    public static IApiHash New() => new TelAccountBuilder(TelAccount.Empty);

    public TelAccount Build() => _account;
    public IApiId WithHash(string hash)
    {
        Common.ThrowIfNullOrEmpty(hash);
        _account.ApiHash = hash;
        return this;
    }
    public IPhoneNumber WithApiId(string id)
    {
        Common.ThrowIfNullOrEmpty(id);
        _account.ApiId = id;
        return this;
    }
    public IOptionalInfo WithPhone(string phone)
    {
        Common.ThrowIfNullOrEmpty(phone);
        _account.PhoneNumber = phone;
        return this;
    }

    public IOptionalInfo WithFirstName(string firstName)
    {
        Common.ThrowIfNullOrEmpty(firstName);
        _account.FirstName = firstName;
        return this;
    }
    public IOptionalInfo WithLastName(string lastName)
    {
        Common.ThrowIfNullOrEmpty(lastName);
        _account.LastName = lastName;
        return this;
    }
    public IOptionalInfo WithTwoFactorAuth(string password)
    {
        Common.ThrowIfNullOrEmpty(password);
        _account.TwoFactor = password;
        return this;
    }
    public IOptionalInfo UseTestServers(bool use)
    {
        _account.UseTestServer = use;
        return this;
    }
}