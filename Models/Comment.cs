﻿namespace api.Models
{
    public sealed class Comment
    {
        
        public int Id { get; private set; }

        public string Title { get;  set; } = String.Empty;
        public string Content { get;  set; } = String.Empty;

        public DateTime CreatedOn { get;  set; } = DateTime.UtcNow;

        public int? StockId { get;  set; }

        //navigation property
        public Stock? Stock { get;  set; }

    }
}