namespace backend.Models;

public class MediaVariant
{
    public Guid Id { get; set; }
    public Guid MediaId { get; set; }
    public Media Media { get; set; } = null!;
    public string VariantType { get; set; } = string.Empty;
    public string StoragePath { get; set; } = string.Empty;
    public int Width { get; set; }
    public int Height { get; set; }
    public long SizeBytes { get; set; }
    public string Format { get; set; } = "webp";
}
