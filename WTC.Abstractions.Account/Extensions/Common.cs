using WTelegram;

namespace WTC.Abstractions.Account.Extensions;

internal static class Common
{
    public static Action<int, string>? DefaultLogger;

    public static async Task Iter(this IEnumerable<Client> clients, Func<Client, Task> expr)
    {
        foreach (var client in clients)
            await expr(client);
    }
    public static async Task Iter(this IDictionary<string, Client> clients, Func<string, Client, Task> expr)
    {
        foreach (var client in clients)
            await expr(client.Key, client.Value);
    }


    public static void ThrowIfNullOrEmpty(string? str)
    {
#if NET8_0_OR_GREATER
        ArgumentException.ThrowIfNullOrEmpty(str);
#else
        if (str is null or "") throw new ArgumentException("argument cannot be null or empty");
#endif
    }
}