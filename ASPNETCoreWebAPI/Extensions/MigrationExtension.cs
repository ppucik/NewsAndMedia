using ASPNETCoreWebAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ASPNETCoreWebAPI.Extensions;

public static class MigrationExtension
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        using AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        context.Database.Migrate();
    }
}
