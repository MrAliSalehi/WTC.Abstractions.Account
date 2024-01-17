## WTelegramClient Account Handler
[![NuGet](https://img.shields.io/nuget/v/WTC.Abstractions.Account)](https://www.nuget.org/packages/WTC.Abstractions.Account)

this small project intents to make it easier to work with multiple telegram accounts inside the [WTelegramClient](https://github.com/wiz0u/WTelegramClient) library, it offers multiple ways of constructing accounts as well as a nice little `AccountBuilder`.

it also offers a `AccountHandler` which can handle all your accounts and have some extra functionality.

## use

simply install the library: `dotnet add package WTC.Abstractions.Account`

### Create Accounts

you can create accounts from different ways

- **from scratch:**
```csharp
var acc1 = new TelAccount("API_ID", "API_HASH", "PHONE");
var acc2 = new TelAccount("API_ID", "API_HASH", "PHONE", isUseTest: true);
```
*here you can also pass your `2FA` password or `firstName` & `lastName` if you want. they are optional.*

- **using builder:**

```csharp
var acc3 = TelAccountBuilder.New()
    .WithHash("")
    .WithApiId("")
    .WithPhone("")
    .WithFirstName("")
    .WithLastName("")
    .WithTwoFactorAuth("")
    .UseTestServers(true) //default to false
    .Build();
```
*note that some of there settings or optional, the required ones are:*
```csharp
var acc4 = TelAccountBuilder.New()
    .WithHash("")
    .WithApiId("")
    .WithPhone("")
    .Build();
```

after building a `TelAccount`, you need to create a `AccountHandler` to work with your Telegram Accounts.

### Create Account Handler

creating a handler might be a bit more difficult if you are not sure with this type of design, take a look at this:

```csharp
var accountHandler = await AccountBuilder
    .CreateAccounts()
    .FromAccount(acc1, () =>
    {
        Console.WriteLine("Enter The Code:");
        return Console.ReadLine()!;
    }, "acc1")
    .BuildAsync();
```

calling `.CreateAccounts()` gives us the opportunity to define every account that we have,

you can see the we are passing 3 arguments:
- the first one is your `TelAccount` structure that we defined earlier.
- second is a `Func` that is responsible for handling authentication from the code you receive from Telegram.
- third is your session file path AND the name of your client(the file name will be the name not the path), it defaults to `WTC.session`.

and after that we are doing `.BuildAsync()` which obviously builds the handler for us.
important thing to remember here is that the `BuildAsync` actually accepts a boolean that indicates whether we want to dispose the client **after login** or not.

```csharp
.BuildAsync(true);
//or
.BuildAsync((name, client) => {
    return name is "acc2";
})
```
you can also check for a specific condition on each client to decide if you want to dispose them or not..

you can chain multiple accounts and also, you can use the `AccountBuilder` inside the handler:

```csharp
var accountHandler = await AccountBuilder
    .CreateAccounts()
    .FromAccount(acc1, () =>
    {
        Console.WriteLine("Enter The Code:");
        return Console.ReadLine()!;
    }, "acc1")
    .FromAccount(builder => builder
                     .WithHash("")
                     .WithApiId("")
                     .WithPhone("")
                 , () => "", "acc2")
    .BuildAsync(true);
```
*notice how we use builder on the second account.*

now that you got your clients, you can access them: 

- by client's name
```csharp
var wClient = accountHandler.ByName("acc1");
```

-get all of them

```csharp
var allClients = accountHandler.GetClients();
```

- iterate over them:

```csharp
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
```
if you dont call `OnParallel` your iteration will NOT be parallelized.
and also the `WithDelay` call is optional.


*the complete example is available [here](https://github.com/MrAliSalehi/WTC.Abstractions.Account/blob/master/WTC.Abstractions.Account.Test/Program.cs).*

