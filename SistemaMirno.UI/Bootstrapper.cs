// <copyright file="Bootstrapper.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Security.Principal;
using System.Windows.Shell;
using Autofac;
using MahApps.Metro.Controls.Dialogs;
using Prism.Events;
using SistemaMirno.DataAccess;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.Data.Repositories.Interfaces;
using SistemaMirno.UI.View;
using SistemaMirno.UI.ViewModel;
using SistemaMirno.UI.ViewModel.Detail;
using SistemaMirno.UI.ViewModel.Detail.Interfaces;
using SistemaMirno.UI.ViewModel.General;
using SistemaMirno.UI.ViewModel.Main;
using SistemaMirno.UI.ViewModel.Reports;

namespace SistemaMirno.UI
{
    /// <summary>
    /// A class representing the Autofac boostrapper.
    /// </summary>
    public static class Bootstrapper
    {
        /// <summary>
        /// Initializes the Autofac builder and registers all the relevant classes.
        /// </summary>
        /// <returns>An <see cref="IContainer"/> used by Autofac to resolve the dependencies.</returns>
        public static IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<DialogCoordinator>().As<IDialogCoordinator>().SingleInstance();
            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();

            builder.RegisterType<MirnoDbContext>().AsSelf();

            builder.RegisterType<MainWindow>().AsSelf();

            RegisterMainViewModels(builder);
            RegisterGeneralViewModels(builder);
            RegisterReportViewModels(builder);
            RegisterDetailViewModels(builder);
            RegisterRepositories(builder);

            return builder.Build();
        }

        /// <summary>
        /// Registers all main view models with autofac.
        /// </summary>
        /// <param name="builder">The autofac builder.</param>
        private static void RegisterMainViewModels(ContainerBuilder builder)
        {
            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<NavigationViewModel>()
                .Keyed<IViewModelBase>(nameof(NavigationViewModel));
            builder.RegisterType<LoginViewModel>()
                .Keyed<IViewModelBase>(nameof(LoginViewModel));
            builder.RegisterType<BranchSelectionViewModel>()
                .Keyed<IViewModelBase>(nameof(BranchSelectionViewModel));
        }

        /// <summary>
        /// Registers all general view models with autofac.
        /// </summary>
        /// <param name="builder">The autofac builder.</param>
        private static void RegisterGeneralViewModels(ContainerBuilder builder)
        {
            builder.RegisterType<UserViewModel>()
                .Keyed<IViewModelBase>(nameof(UserViewModel));
            builder.RegisterType<BranchViewModel>()
                .Keyed<IViewModelBase>(nameof(BranchViewModel));
            builder.RegisterType<RoleViewModel>()
                .Keyed<IViewModelBase>(nameof(RoleViewModel));
            builder.RegisterType<EmployeeViewModel>()
                .Keyed<IViewModelBase>(nameof(EmployeeViewModel));
            builder.RegisterType<WorkAreaViewModel>()
                .Keyed<IViewModelBase>(nameof(WorkAreaViewModel));
            builder.RegisterType<ProductViewModel>()
                .Keyed<IViewModelBase>(nameof(ProductViewModel));
            builder.RegisterType<MaterialViewModel>()
                .Keyed<IViewModelBase>(nameof(MaterialViewModel));
            builder.RegisterType<ColorViewModel>()
                .Keyed<IViewModelBase>(nameof(ColorViewModel));
            builder.RegisterType<ProductCategoryViewModel>()
                .Keyed<IViewModelBase>(nameof(ProductCategoryViewModel));
            builder.RegisterType<WorkUnitViewModel>()
                .Keyed<IViewModelBase>(nameof(WorkUnitViewModel));
            builder.RegisterType<ClientViewModel>()
                .Keyed<IViewModelBase>(nameof(ClientViewModel));
            builder.RegisterType<RequisitionViewModel>()
                .Keyed<IViewModelBase>(nameof(RequisitionViewModel));
            builder.RegisterType<StockViewModel>()
                .Keyed<IViewModelBase>(nameof(StockViewModel));
        }

        /// <summary>
        /// Registers all report view models with autofac.
        /// </summary>
        /// <param name="builder">The autofac builder.</param>
        private static void RegisterReportViewModels(ContainerBuilder builder)
        {
            builder.RegisterType<InProcessByWorkAreasViewModel>()
                .Keyed<IViewModelBase>(nameof(InProcessByWorkAreasViewModel));
            builder.RegisterType<ProductionByWorkAreaViewModel>()
                .Keyed<IViewModelBase>(nameof(ProductionByWorkAreaViewModel));
        }

        /// <summary>
        /// Registers all detail view models with autofac.
        /// </summary>
        /// <param name="builder">The autofac builder.</param>
        private static void RegisterDetailViewModels(ContainerBuilder builder)
        {
            builder.RegisterType<UserDetailViewModel>().As<IUserDetailViewModel>()
                .Keyed<IViewModelBase>(nameof(UserDetailViewModel));
            builder.RegisterType<BranchDetailViewModel>().As<IBranchDetailViewModel>()
                .Keyed<IViewModelBase>(nameof(BranchDetailViewModel));
            builder.RegisterType<RoleDetailViewModel>().As<IRoleDetailViewModel>()
                .Keyed<IViewModelBase>(nameof(RoleDetailViewModel));
            builder.RegisterType<EmployeeDetailViewModel>().As<IEmployeeDetailViewModel>()
                .Keyed<IViewModelBase>(nameof(EmployeeDetailViewModel));
            builder.RegisterType<WorkAreaDetailViewModel>().As<IWorkAreaDetailViewModel>()
                .Keyed<IViewModelBase>(nameof(WorkAreaDetailViewModel));
            builder.RegisterType<ProductDetailViewModel>().As<IProductDetailViewModel>()
                .Keyed<IViewModelBase>(nameof(ProductDetailViewModel));
            builder.RegisterType<MaterialDetailViewModel>().As<IMaterialDetailViewModel>()
                .Keyed<IViewModelBase>(nameof(MaterialDetailViewModel));
            builder.RegisterType<ColorDetailViewModel>().As<IColorDetailViewModel>()
                .Keyed<IViewModelBase>(nameof(ColorDetailViewModel));
            builder.RegisterType<ProductCategoryDetailViewModel>().As<IProductCategoryDetailViewModel>()
                .Keyed<IViewModelBase>(nameof(ProductCategoryDetailViewModel));
            builder.RegisterType<WorkOrderDetailViewModel>().As<IWorkOrderDetailViewModel>()
                .Keyed<IViewModelBase>(nameof(WorkOrderDetailViewModel));
            builder.RegisterType<ClientDetailViewModel>().As<IClientDetailViewModel>()
                .Keyed<IViewModelBase>(nameof(ClientDetailViewModel));
            builder.RegisterType<RequisitionDetailViewModel>().As<IRequisitionDetailViewModel>()
                .Keyed<IViewModelBase>(nameof(RequisitionDetailViewModel));
            builder.RegisterType<WorkOrderDetailViewModel>().As<IWorkOrderDetailViewModel>()
                .Keyed<IViewModelBase>(nameof(WorkOrderDetailViewModel));
        }

        /// <summary>
        /// Registers all repositories with autofac.
        /// </summary>
        /// <param name="builder">The autofac builder.</param>
        private static void RegisterRepositories(ContainerBuilder builder)
        {
            builder.RegisterType<UserRepository>().As<IUserRepository>();
            builder.RegisterType<BranchRepository>().As<IBranchRepository>();
            builder.RegisterType<RoleRepository>().As<IRoleRepository>();
            builder.RegisterType<EmployeeRepository>().As<IEmployeeRepository>();
            builder.RegisterType<WorkAreaRepository>().As<IWorkAreaRepository>();
            builder.RegisterType<ProductRepository>().As<IProductRepository>();
            builder.RegisterType<MaterialRepository>().As<IMaterialRepository>();
            builder.RegisterType<ColorRepository>().As<IColorRepository>();
            builder.RegisterType<ProductCategoryRepository>().As<IProductCategoryRepository>();
            builder.RegisterType<ClientRepository>().As<IClientRepository>();
            builder.RegisterType<RequisitionRepository>().As<IRequisitionRepository>();
            builder.RegisterType<WorkOrderRepository>().As<IWorkOrderRepository>();
            builder.RegisterType<WorkUnitRepository>().As<IWorkUnitRepository>();
        }
    }
}