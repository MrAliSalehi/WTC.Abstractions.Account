namespace WTC.Abstractions.Account.Interfaces;

public interface IApiHash
{
    IApiId WithHash(string hash);
}