using Microsoft.EntityFrameworkCore;

namespace ASPNETCoreWebAPI.Repositories;

public static class AppDbContextSeed
{
    public static IHost SeedData(this IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            context.Database.MigrateAsync().GetAwaiter().GetResult();

            // insert data ...
        }

        return host;
    }
}
