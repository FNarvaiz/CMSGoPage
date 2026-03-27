namespace backend.Models;

public class Site
{
    public Guid Id { get; set; }
    public Guid DeveloperId { get; set; }
    public User Developer { get; set; } = null!;
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Domain { get; set; }
    public string ApiKey { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ICollection<Section> Sections { get; set; } = [];
    public ICollection<ClientSiteAccess> ClientSiteAccesses { get; set; } = [];
    public ICollection<Media> MediaFiles { get; set; } = [];
}
