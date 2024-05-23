using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPNETCoreWebAPI.Entities;

/// <summary>
/// Audit log
/// </summary>
[Table(nameof(AuditLog))]
public class AuditLog
{
    [Key]
    public long Id { get; set; } // Primary key

    public string User => "System-User"; // Authorization

    public required string EntityType { get; set; }

    public required string Action { get; set; }

    public DateTime TimeStamp { get; set; }

    public required string Changes { get; set; }
}
