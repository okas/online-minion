namespace OnlineMinion.Contracts;

public interface IQueryParams : IPagingInfo
{
    string? Filter { get; }

    string? Sort { get; }
}
