namespace api.DTOs.Comment
{
    public sealed class CommentDTO
    {
        public int Id { get;  set; }

        public string Title { get;  set; } = String.Empty;
        public string Content { get;  set; } = String.Empty;

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public string CreatedBy { get; set; } = String.Empty;

        public int? StockId { get; set; }
    }
}
