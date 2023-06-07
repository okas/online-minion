using OnlineMinion.Contracts;

namespace OnlineMinion.Data.BaseEntities;

public abstract class BaseEntity : IHasIntId
{
    public int Id { get; set; }
}
