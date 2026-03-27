namespace backend.Models;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = "Developer";
    public bool IsApproved { get; set; }
    public Guid? CreatedById { get; set; }
    public User? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ICollection<Site> Sites { get; set; } = [];
    public ICollection<ClientSiteAccess> ClientSiteAccesses { get; set; } = [];
    public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
}
