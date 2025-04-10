using Microsoft.EntityFrameworkCore;

namespace Presentation.Seeds;

public interface ISeeder
{
    static abstract void Seed(DbContext context);
    static abstract Task SeedAsync(DbContext context, CancellationToken ct = default);
}