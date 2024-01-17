using WTC.Abstractions.Account.Interfaces;
using WTelegram;

namespace WTC.Abstractions.Account.Extensions;

public static class AccountHandlerExtensions
{
    public static Client? ByName(this IAccountHandler accountHandler, string name)
    {
        accountHandler.Clients.TryGetValue(name, out var client);
        return client;
    }
}