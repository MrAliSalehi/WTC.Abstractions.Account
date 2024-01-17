namespace WTC.Abstractions.Account.Interfaces;

public interface IPhoneNumber
{
    IOptionalInfo WithPhone(string phone);
}