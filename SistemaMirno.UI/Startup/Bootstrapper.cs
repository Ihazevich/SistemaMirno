﻿using Autofac;
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
            builder.RegisterType<MaterialViewModel>().As<IMaterialViewModel>().SingleInstance();
            builder.RegisterType<ProductionAreasNavigationViewModel>().As<IProductionAreasNavigationViewModel>().SingleInstance();
            builder.RegisterType<WorkUnitViewModel>().As<IWorkUnitViewModel>().SingleInstance();
            builder.RegisterType<ColorViewModel>().As<IColorViewModel>().SingleInstance();
            builder.RegisterType<ProductViewModel>().As<IProductViewModel>().SingleInstance();
            builder.RegisterType<ProductionAreaViewModel>().As<IProductionAreaViewModel>().SingleInstance();


            builder.RegisterType<ProductionAreaDataService>().As<IProductionAreaDataService>();
            builder.RegisterType<WorkUnitDataService>().As<IWorkUnitDataService>();
            builder.RegisterType<MaterialDataService>().As<IMaterialDataService>();
            builder.RegisterType<ColorDataService>().As<IColorDataService>();
            builder.RegisterType<ProductDataService>().As<IProductDataService>();
            builder.RegisterType<ResponsibleDataService>().As<IResponsibleDataService>();
            builder.RegisterType<SupervisorDataService>().As<ISupervisorDataService>();

            return builder.Build();
        }
    }
}
