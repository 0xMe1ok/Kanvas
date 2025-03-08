using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public abstract class EntityBase<T>
{
    [Key]
    public T Id { get; set; }
}