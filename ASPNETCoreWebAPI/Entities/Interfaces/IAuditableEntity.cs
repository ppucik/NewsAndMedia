namespace ASPNETCoreWebAPI.Entities.Interfaces;

public interface IAuditableEntity
{
    DateTime Created { get; set; }

    DateTime? Modified { get; set; }
}
