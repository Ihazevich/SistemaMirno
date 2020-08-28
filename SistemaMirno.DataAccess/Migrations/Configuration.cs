using System.Data.Entity.Migrations;
using SistemaMirno.Model;

namespace SistemaMirno.DataAccess.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<SistemaMirno.DataAccess.MirnoDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(SistemaMirno.DataAccess.MirnoDbContext context)
        {
        }
    }
}