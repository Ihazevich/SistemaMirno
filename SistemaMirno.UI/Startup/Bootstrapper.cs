using Autofac;
using Prism.Events;
using SistemaMirno.DataAccess;
using SistemaMirno.UI.Data;
using SistemaMirno.UI.ViewModel;

namespace SistemaMirno.UI.Startup
{
    public class Bootstrapper
    {
        public IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();

            builder.RegisterType<MirnoDbContext>().AsSelf();

            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<ProductionAreasViewModel>().As<IProductionAreasViewModel>();
            builder.RegisterType<WorkUnitViewModel>().As<IWorkUnitViewModel>();

            builder.RegisterType<AreaDataService>().As<IAreaDataService>();
            builder.RegisterType<WorkUnitDataService>().As<IWorkUnitDataService>();

            return builder.Build();
        }
    }
}
