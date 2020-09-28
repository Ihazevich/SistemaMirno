// <copyright file="AdminWorkUnitViewModel.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories.Interfaces;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.SysAdmin
{
    /// <summary>
    /// Represents the View Model to add work units to a specific work area directly,
    /// used by system admins only.
    /// </summary>
    public class AdminWorkUnitViewModel : ViewModelBase
    {
        private readonly IWorkUnitRepository _workUnitRepository;
        private string _quantity;
        private Color _selectedColor;
        private Material _selectedMaterial;
        private Product _selectedProduct;
        private WorkArea _selectedWorkArea;
        private WorkUnitWrapper _workUnit;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminWorkUnitViewModel"/> class.
        /// </summary>
        /// <param name="workUnitRepository">The data repository.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        /// <param name="dialogCoordinator">The dialog coordinator.</param>
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

        /// <summary>
        /// Gets the command used to add a new work unit.
        /// </summary>
        public ICommand AddNewWorkUnitCommand { get; }

        /// <summary>
        /// Gets the current collection of <see cref="Color"/> entities.
        /// </summary>
        public ObservableCollection<Color> Colors { get; }

        /// <summary>
        /// Gets the current collection of <see cref="Material"/> entities.
        /// </summary>
        public ObservableCollection<Material> Materials { get; }

        /// <summary>
        /// Gets or sets the new work unit entity wrapper.
        /// </summary>
        public WorkUnitWrapper NewWorkUnit
        {
            get => _workUnit;

            set
            {
                _workUnit = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the current collection of <see cref="Product"/> entities.
        /// </summary>
        public ObservableCollection<Product> Products { get; }

        /// <summary>
        /// Gets or sets the quantity used to create new work units.
        /// </summary>
        public string Quantity
        {
            get => _quantity;

            set
            {
                _quantity = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the current selected <see cref="Color"/> entity.
        /// </summary>
        public Color SelectedColor
        {
            get => _selectedColor;

            set
            {
                _selectedColor = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the current selected <see cref="Material"/> entity.
        /// </summary>
        public Material SelectedMaterial
        {
            get => _selectedMaterial;

            set
            {
                _selectedMaterial = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the current selected <see cref="Product"/> entity.
        /// </summary>
        public Product SelectedProduct
        {
            get => _selectedProduct;

            set
            {
                _selectedProduct = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the current selected <see cref="WorkArea"/> entity.
        /// </summary>
        public WorkArea SelectedWorkArea
        {
            get => _selectedWorkArea;

            set
            {
                _selectedWorkArea = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the current collection of <see cref="WorkArea"/> entities.
        /// </summary>
        public ObservableCollection<WorkArea> WorkAreas { get; }

        /// <inheritdoc/>
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

        private async Task LoadColors()
        {
            var colors = await _workUnitRepository.GetAllColorsAsync();

            foreach (var color in colors)
            {
                Application.Current.Dispatcher.Invoke(() => Colors.Add(color));
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

        private async Task LoadProducts()
        {
            var products = await _workUnitRepository.GetAllProductsAsync();

            foreach (var product in products)
            {
                Application.Current.Dispatcher.Invoke(() => Products.Add(product));
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

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(NewWorkUnit.HasErrors))
            {
                ((DelegateCommand)AddNewWorkUnitCommand).RaiseCanExecuteChanged();
            }
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
                    Details = NewWorkUnit.Details,
                };
                newWorkUnits.Add(workUnit);
            }

            await _workUnitRepository.AddRangeAsync(newWorkUnits);

            Application.Current.Dispatcher.Invoke(() => ProgressVisibility = Visibility.Collapsed);
        }
    }
}
