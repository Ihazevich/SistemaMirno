using Autofac;
using SistemaMirno.UI.Data;
using SistemaMirno.UI.ViewModel;

namespace SistemaMirno.UI.Startup
{
    public class Bootstrapper
    {
        public IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<AreaDataService>().As<IDataService>();

            return builder.Build();
        }
    }
}
