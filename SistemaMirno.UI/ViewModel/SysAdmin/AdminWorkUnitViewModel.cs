using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FileHelpers;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.FileHelpers;
using SistemaMirno.UI.Data.Repositories.Interfaces;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.ViewModel.Detail;
using SistemaMirno.UI.ViewModel.General;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.SysAdmin
{
    public class AdminWorkUnitViewModel : ViewModelBase
    {
        private IWorkUnitRepository _workUnitRepository;
        private ProductWrapper _selectedProduct;

        public AdminWorkUnitViewModel(
            IWorkUnitRepository workUnitRepository,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Administracion Unidades de Trabajo", dialogCoordinator)
        {
            _workUnitRepository = workUnitRepository;

            ImportFromFileCommand = new DelegateCommand(OnImportFromFileExecute);
        }

        public ICommand ImportFromFileCommand { get; }

        public override async Task LoadAsync(int? id = null)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                ProgressVisibility = Visibility.Collapsed;
                ViewVisibility = Visibility.Visible;
            });
        }

        private async void OnImportFromFileExecute()
        {
            // Configure open file dialog box
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                InitialDirectory = Directory.GetCurrentDirectory(),
                DefaultExt = ".csv",
                Filter = "CSV Files(.csv)|*.csv",
            };

            // Show open file dialog box
            var result = dialog.ShowDialog();

            var workUnitsAdded = 0;
            var workUnitsDiscarded = 0;

            // Process open file dialog box results
            if (result != true)
            {
                return;
            }

            Application.Current.Dispatcher.Invoke(() => ProgressVisibility = Visibility.Visible);

            // Open document
            string filename = dialog.FileName;

            // Create FileHelpers Engine
            var engine = new FileHelperEngine<WorkUnitFileHelper>();

            // Collection of processed products
            var workUnits = new List<WorkUnit>();

            try
            {
                // Read Use:
                var data = engine.ReadFile(filename);

                foreach (WorkUnitFileHelper workUnit in data)
                {
                    // Search the product
                    var productId = await _workUnitRepository.FindProductByNameAsync(workUnit.Product);

                    // Search the material
                    var materialId = await _workUnitRepository.FindMaterialByNameAsync(workUnit.Material);

                    // Search the color
                    var colorId = await _workUnitRepository.FindColorByNameAsync(workUnit.Color);

                    // Search the area
                    var workAreaId =
                        await _workUnitRepository.FindWorkAreaByNameAndBranchNameAsync(workUnit.CurrentArea,
                            workUnit.Branch);

                    // Create a new model 
                    var newWorkUnit = new WorkUnit
                    {
                        ProductId = productId,
                        MaterialId = materialId,
                        ColorId = colorId,
                        Sold = false,
                        Delivered = false,
                        CurrentWorkAreaId = workAreaId,
                        LatestResponsibleId = SessionInfo.User.EmployeeId,
                        LatestSupervisorId = SessionInfo.User.EmployeeId,
                        Details = string.Empty,
                    };

                    workUnits.Add(newWorkUnit);
                    workUnitsAdded++;
                }

                // After all products added to the context, save to the database
                await _workUnitRepository.AddRangeAsync(workUnits);

                Application.Current.Dispatcher.Invoke(() => ProgressVisibility = Visibility.Collapsed);

                // Show a messagebox with the results
                EventAggregator.GetEvent<ShowDialogEvent>()
                    .Publish(new ShowDialogEventArgs
                    {
                        Title = "Proceso completado",
                        Message = $"Registros nuevos: {workUnitsAdded} | Registros descartados: {workUnitsDiscarded}",
                    });
                EventAggregator.GetEvent<ChangeViewEvent>()
                    .Publish(new ChangeViewEventArgs
                    {
                        ViewModel = nameof(ProductViewModel),
                    });
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() => ProgressVisibility = Visibility.Collapsed);
                EventAggregator.GetEvent<ShowDialogEvent>()
                    .Publish(new ShowDialogEventArgs
                    {
                        Title = "Error",
                        Message = $"Error [{ex.Message}]. Contacte al Administrador de Sistema.",
                    });
                EventAggregator.GetEvent<ChangeViewEvent>()
                    .Publish(new ChangeViewEventArgs
                    {
                        ViewModel = nameof(ProductViewModel),
                    });
            }
        }
    }
}
