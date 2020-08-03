using Autofac;
using Prism.Events;
using SistemaMirno.DataAccess;
using SistemaMirno.UI.Data;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.View.Services;
using SistemaMirno.UI.ViewModel;
using SistemaMirno.UI.ViewModel.Detail;
using SistemaMirno.UI.ViewModel.General;
using SistemaMirno.UI.ViewModel.Main;

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
            builder.RegisterType<WorkAreaNavigationViewModel>().As<IWorkAreaNavigationViewModel>().SingleInstance();

            builder.RegisterType<WorkAreaViewModel>().As<IWorkAreaViewModel>().SingleInstance();
            builder.RegisterType<MaterialViewModel>().As<IMaterialViewModel>().SingleInstance();
            builder.RegisterType<ColorViewModel>().As<IColorViewModel>().SingleInstance();
            builder.RegisterType<ProductCategoryViewModel>().As<IProductCategoryViewModel>().SingleInstance();
            builder.RegisterType<ProductViewModel>().As<IProductViewModel>().SingleInstance();
            builder.RegisterType<EmployeeViewModel>().As<IEmployeeViewModel>().SingleInstance();
            builder.RegisterType<EmployeeRoleViewModel>().As<IEmployeeRoleViewModel>().SingleInstance();
            builder.RegisterType<WorkUnitViewModel>().As<IWorkUnitViewModel>().SingleInstance();
            builder.RegisterType<WorkOrderViewModel>().As<IWorkOrderViewModel>().SingleInstance();

            builder.RegisterType<WorkAreaDetailViewModel>().As<IWorkAreaDetailViewModel>();
            builder.RegisterType<MaterialDetailViewModel>().As<IMaterialDetailViewModel>();
            builder.RegisterType<ColorDetailViewModel>().As<IColorDetailViewModel>();
            builder.RegisterType<ProductCategoryDetailViewModel>().As<IProductCategoryDetailViewModel>();
            builder.RegisterType<ProductDetailViewModel>().As<IProductDetailViewModel>();
            builder.RegisterType<EmployeeDetailViewModel>().As<IEmployeeDetailViewModel>();
            builder.RegisterType<EmployeeRoleDetailViewModel>().As<IEmployeeRoleDetailViewModel>();
            builder.RegisterType<WorkOrderDetailViewModel>().As<IWorkOrderDetailViewModel>();

            builder.RegisterType<WorkAreaRepository>().As<IWorkAreaRepository>();
            builder.RegisterType<MaterialRepository>().As<IMaterialRepository>();
            builder.RegisterType<ColorRepository>().As<IColorRepository>();
            builder.RegisterType<ProductCategoryRepository>().As<IProductCategoryRepository>();
            builder.RegisterType<ProductRepository>().As<IProductRepository>();
            builder.RegisterType<EmployeeRepository>().As<IEmployeeRepository>();
            builder.RegisterType<EmployeeRoleRepository>().As<IEmployeeRoleRepository>();
            builder.RegisterType<WorkUnitRepository>().As<IWorkUnitRepository>();
            builder.RegisterType<WorkOrderRepository>().As<IWorkOrderRepository>();

            return builder.Build();
        }
    }
}