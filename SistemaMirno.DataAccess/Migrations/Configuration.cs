namespace SistemaMirno.DataAccess.Migrations
{
    using SistemaMirno.Model;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<SistemaMirno.DataAccess.MirnoDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SistemaMirno.DataAccess.MirnoDbContext context)
        {
            context.ProductionAreas.AddOrUpdate(
                a => a.Name,
                new ProductionArea { Name = "Pedidos" },
                new ProductionArea { Name = "Lamina" },
                new ProductionArea { Name = "Terciados" },
                new ProductionArea { Name = "Prensa" },
                new ProductionArea { Name = "Maquina" },
                new ProductionArea { Name = "Perforacion" },
                new ProductionArea { Name = "Lija" },
                new ProductionArea { Name = "Filos" },
                new ProductionArea { Name = "Banco" },
                new ProductionArea { Name = "Lustre" },
                new ProductionArea { Name = "Terminacion" },
                new ProductionArea { Name = "Stock" }
                );
        }
    }
}