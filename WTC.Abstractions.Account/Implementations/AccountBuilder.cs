using WTC.Abstractions.Account.Extensions;
using WTC.Abstractions.Account.Interfaces;
using WTelegram;

namespace WTC.Abstractions.Account.Implementations;

public class AccountBuilder : IAccountCreator
{
    private readonly Dictionary<string, Client> _clients;
    private AccountBuilder()
    {
        _clients = new Dictionary<string, Client>();
    }
    public static AccountBuilder CreateAccounts()
    {
        Common.DefaultLogger = Helpers.Log;
        Helpers.Log = (_, _) => { };
        return new();
    }


    public IAccountCreator FromPath(string path)
    {
        var client = new Client(null, new FileStream(path, FileMode.Open));
        ArgumentNullException.ThrowIfNull(client);
        _clients.Add(new FileInfo(path).Name.Replace(".session", ""), client);

        return this;
    }

    public IAccountCreator FromAccount(TelAccount account, Func<string> verifyCode, string sessionName = "WTC.session")
    {
        CreateAccount(account, verifyCode, sessionName);
        return this;
    }

    public IAccountCreator FromAccount(Func<IApiHash, ITelAccountBuilder> builder, Func<string> verifyCode, string sessionName = "WTC.session")
    {
        var account = builder(TelAccountBuilder.New()).Build();
        CreateAccount(account, verifyCode, sessionName);
        return this;
    }

    public async ValueTask<IAccountHandler> BuildAsync(bool disposeAfter = false)
    {
        await _clients.Values.Iter(async client =>
        {
            await client.LoginBotIfNeeded();
            if (disposeAfter)
                client.Dispose();
        });

        return new AccountHandler(_clients);
    }
    public async ValueTask<IAccountHandler> BuildAsync(Func<Client, bool> disposeWhen)
    {
        await _clients.Values.Iter(async client =>
        {
            await client.LoginBotIfNeeded();
            if (disposeWhen(client))
                client.Dispose();
        });
        return new AccountHandler(_clients);
    }

    private void CreateAccount(TelAccount account, Func<string> verifyCode, string sessionName)
    {
        var client = new Client(str =>
        {
            return str switch
            {
                "api_id"            => account.ApiId,
                "api_hash"          => account.ApiHash,
                "phone_number"      => account.PhoneNumber,
                "verification_code" => verifyCode(),
                "first_name"        => account.FirstName,
                "last_name"         => account.LastName,
                "password"          => account.TwoFactor,
                "server_address"    => account.UseTestServer ? TelAccount.TestServerAddr : null,
                _                   => null
            };
        }, new FileStream(sessionName, FileMode.OpenOrCreate));

        ArgumentNullException.ThrowIfNull(client);

        _clients.Add(sessionName.Replace(".session", ""), client);
    }
}