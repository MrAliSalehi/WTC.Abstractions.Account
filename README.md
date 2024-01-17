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



*complete example is available [here](https://github.com/MrAliSalehi/WTC.Abstractions.Account/blob/master/WTC.Abstractions.Account.Test/Program.cs).*

