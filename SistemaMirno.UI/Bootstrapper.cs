// <copyright file="Bootstrapper.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Security.Principal;
using Autofac;
using MahApps.Metro.Controls.Dialogs;
using Prism.Events;
using SistemaMirno.DataAccess;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.Data.Repositories.Interfaces;
using SistemaMirno.UI.ViewModel;
using SistemaMirno.UI.ViewModel.Detail;
using SistemaMirno.UI.ViewModel.Detail.Interfaces;
using SistemaMirno.UI.ViewModel.General;
using SistemaMirno.UI.ViewModel.Main;
using SistemaMirno.UI.ViewModel.Reports;

namespace SistemaMirno.UI
{
    /// <summary>
    /// A class represneting the Autofac boostrapper.
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

            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<WorkAreaNavigationViewModel>()
                .Keyed<IViewModelBase>(nameof(WorkAreaNavigationViewModel));
            builder.RegisterType<LoginViewModel>()
                .Keyed<IViewModelBase>(nameof(LoginViewModel));
            builder.RegisterType<BranchSelectionViewModel>()
                .Keyed<IViewModelBase>(nameof(BranchSelectionViewModel));

            builder.RegisterType<UserViewModel>()
                .Keyed<IViewModelBase>(nameof(UserViewModel));
            builder.RegisterType<BranchViewModel>()
                .Keyed<IViewModelBase>(nameof(BranchViewModel));
            builder.RegisterType<RoleViewModel>()
                .Keyed<IViewModelBase>(nameof(RoleViewModel));
            builder.RegisterType<EmployeeViewModel>()
                .Keyed<IViewModelBase>(nameof(EmployeeViewModel));

            builder.RegisterType<InProcessByWorkAreasViewModel>()
                .Keyed<IViewModelBase>(nameof(InProcessByWorkAreasViewModel));
            builder.RegisterType<ProductionByWorkAreaViewModel>()
                .Keyed<IViewModelBase>(nameof(ProductionByWorkAreaViewModel));


            builder.RegisterType<UserDetailViewModel>().As<IUserDetailViewModel>();
            builder.RegisterType<BranchDetailViewModel>().As<IBranchDetailViewModel>();
            builder.RegisterType<RoleDetailViewModel>().As<IRoleDetailViewModel>();
            builder.RegisterType<EmployeeDetailViewModel>().As<IEmployeeDetailViewModel>()
                .Keyed<IViewModelBase>(nameof(EmployeeDetailViewModel));

            builder.RegisterType<UserRepository>().As<IUserRepository>();
            builder.RegisterType<BranchRepository>().As<IBranchRepository>();
            builder.RegisterType<RoleRepository>().As<IRoleRepository>();
            builder.RegisterType<EmployeeRepository>().As<IEmployeeRepository>();


            return builder.Build();
        }
    }
}