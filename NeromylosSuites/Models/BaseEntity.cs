namespace NeromylosSuites.Models
{
    public class BaseEntity : IEntity
    {
        public int Id { get; set; }
        public DateTime InsertedAt { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
    }
}
