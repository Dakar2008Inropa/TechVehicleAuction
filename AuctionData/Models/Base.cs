namespace AuctionData.Models
{
    public abstract class Base
    {
        public int Id { get; set; }
        public BaseStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime DeletedAt { get; set; }

        protected Base()
        {
            Status = BaseStatus.Active;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            DeletedAt = DateTime.MinValue;
        }
    }
}