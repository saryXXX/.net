namespace Backend.Models
{
    public interface IBaseEntity
    {
        bool IsDeleted { get; set; }
    }
}
