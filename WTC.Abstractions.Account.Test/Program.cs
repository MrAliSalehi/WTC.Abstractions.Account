using WTC.Abstractions.Account;
using WTC.Abstractions.Account.Extensions;
using WTC.Abstractions.Account.Implementations;

var telAccount1 = new TelAccount("", "", "", "");
var telAccount2 = new TelAccount("", "", "", "", isUseTest: true);

var telAccount3 = TelAccountBuilder.New()
    .WithHash("")
    .WithApiId("")
    .WithPhone("")
    .WithFirstName("")
    .WithLastName("")
    .WithTwoFactorAuth("")
    .UseTestServers(true) //default to false
    .Build();

var telAccount4 = TelAccountBuilder.New().WithHash("").WithApiId("").WithPhone("").Build();

var accountHandler = await AccountBuilder
    .CreateAccounts()
    .FromAccount(telAccount1, () =>
    {
        Console.WriteLine("Enter The Code:");
        return Console.ReadLine()!;
    }, "acc1")
    .FromAccount(telAccount2, () =>
    {
        Console.WriteLine("Enter The Code:");
        return Console.ReadLine()!;
    }, "acc2")
    .FromAccount(builder => builder
                     .WithHash("")
                     .WithApiId("")
                     .WithPhone("")
                 , () => "", "acc3")
    .BuildAsync();


var wClient = accountHandler.ByName("acc1");
await wClient!.LoginUserIfNeeded();

var allClient = accountHandler.GetClients();


await accountHandler
    .WithDelay(TimeSpan.FromSeconds(2))
    .OnParallel()
    .ForEachClientAsync((client, name) =>
    {
        client.OnUpdate += u =>
        {
            Console.WriteLine($"got an update from {name}.");
            return Task.CompletedTask;
        };
        return Task.CompletedTask;
    });
Console.WriteLine("Hello, World!");