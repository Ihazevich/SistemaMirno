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
                new WorkArea { Name = "Pedidos" },
                new WorkArea { Name = "Lamina" },
                new WorkArea { Name = "Terciados" },
                new WorkArea { Name = "Prensa" },
                new WorkArea { Name = "Maquina" },
                new WorkArea { Name = "Perforacion" },
                new WorkArea { Name = "Lija" },
                new WorkArea { Name = "Filos" },
                new WorkArea { Name = "Banco" },
                new WorkArea { Name = "Lustre" },
                new WorkArea { Name = "Terminacion" },
                new WorkArea { Name = "Stock" }
                );
        }
    }
}