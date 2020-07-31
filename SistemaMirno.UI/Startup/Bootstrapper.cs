using Autofac;
using Prism.Events;
using SistemaMirno.DataAccess;
using SistemaMirno.Model;
using SistemaMirno.UI.Data;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.View.Services;
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

            builder.RegisterType<MessageDialogService>().As<IMessageDialogService>();

            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<ProductionAreasNavigationViewModel>().As<IProductionAreasNavigationViewModel>().SingleInstance();

            builder.RegisterType<ProductionAreaViewModel>().As<IProductionAreaViewModel>().SingleInstance();
            builder.RegisterType<MaterialViewModel>().As<IMaterialViewModel>().SingleInstance();
            builder.RegisterType<ColorViewModel>().As<IColorViewModel>().SingleInstance();
            builder.RegisterType<ProductCategoryViewModel>().As<IProductCategoryViewModel>().SingleInstance();
            builder.RegisterType<ProductViewModel>().As<IProductViewModel>().SingleInstance();
            builder.RegisterType<WorkUnitViewModel>().As<IWorkUnitViewModel>().SingleInstance();

            builder.RegisterType<ProductionAreaDetailViewModel>().As<IProductionAreaDetailViewModel>();
            builder.RegisterType<MaterialDetailViewModel>().As<IMaterialDetailViewModel>();
            builder.RegisterType<ColorDetailViewModel>().As<IColorDetailViewModel>();
            builder.RegisterType<ProductCategoryDetailViewModel>().As<IProductCategoryDetailViewModel>();
            builder.RegisterType<ProductDetailViewModel>().As<IProductDetailViewModel>();

            builder.RegisterType<ProductionAreaRepository>().As<IProductionAreaRepository>();
            builder.RegisterType<MaterialRepository>().As<IMaterialRepository>();
            builder.RegisterType<ColorRepository>().As<IColorRepository>();
            builder.RegisterType<ProductCategoryRepository>().As<IProductCategoryRepository>();
            builder.RegisterType<WorkUnitDataService>().As<IWorkUnitDataService>();
            builder.RegisterType<ProductRepository>().As<IProductRepository>();
            builder.RegisterType<ResponsibleDataService>().As<IResponsibleDataService>();
            builder.RegisterType<SupervisorDataService>().As<ISupervisorDataService>();

            return builder.Build();
        }
    }
}
