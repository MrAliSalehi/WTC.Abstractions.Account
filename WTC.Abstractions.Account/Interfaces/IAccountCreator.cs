using WTelegram;

namespace WTC.Abstractions.Account.Interfaces;

public interface IAccountCreator
{
    public IAccountCreator FromPath(string path);

    public IAccountCreator FromAccount(TelAccount account, Func<string> verifyCode, string sessionName = "WTC.session");
    public IAccountCreator FromAccount(Func<IApiHash, ITelAccountBuilder> builder, Func<string> verifyCode, string sessionName = "WTC.session");
    public ValueTask<IAccountHandler> BuildAsync(bool disposeAfter = false);
    public ValueTask<IAccountHandler> BuildAsync(Func<string, Client, bool> disposeWhen);
}