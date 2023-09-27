using OnlineMinion.Contracts;

namespace OnlineMinion.Domain;

public abstract class BaseEntity : IHasIntId
{
    public int Id { get; set; }
}
