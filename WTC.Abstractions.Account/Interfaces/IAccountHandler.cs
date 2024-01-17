using System.Collections.Immutable;
using WTelegram;

namespace WTC.Abstractions.Account.Interfaces;

public interface IAccountHandler
{
    internal ImmutableDictionary<string, Client> Clients { get; }
    public IReadOnlyList<Client> GetClients();
    public void DisposeAll();
    public ValueTask<IAccountHandler> ForEachClientAsync(Func<Client, string, Task> onEachClient);
    public IAccountHandler WithDelay(TimeSpan delay);
    public IAccountHandler OnParallel();
}