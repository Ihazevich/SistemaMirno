using Autofac;
using SistemaMirno.UI.Startup;
using System;
using System.Windows;

namespace SistemaMirno.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var bootstrapper = new Bootstrapper();
            var container = bootstrapper.Bootstrap();

            var mainWindow = container.Resolve<MainWindow>();
            mainWindow.Show();
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("Ocurrio un error inesperado. Por favor informe al administrador de sistema." + Environment.NewLine +
                e.Exception.Message, "Error inesperado");
            e.Handled = true;
        }
    }
}