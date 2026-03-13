namespace AuthService.Core.Entities.Common;

public interface IEntity<out TKey>
{
    TKey Id { get; }
}
