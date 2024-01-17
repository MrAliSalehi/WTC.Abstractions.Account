using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using WTC.Abstractions.Account.Extensions;
using WTC.Abstractions.Account.Interfaces;
using WTelegram;

namespace WTC.Abstractions.Account.Implementations;

public sealed class AccountHandler : IAccountHandler
{
    private readonly ImmutableDictionary<string, Client> _clients;
    private readonly Client[] _clientsArray;
    private bool _withParallel;
    private TimeSpan _delay = TimeSpan.MinValue;
    ImmutableDictionary<string, Client> IAccountHandler.Clients
    {
        get => _clients;
    }
    internal AccountHandler(Dictionary<string, Client> clients)
    {
        _clients = clients.ToImmutableDictionary();
        _clientsArray = clients.Values.ToArray();
        Helpers.Log = Common.DefaultLogger;
    }

    public IReadOnlyList<Client> GetClients() => _clientsArray;

    public void DisposeAll()
    {
        ref var space = ref MemoryMarshal.GetArrayDataReference(_clientsArray);
        for (var i = 0; i < _clientsArray.Length; i++)
        {
            var client = Unsafe.Add(ref space, i);
            client.Dispose();
        }
    }
    public IAccountHandler WithDelay(TimeSpan delay)
    {
        _delay = _delay.Add(delay);
        return this;
    }

    public IAccountHandler OnParallel()
    {
        _withParallel = true;
        return this;
    }
    public async ValueTask<IAccountHandler> ForEachClientAsync(Func<Client, string, Task> onEachClient)
    {
        if (_withParallel)
        {
            await Parallel.ForEachAsync(_clients, async (keyValue, ct) => await Exec(onEachClient, keyValue.Value, keyValue.Key, ct));
        }
        else
        {
            await _clients.Iter(async (key, client) => await Exec(onEachClient, client, key));
        }

        return this;
    }
    private async Task Exec(Func<Client, string, Task> onEachClient, Client client, string name, CancellationToken ct = default)
    {
        await Task.Delay(_delay, ct);
        await onEachClient(client, name);
    }
}