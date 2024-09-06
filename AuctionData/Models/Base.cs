using System;

namespace AuctionData.Models
{
    public abstract class Base
    {
        public string Id { get; set; }
        public BaseStatus Status { get; set; }
        private DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime DeletedAt { get; set; }
        protected Base()
        {
            Id = Guid.NewGuid().ToString("N");
            Status = BaseStatus.Active;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            DeletedAt = DateTime.MinValue;
        }
    }
}