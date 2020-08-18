﻿// <copyright file="Bootstrapper.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using Autofac;
using Prism.Events;
using SistemaMirno.DataAccess;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.View;
using SistemaMirno.UI.View.Services;
using SistemaMirno.UI.ViewModel;
using SistemaMirno.UI.ViewModel.Detail;
using SistemaMirno.UI.ViewModel.General;
using SistemaMirno.UI.ViewModel.Main;
using SistemaMirno.UI.ViewModel.Reports;

namespace SistemaMirno.UI.Startup
{
    /// <summary>
    /// A class represneting the Autofac boostrapper.
    /// </summary>
    public class Bootstrapper
    {
        /// <summary>
        /// Initializes the Autofac builder and registers all the relevant classes.
        /// </summary>
        /// <returns>An <see cref="IContainer"/> used by Autofac to resolve the dependencies.</returns>
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

            builder.RegisterType<LoginViewModel>()
                .Keyed<IViewModelBase>(nameof(LoginViewModel));

            builder.RegisterType<UserViewModel>()
                .Keyed<IViewModelBase>(nameof(UserViewModel));
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

            builder.RegisterType<InProcessByWorkAreasViewModel>()
                .Keyed<IViewModelBase>(nameof(InProcessByWorkAreasViewModel));
            builder.RegisterType<ProductionByWorkAreaViewModel>()
                .Keyed<IViewModelBase>(nameof(ProductionByWorkAreaViewModel));

            builder.RegisterType<StockViewModel>()
                .Keyed<IViewModelBase>(nameof(StockViewModel));

            builder.RegisterType<ClientViewModel>()
                .Keyed<IViewModelBase>(nameof(ClientViewModel));

            builder.RegisterType<UserDetailViewModel>().As<IUserDetailViewModel>();
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

            builder.RegisterType<ClientDetailViewModel>().As<IClientDetailViewModel>();

            builder.RegisterType<UserRepository>().As<IUserRepository>();
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

            builder.RegisterType<ClientRepository>().As<IClientRepository>();

            return builder.Build();
        }
    }
}