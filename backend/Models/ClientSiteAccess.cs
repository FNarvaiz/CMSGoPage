namespace backend.Models;

public class ClientSiteAccess
{
    public Guid ClientId { get; set; }
    public User Client { get; set; } = null!;
    public Guid SiteId { get; set; }
    public Site Site { get; set; } = null!;
    public DateTime AssignedAt { get; set; }
}
