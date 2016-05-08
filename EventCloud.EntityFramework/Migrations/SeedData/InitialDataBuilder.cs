using EventCloud.EntityFramework;
using EntityFramework.DynamicFilters;

namespace EventCloud.Migrations.SeedData
{
    public class InitialDataBuilder
    {
        private readonly EventCloudDbContext _context;

        public InitialDataBuilder(EventCloudDbContext context)
        {
            _context = context;
        }

        public void Build()
        {
            _context.DisableAllFilters();

            new DefaultEditionsBuilder(_context).Build();
            new DefaultTenantRoleAndUserBuilder(_context).Build();
            new DefaultLanguagesBuilder(_context).Build();
        }
    }
}
