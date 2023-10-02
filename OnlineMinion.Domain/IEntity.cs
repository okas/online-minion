namespace OnlineMinion.Domain;

public interface IEntity<out TId>
    where TId : IId
{
    public TId Id { get; }
}
