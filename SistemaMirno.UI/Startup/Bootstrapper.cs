﻿using Autofac;
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

            builder.RegisterType<MirnoDbContext>().AsSelf().SingleInstance();

            builder.RegisterType<MainWindow>().AsSelf();

            builder.RegisterType<MessageDialogService>().As<IMessageDialogService>();

            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<WorkAreaNavigationViewModel>()
                .Keyed<IViewModelBase>(nameof(WorkAreaNavigationViewModel));

            builder.RegisterType<AreaConnectionViewModel>()
                .Keyed<IViewModelBase>(nameof(AreaConnectionViewModel));
            builder.RegisterType<WorkAreaViewModel>()
                .Keyed<IViewModelBase>(nameof(WorkAreaViewModel));
            builder.RegisterType<MaterialViewModel>()
                .Keyed<IViewModelBase>(nameof(MaterialViewModel));
            builder.RegisterType<ColorViewModel>()
                .Keyed<IViewModelBase>(nameof(ColorViewModel));
            builder.RegisterType<ProductCategoryViewModel>()
                .Keyed<IViewModelBase>(nameof(ProductCategoryViewModel));
            builder.RegisterType<ProductViewModel>()
                .Keyed<IViewModelBase>(nameof(ProductViewModel));
            builder.RegisterType<EmployeeViewModel>()
                .Keyed<IViewModelBase>(nameof(EmployeeViewModel));
            builder.RegisterType<EmployeeRoleViewModel>()
                .Keyed<IViewModelBase>(nameof(EmployeeRoleViewModel));
            builder.RegisterType<WorkUnitViewModel>()
                .Keyed<IViewModelBase>(nameof(WorkUnitViewModel));
            builder.RegisterType<WorkOrderViewModel>()
                .Keyed<IViewModelBase>(nameof(WorkOrderViewModel));

            builder.RegisterType<AreaConnectionDetailViewModel>().As<IAreaConnectionDetailViewModel>();
            builder.RegisterType<WorkAreaDetailViewModel>().As<IWorkAreaDetailViewModel>();
            builder.RegisterType<MaterialDetailViewModel>().As<IMaterialDetailViewModel>();
            builder.RegisterType<ColorDetailViewModel>().As<IColorDetailViewModel>();
            builder.RegisterType<ProductCategoryDetailViewModel>().As<IProductCategoryDetailViewModel>();
            builder.RegisterType<ProductDetailViewModel>().As<IProductDetailViewModel>();
            builder.RegisterType<EmployeeDetailViewModel>().As<IEmployeeDetailViewModel>();
            builder.RegisterType<EmployeeRoleDetailViewModel>().As<IEmployeeRoleDetailViewModel>();
            builder.RegisterType<WorkOrderDetailViewModel>().As<IWorkOrderDetailViewModel>()
                .Keyed<IViewModelBase>(nameof(WorkOrderDetailViewModel));

            builder.RegisterType<AreaConnectionRepository>().As<IAreaConnectionRepository>();
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