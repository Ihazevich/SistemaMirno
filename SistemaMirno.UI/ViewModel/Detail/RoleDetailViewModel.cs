using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories.Interfaces;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.ViewModel.Detail.Interfaces;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.Detail
{
    public class RoleDetailViewModel : DetailViewModelBase, IRoleDetailViewModel
    {
        private readonly IRoleRepository _roleRepository;
        private RoleWrapper _role;

        public RoleDetailViewModel(
            IRoleRepository roleRepository,
            IEventAggregator eventAggregator, 
            IDialogCoordinator dialogCoordinator) 
            : base(eventAggregator, "Detalles de Rol", dialogCoordinator)
        {
            _roleRepository = roleRepository;
            Branches = new ObservableCollection<BranchWrapper>();

            SelectFileCommand = new DelegateCommand(OnSelectFileExecute);
        }

        private void OnSelectFileExecute()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.InitialDirectory = Directory.GetCurrentDirectory();
            dlg.DefaultExt = ".pdf"; // Default file extension
            dlg.Filter = "Archivos PDF(.pdf)|*.pdf"; // Filter files by extension

            bool? result = dlg.ShowDialog();
            // Process open file dialog box results
            if (result == true)
            {
                // Save filename
                Application.Current.Dispatcher.Invoke(() => Role.ProceduresManualPdfFile = dlg.FileName);
            }
        }

        /// <summary>
        /// Gets or sets the data model wrapper.
        /// </summary>
        public RoleWrapper Role
        {
            get => _role;

            set
            {
                _role = value;
                OnPropertyChanged();
            }

        }

        public ObservableCollection<BranchWrapper> Branches { get; set; }

        public ICommand SelectFileCommand { get; }

        /// <inheritdoc/>
        public override async Task LoadDetailAsync(int id)
        {
            var model = await _roleRepository.GetByIdAsync(id);

            await LoadBranches();

            Application.Current.Dispatcher.Invoke(() =>
            {
                Role = new RoleWrapper(model);
                Role.PropertyChanged += Model_PropertyChanged;
                ((DelegateCommand) SaveCommand).RaiseCanExecuteChanged();
            });
            await base.LoadDetailAsync(id).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        protected override async void OnSaveExecute()
        {
            base.OnSaveExecute();
            if (IsNew)
            {
                await _roleRepository.AddAsync(Role.Model);
            }
            else
            {
                await _roleRepository.SaveAsync(Role.Model);
            }

            HasChanges = false;
            EventAggregator.GetEvent<CloseDetailViewEvent<RoleDetailViewModel>>()
                .Publish();
        }

        /// <inheritdoc/>
        protected override bool OnSaveCanExecute()
        {
            return OnSaveCanExecute(Role);
        }

        /// <inheritdoc/>
        protected override async void OnDeleteExecute()
        {
            await _roleRepository.DeleteAsync(Role.Model);
            EventAggregator.GetEvent<CloseDetailViewEvent<RoleDetailViewModel>>()
                .Publish();
        }

        protected override void OnCancelExecute()
        {
            EventAggregator.GetEvent<CloseDetailViewEvent<RoleDetailViewModel>>()
                .Publish();
        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = _roleRepository.HasChanges();
            }

            if (e.PropertyName == nameof(Role.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        public override async Task LoadAsync()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Role = new RoleWrapper();
                Role.PropertyChanged += Model_PropertyChanged;
                ((DelegateCommand) SaveCommand).RaiseCanExecuteChanged();

                Role.Description = string.Empty;
                Role.BranchId = -1;
                Role.HasAccessToAccounting = false;
                Role.HasAccessToHumanResources = false;
                Role.HasAccessToLogistics = false;
                Role.HasAccessToProduction = false;
                Role.HasAccessToSales = false;
                Role.IsSystemAdmin = false;
                Role.HasProceduresManual = false;
                Role.ProceduresManualPdfFile = string.Empty;

                ProgressVisibility = Visibility.Collapsed;
            });

            await LoadBranches();
        }

        private async Task LoadBranches()
        {
            var branches = await _roleRepository.GetAllBranchesAsync();

            foreach (var branch in branches)
            {
                Application.Current.Dispatcher.Invoke(() => Branches.Add(new BranchWrapper(branch)));
            }
        }
    }
}
