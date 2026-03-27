namespace backend.Models;

public class Media
{
    public Guid Id { get; set; }
    public Guid UploadedById { get; set; }
    public User UploadedBy { get; set; } = null!;
    public Guid SiteId { get; set; }
    public Site Site { get; set; } = null!;
    public string FileName { get; set; } = string.Empty;
    public string OriginalPath { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long SizeBytes { get; set; }
    public bool IsProcessed { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<MediaVariant> Variants { get; set; } = [];
}
