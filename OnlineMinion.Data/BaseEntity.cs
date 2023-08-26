using OnlineMinion.Contracts;

namespace OnlineMinion.Data;

public abstract class BaseEntity : IHasIntId
{
    public int Id { get; set; }
}
