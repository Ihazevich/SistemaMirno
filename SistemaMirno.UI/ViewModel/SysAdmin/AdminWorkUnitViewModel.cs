using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
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
        private WorkUnitWrapper _workUnit;
        private Product _selectedProduct;
        private Material _selectedMaterial;
        private Color _selectedColor;
        private WorkArea _selectedWorkArea;

        private string _quantity;

        public AdminWorkUnitViewModel(
            IWorkUnitRepository workUnitRepository,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Administracion Unidades de Trabajo", dialogCoordinator)
        {
            _workUnitRepository = workUnitRepository;

            Products = new ObservableCollection<Product>();
            Materials = new ObservableCollection<Material>();
            Colors = new ObservableCollection<Color>();
            WorkAreas = new ObservableCollection<WorkArea>();

            AddNewWorkUnitCommand = new DelegateCommand(OnAddNewWorkUnitExecute, OnAddNewWorkUnitCanExecute);
        }

        private bool OnAddNewWorkUnitCanExecute()
        {
            return NewWorkUnit != null && !NewWorkUnit.HasErrors && int.TryParse(Quantity, out int quantity) && quantity > 0;
        }

        private async void OnAddNewWorkUnitExecute()
        {
            Application.Current.Dispatcher.Invoke(() => ProgressVisibility = Visibility.Visible);

            var newWorkUnits = new List<WorkUnit>();
            
            for (var i = 0; i < int.Parse(Quantity); i++)
            {
                var workUnit = new WorkUnit
                {
                    CreationDate = DateTime.Now,
                    LatestResponsibleId = SessionInfo.User.EmployeeId,
                    LatestSupervisorId = SessionInfo.User.EmployeeId,
                    Delivered = false,
                    Sold = false,
                    TotalWorkTime = 0,
                    ProductId = NewWorkUnit.ProductId,
                    MaterialId = NewWorkUnit.MaterialId,
                    ColorId = NewWorkUnit.ColorId,
                    CurrentWorkAreaId = NewWorkUnit.CurrentWorkAreaId,
                };
                newWorkUnits.Add(workUnit);
            }

            await _workUnitRepository.AddRangeAsync(newWorkUnits);

            Application.Current.Dispatcher.Invoke(() => ProgressVisibility = Visibility.Collapsed);
        }

        public ObservableCollection<Product> Products { get; }

        public ObservableCollection<Material> Materials { get; }

        public ObservableCollection<Color> Colors { get; }

        public ObservableCollection<WorkArea> WorkAreas { get; }

        public ICommand AddNewWorkUnitCommand { get; }

        public string Quantity
        {
            get => _quantity;

            set
            {
                _quantity = value;
                OnPropertyChanged();
            }
        }

        public WorkUnitWrapper NewWorkUnit
        {
            get => _workUnit;

            set
            {
                _workUnit = value;
                OnPropertyChanged();
            }
        }

        public Product SelectedProduct
        {
            get => _selectedProduct;

            set
            {
                _selectedProduct = value;
                OnPropertyChanged();
            }
        }

        public Material SelectedMaterial
        {
            get => _selectedMaterial;

            set
            {
                _selectedMaterial= value;
                OnPropertyChanged();
            }
        }

        public Color SelectedColor
        {
            get => _selectedColor;

            set
            {
                _selectedColor = value;
                OnPropertyChanged();
            }
        }

        public WorkArea SelectedWorkArea
        {
            get => _selectedWorkArea;

            set
            {
                _selectedWorkArea = value;
                OnPropertyChanged();
            }
        }

        public override async Task LoadAsync(int? id = null)
        {
            await LoadProducts();
            await LoadColors();
            await LoadMaterials();
            await LoadWorkAreas();

            Application.Current.Dispatcher.Invoke(() =>
            {
                NewWorkUnit = new WorkUnitWrapper();
                NewWorkUnit.PropertyChanged += Model_PropertyChanged;

                NewWorkUnit.CurrentWorkAreaId = 0;
                NewWorkUnit.ProductId = 0;
                NewWorkUnit.MaterialId = 0;
                NewWorkUnit.ColorId = 0;

                ProgressVisibility = Visibility.Collapsed;
                ViewVisibility = Visibility.Visible;
            });
        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

            if (e.PropertyName == nameof(NewWorkUnit.HasErrors))
            {
                ((DelegateCommand)AddNewWorkUnitCommand).RaiseCanExecuteChanged();
            }
        }

        private async Task LoadWorkAreas()
        {
            var workAreas = await _workUnitRepository.GetAllWorkAreasAsync();

            foreach (var workArea in workAreas)
            {
                Application.Current.Dispatcher.Invoke(() => WorkAreas.Add(workArea));
            }
        }

        private async Task LoadMaterials()
        {
            var materials = await _workUnitRepository.GetAllMaterialsAsync();

            foreach (var material in materials)
            {
                Application.Current.Dispatcher.Invoke(() => Materials.Add(material));
            }
        }

        private async Task LoadColors()
        {
            var colors = await _workUnitRepository.GetAllColorsAsync();

            foreach (var color in colors)
            {
                Application.Current.Dispatcher.Invoke(() => Colors.Add(color));
            }
        }

        private async Task LoadProducts()
        {
            var products = await _workUnitRepository.GetAllProductsAsync();

            foreach (var product in products)
            {
                Application.Current.Dispatcher.Invoke(() => Products.Add(product));
            }
        }
    }
}
