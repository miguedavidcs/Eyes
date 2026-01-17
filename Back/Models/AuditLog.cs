using Back.Models;

public class AuditLog : BaseEntity
{
    public Guid? UserId { get; set; }

    public string Action { get; set; } = null!;   // POST / PUT / DELETE
    public string Endpoint { get; set; } = null!;
    public int StatusCode { get; set; }

    public string? RequestBody { get; set; }
    public string? IpAddress { get; set; }

}
