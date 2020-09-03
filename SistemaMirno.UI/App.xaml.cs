// <copyright file="App.xaml.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Windows;
using Autofac;

namespace SistemaMirno.UI
{
    /// <summary>
    /// Interaction logic for App.xaml.
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var container = Bootstrapper.Bootstrap();

            var mainWindow = container.Resolve<MainWindow>();
            mainWindow.ShowDialog();
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(
                "Ocurrio un error inesperado. Por favor informe al administrador de sistema." + Environment.NewLine +
                e.Exception.Message, "Error inesperado");
            e.Handled = true;
        }
    }
}