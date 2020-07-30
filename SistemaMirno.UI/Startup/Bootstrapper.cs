using Autofac;
using Prism.Events;
using SistemaMirno.DataAccess;
using SistemaMirno.Model;
using SistemaMirno.UI.Data;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.ViewModel;
using SistemaMirno.UI.ViewModel.Detail;
using SistemaMirno.UI.ViewModel.General;
using SistemaMirno.UI.ViewModel.Main;
using SistemaMirno.UI.Wrapper;

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
            builder.RegisterType<MaterialViewModel>().As<IMaterialViewModel>().SingleInstance();
            builder.RegisterType<ProductionAreasNavigationViewModel>().As<IProductionAreasNavigationViewModel>().SingleInstance();
            builder.RegisterType<WorkUnitViewModel>().As<IWorkUnitViewModel>().SingleInstance();
            builder.RegisterType<ColorViewModel>().As<IColorViewModel>().SingleInstance();
            builder.RegisterType<ProductViewModel>().As<IProductViewModel>().SingleInstance();
            builder.RegisterType<ProductionAreaViewModel>().As<IProductionAreaViewModel>().SingleInstance();

            builder.RegisterType<ProductionAreaDetailViewModel>().As<IProductionAreaDetailViewModel>().SingleInstance();

            builder.RegisterType<ProductionAreaRepository>().As<IProductionAreaRepository>();
            builder.RegisterType<WorkUnitDataService>().As<IWorkUnitDataService>();
            builder.RegisterType<MaterialDataService>().As<IMaterialDataService>();
            builder.RegisterType<ColorDataService>().As<IColorDataService>();
            builder.RegisterType<ProductDataService>().As<IProductDataService>();
            builder.RegisterType<ResponsibleDataService>().As<IResponsibleDataService>();
            builder.RegisterType<SupervisorDataService>().As<ISupervisorDataService>();

            builder.RegisterType<ProductionAreaWrapper>().AsSelf();

            builder.RegisterType<ProductionArea>().AsSelf();

            return builder.Build();
        }
    }
}
