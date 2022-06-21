namespace Template.Domain.Entities.Identity;

public class IdentityTokenEntity : ATAEntity
{
    public new int Id { get; set; }

    public string? ClientName { get; set; }

    public DateTimeOffset ExpiresAt { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset ModifiedAt { get; set; }

    public int UserId { get; set; }

    public string? IPAddress { get; set; }

    public string? DeviceName { get; set; }

    public override string ToString()
    {
        return $"{nameof(ClientName)}: {ClientName}, UserId: {UserId}";
    }
}